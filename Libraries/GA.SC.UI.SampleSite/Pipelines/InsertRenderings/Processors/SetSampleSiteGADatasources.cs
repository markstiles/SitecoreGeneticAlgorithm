using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GA.Nucleus;
using GA.Nucleus.Gene;
using GA.Nucleus.Population;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Security;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Layouts;
using Sitecore.Pipelines.InsertRenderings;
using Sitecore.Security.Domains;
using Sitecore.Configuration;
using Sitecore.Sites;

namespace GA.SC.UI.SampleSite.Pipelines.InsertRenderings.Processors {
	public class SetSampleSiteGADatasources {

		public void Process(InsertRenderingsArgs args) {

            Assert.ArgumentNotNull(args, "args");

            SiteContext sc = Sitecore.Context.Site;
            string gaName = Statics.GetGAName(sc);
            if (Sitecore.Context.Site == null || !sc.Name.Equals("GASampleSite") || string.IsNullOrEmpty(gaName))
                return;

            // get the count of renderings with the datasource set as GAManager 
            List<KeyValuePair<int, RenderingReference>> rr = args.Renderings
				.Select((value, index) => new { value, index })
				.Where(a => a.value.Settings.DataSource.Equals(Statics.DatasourceValue))
                .Select(x => new KeyValuePair<int, RenderingReference>(x.index, x.value))
                .ToList();
			if (!rr.Any())
				return;

            IPopulationManager popman = Statics.GetPopulationManager(sc);
            IPopulation p = popman.Population;
            
			//choose best
            IKaryotype CurrentKaryotype = p.ChooseFitKaryotype();

            Item tagBucket = GetItemFromID(Statics.TagFolder);
            List<Item> tags = Sitecore.Context.Database.SelectItems(string.Format("{0}//*", tagBucket.Paths.FullPath)).ToList();

			// process individual rendering overrides
			int i = -1;
			foreach (KeyValuePair<int,RenderingReference> kvp in rr) {
				//at the beginning because of the continues
				i++;

				RenderingReference r = kvp.Value;
				if (r == null || r.RenderingItem == null)
					continue;

				//wire up renderings with results 
				string tagID = CurrentKaryotype.Phenotype[Statics.ChromoKey][i].GeneID;
				IEnumerable<Item> tagMatches = tags.Where(a => a.ID.ToString().Equals(tagID));
				if (!tagMatches.Any())
					continue;

				//tag id
				string tid = tagMatches.First().ID.ToString();

				// get content with a tag selected 
				// TODO replace this with a content search
                
                Item cItem = GetItemFromID(Statics.ContentFolder);
				List<Item> contentMatches = cItem.Children.Where(a => a.Fields[Statics.ContentTagField].Value.Contains(tid)).ToList();
				if(!contentMatches.Any())
					continue;

				//from all possible show a random one
				r.Settings.DataSource = contentMatches[RandomUtil.Instance.Next(contentMatches.Count)].ID.ToString();
			}

			//evolve
			p.Evolve();
		}

		protected Item GetItemFromID(string idStr) {
			if (!Sitecore.Data.ID.IsID(idStr))
				return null;
			Sitecore.Data.ID id = Sitecore.Data.ID.Parse(idStr);
			return Sitecore.Context.Database.GetItem(id);
		}
	}
}
