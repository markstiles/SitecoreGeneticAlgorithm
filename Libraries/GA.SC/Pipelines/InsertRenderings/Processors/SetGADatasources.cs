using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GA.Lib;
using GA.Lib.Gene;
using GA.Lib.Population;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Security;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Layouts;
using Sitecore.Pipelines.InsertRenderings;

namespace GA.SC.Pipelines.InsertRenderings.Processors {
	public class SetGADatasources {

		protected List<KeyValuePair<int, string>> Chromosomes = new List<KeyValuePair<int, string>>();
		protected IKaryotype CurrentKaryotype;
		protected DefaultPopulationManager popman = new DefaultPopulationManager();
 
		public void Process(InsertRenderingsArgs args) {
			Assert.ArgumentNotNull(args, "args");
			
			if (Sitecore.Context.Site == null)
				return;

			// get the count of renderings with the datasource set as GAManager 
			List<KeyValuePair<int,RenderingReference>> rr = args.Renderings
				.Select((value, index) => new { value, index })
				.Where(a => a.value.Settings.DataSource.Equals(ConfigUtil.Current.DatasourceValue))
                .Select(x => new KeyValuePair<int, RenderingReference>(x.index, x.value))
                .ToList();
			if (!rr.Any())
				return; 

			//setup chromosomes
			Chromosomes.Add(new KeyValuePair<int, string>(rr.Count, ConfigUtil.Current.ChromosomeName));

			//setup population options
			popman.PopulationType = ConfigUtil.Current.PopulationType;
			popman.KaryotypeType = ConfigUtil.Current.KaryotypeType;

			// get tags - TODO change with content search or api call
			Item tagBucket = GetItemFromID(ConfigUtil.Current.TagFolder);
			List<Item> tags = Sitecore.Context.Database.SelectItems(string.Format("{0}//*[@@templatename='Tag']",tagBucket.Paths.FullPath)).ToList();

			foreach (KeyValuePair<int, string> c in Chromosomes) {
				Genotype g = new Genotype();
				//number of genes corresponds to the number of placeholders to fill with display content
				g.GeneLimit = c.Key;
				for (int z = 0; z < tags.Count; z++) { //add all the tags to the genotype
					TagGene t = new TagGene(tags[z].DisplayName, RandomUtil.NextBool());
					g.Add(t);
				}
				if(!popman.Genotype.ContainsKey(c.Value))
					popman.Genotype.Add(c.Value, g);
			}

			// pull from config
			popman.CrossoverRatio = ConfigUtil.Current.CrossoverRatio;
			popman.ElitismRatio = ConfigUtil.Current.ElitismRatio;
			popman.FitnessRatio = ConfigUtil.Current.FitnessRatio;
			popman.MutationRatio = ConfigUtil.Current.MutationRatio;
			popman.TourneySize = ConfigUtil.Current.TourneySize;
			popman.PopSize = ConfigUtil.Current.PopSize;
			
			//get or create the population
			SCPopulation p = SCPopulation.GetPop(popman);

			//choose best
			CurrentKaryotype = p.ChooseFitKaryotype();

			// process individual rendering overrides
			int i = -1;
			foreach (KeyValuePair<int,RenderingReference> kvp in rr) {
				//at the beginning because of the continues
				i++;

				RenderingReference r = kvp.Value;
				if (r == null || r.RenderingItem == null)
					continue;

				//wire up renderings with results 
				string tagName = CurrentKaryotype.ExpressedHaploid[Chromosomes[0].Value][i].GeneID;
				IEnumerable<Item> tagMatches = tags.Where(a => a.DisplayName.Equals(tagName));
				if (!tagMatches.Any())
					continue;

				string tid = tagMatches.First().ID.ToString();

				// get content with a tag selected 
				// TODO replace this with a content search
				Item cItem = GetItemFromID(ConfigUtil.Current.ContentFolder);
				List<Item> contentMatches = cItem.Children.Where(a => a.Fields[ConfigUtil.Current.ContentTagField].Value.Contains(tid)).ToList();
				if(!contentMatches.Any())
					continue;

				r.Settings.DataSource = contentMatches[RandomUtil.Instance.Next(0, contentMatches.Count)].ID.ToString();
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

		// TODO fill out the content search or define one for each algorithm entry
		private List<string> PerformSearch() {
			var index = ContentSearchManager.GetIndex("sitecore_master_index");
			using (var context = index.CreateSearchContext(SearchSecurityOptions.EnableSecurityCheck)) {
				var queryable = context.GetQueryable<StringBuilder>();
				return new List<string>();			
			}
		}
	}
}
