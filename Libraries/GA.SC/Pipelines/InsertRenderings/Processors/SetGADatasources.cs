using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GA.Lib.Gene;
using GA.Lib.Population;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.InsertRenderings;

namespace GA.SC.Pipelines.InsertRenderings.Processors {
	public class SetGADatasources {

		protected List<KeyValuePair<int, string>> Chromosomes = new List<KeyValuePair<int, string>>();

		protected IKaryotype CurrentKaryotype;
		
		protected DefaultPopulationManager popman = new DefaultPopulationManager();
 
		public void Process(InsertRenderingsArgs args) {
			Assert.ArgumentNotNull(args, "args");

			// TODO select the sublayouts by the datasource. it should be a GA content item
			/*
			//setup chromosomes
			Chromosomes.Add(new KeyValuePair<int, string>(Placeholders.Count, "pageContent"));

			//setup population options TODO from config
			popman.PopulationType = Type.GetType("GA.SC.SCPopulation,GA.SC");
			popman.KaryotypeType = Type.GetType("GA.SC.SCKaryotype,GA.SC");
			foreach (KeyValuePair<int, string> c in Chromosomes) {
				Genotype g = new Genotype();
				//number of genes corresponds to the number of placeholders to fill with display content
				g.GeneLimit = c.Key;
				for (int z = 0; z < Buttons.Count; z++) { //add all the tags to the genotype
					TagGene t = new TagGene(Buttons[z].Text, true);
					g.Add(t);
				}
				popman.Genotype.Add(c.Value, g);
			}
			*/
			// TODO pull from sitecore content 
			/*
			apo.CrossoverRatio = float.Parse(txtCrossover.Text);
			apo.ElitismRatio = float.Parse(txtElitism.Text);
			apo.FitnessRatio = float.Parse(txtFitness.Text);
			apo.MutationRatio = float.Parse(txtMutation.Text);
			apo.TourneySize = int.Parse(txtTourney.Text);
			apo.PopSize = int.Parse(txtPopSize.Text);
			*/

			/* TODO make sure you retrieve the same sublayout on postback.  */
			/*
			//get or create the population
			SCPopulation p = SCPopulation.GetPop(popman);

			//choose best
			CurrentKaryotype = p.ChooseFitKaryotype();

			//wire up renderings with results TODO figure out which position in the gene to affect
			for (int z = 0; z < Placeholders.Count; z++) {
				string gid = CurrentKaryotype.ExpressedHaploid[Chromosomes[0].Value][z].GeneID;
				Placeholders[z].Text = gid;
				Panels[z].Attributes["style"] = string.Format("background-color:{0};", gid);
			}

			//evolve
			p.Evolve();

			if (Sitecore.Context.Site == null)
				return;

			//add a rendering 
			using (new ProfileSection("Add EU Message Renderings.")) {
				IEnumerable<RenderingReference> enumRR = refItem.Visualization.GetRenderings(Context.Device, true);
				foreach (RenderingReference rr in enumRR)
					args.Renderings.Add(rr);
				args.HasRenderings = (enumRR != null && enumRR.Any());
			}

			// process individual rendering overrides
			for (int index = 0; index < args.Renderings.Count; index++) {
				RenderingReference rendering = args.Renderings[index];
				if (rendering == null || rendering.RenderingItem == null)
					continue;

				string renderingPath = rendering.RenderingItem.InnerItem["Path"];
				if (string.IsNullOrEmpty(renderingPath) == true)
					continue;

				string newRenderingPath = SitecoreUtility.PathCombine(SitePhysicalFolder, renderingPath);
				if (File.Exists(StateUtility.Server.MapPath(newRenderingPath)) == false)
					continue;

				// override found, get it
				RenderingReference newRenderingRef = null;
				Item newInnerItem = rendering.RenderingItem.InnerItem.Clone(Sitecore.Data.ID.NewID, Sitecore.Context.Database);
				using (new Sitecore.SecurityModel.SecurityDisabler()) {
					newInnerItem.Editing.BeginEdit();
					newInnerItem["Path"] = newRenderingPath;
					newInnerItem.Editing.EndEdit();
				}
				newRenderingRef = new RenderingReference(new RenderingItem(newInnerItem));
				newRenderingRef.Settings.Caching = rendering.Settings.Caching;
				newRenderingRef.Settings.DataSource = rendering.Settings.DataSource;
				newRenderingRef.Settings.Parameters = rendering.Settings.Parameters;
				newRenderingRef.Settings.Placeholder = rendering.Settings.Placeholder;
				//then set it
				args.Renderings[index] = newRenderingRef;
			}
			 * */
		}
	}
}
