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

			// get the count of renderings with the datasource set as GAManager // GAManager Template ID = "{9AF6BAB4-D980-4190-9B90-E2E9D97C0C99}"
			List<KeyValuePair<int,RenderingReference>> rr = args.Renderings
				.Select((value, index) => new { value, index })
				.Where(a => a.value.Settings.DataSource.Equals("{9F8F8F2C-2980-4961-94A1-49A48209C2FD}"))
                .Select(x => new KeyValuePair<int, RenderingReference>(x.index, x.value))
                .ToList();
			if (!rr.Any())
				return; 

			//setup chromosomes
			Chromosomes.Add(new KeyValuePair<int, string>(rr.Count, "pageContent"));

			//setup population options TODO from config
			popman.PopulationType = Type.GetType("GA.SC.SCPopulation,GA.SC");
			popman.KaryotypeType = Type.GetType("GA.SC.PageKaryotype,GA.SC");

			// get tags TODO change with search or api call
			List<Item> tags = Sitecore.Context.Database.SelectItems("/sitecore/system/Settings/Buckets/TagRepository//*[@@templatename='Tag']").ToList();

			foreach (KeyValuePair<int, string> c in Chromosomes) {
				Genotype g = new Genotype();
				//number of genes corresponds to the number of placeholders to fill with display content
				g.GeneLimit = c.Key;
				for (int z = 0; z < tags.Count; z++) { //add all the tags to the genotype
					TagGene t = new TagGene(tags[z].DisplayName, true);
					g.Add(t);
				}
				if(!popman.Genotype.ContainsKey(c.Value))
					popman.Genotype.Add(c.Value, g);
			}

			// TODO pull from sitecore content or config
			/*
			apo.CrossoverRatio = float.Parse(txtCrossover.Text);
			apo.ElitismRatio = float.Parse(txtElitism.Text);
			apo.FitnessRatio = float.Parse(txtFitness.Text);
			apo.MutationRatio = float.Parse(txtMutation.Text);
			apo.TourneySize = int.Parse(txtTourney.Text);
			apo.PopSize = int.Parse(txtPopSize.Text);
			*/

			/* TODO make sure you retrieve and display the same sublayout on postback or have some state management */

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
				Item newItem = r.RenderingItem.InnerItem.Clone(Sitecore.Data.ID.NewID, Sitecore.Context.Database);
				RenderingReference newR = new RenderingReference(new RenderingItem(newItem));
				string tagName = CurrentKaryotype.ExpressedHaploid[Chromosomes[0].Value][i].GeneID;
				IEnumerable<Item> tagMatches = tags.Where(a => a.DisplayName.Equals(tagName));
				if (!tagMatches.Any())
					continue;

				string tid = tagMatches.First().ID.ToString();

				// TODO move this to config or constants
				Item cItem = Sitecore.Context.Database.GetItem("/sitecore/content/GAContent");
				// TODO find a place to store this. It's not in Sitecore.FieldIDs
				List<Item> contentMatches = cItem.Children.Where(a => a.Fields["__Semantics"].Value.Contains(tid)).ToList();
				if(!contentMatches.Any())
					continue;

				newR.Settings.DataSource = contentMatches[RandomUtil.Instance.Next(0, contentMatches.Count)].ID.ToString();
				//then set it
				args.Renderings[kvp.Key] = newR;
			}

			//evolve
			p.Evolve();
		}

		private List<string> PerformSearch() {
			//the fields are managed in:
			//			/App_Config/includes/Sitecore.ContentSearch.Lucene.DefaultIndexConfiguration
			//this index is managed in:
			//			/App_Config/includes/Sitecore.ContentSearch.Lucene.Index.Master.config
			//this settings for indexing in:
			//			/App_Config/includes/Sitecore.ContentSearch.config
			var index = ContentSearchManager.GetIndex("sitecore_master_index");
			using (var context = index.CreateSearchContext(SearchSecurityOptions.EnableSecurityCheck)) {
				var queryable = context.GetQueryable<StringBuilder>();

				//need to send to list before the using outside the context
				return new List<string>();// queryable.ToList();
			}
		}
	}
}
