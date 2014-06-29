using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GA.Lib;
using GA.Lib.Chromosome;
using GA.Lib.Population;
using GA.SC;

namespace GA.UI.Sublayouts {
	public partial class Dev : System.Web.UI.UserControl {

		/* 
		 *could also just have a chromosome for each day or even time of day. a set by season. In this way you might defeat
		 *monocultures by adding in more variability
		 *
		 * these can be used just like levels of navigation. you can create a population at a top level and each chromosome 
		 * could have it's own population as subcategories. visually stored in a tree or bucket but just in a sense of 
		 * giving depth to the category itself. for example you like shoes (top level chromosome) but you prefer sandals 
		 * and sneakers (second level)
		 * 
		 * There's two places that are still tied to instances. In the algoChromosome it's sitecore specific for the use of Engagement values
		 * also in the population it's instantiation the AlgoChromosome during init population. 
		 * 
		 * the population options could also be an interface and pull from the config. provide a default with the values pulling
		 * from app settings
		 * 
		 * Engagement values might not be best stored on the SCChromosome class
		 * 
		 * 
		 * need to make sure the evolve function is leaving some diversity left
		 * also make sure the top fittest isn't always chosen to add variability. or at least up the randomness either through mutation rate etc.
		 * 
		 * when you click around and the population gets too low you need to restart it. 
		 * There may be a way to determine this lower bound by it's relation to other values
		 * 
		 * it may be the number of generations before its honed
		 * 
		 * populations should be refreshed every session but the ev's should be stored. 
		 * */

		private StringBuilder sbOut = new StringBuilder();

		protected List<Literal> Placeholders;
		protected List<Button> Buttons;

		protected List<string> Tags = new List<string> { "1", "2", "3", "4" };

		protected IChromosome CurrentChromosome;

		protected DefaultPopulationOptions apo = new DefaultPopulationOptions();

		protected void Page_Load(object sender, EventArgs e) {

			//setup accessors
			Placeholders = new List<Literal>() { ltlOne, ltlTwo, ltlThree, ltlFour };
			Buttons = new List<Button>() { btnA, btnB, btnC, btnD };
			//setup button text
			for (int i = 0; i < Buttons.Count; i++)
				Buttons[i].Text = Tags[i];

			//setup population options
			apo.GeneCount = Placeholders.Count; //number of genes corresponds to the number of placeholders to fill with display content
			for (int z = 0; z < Tags.Count; z++) //add all the tags to the genotype
				apo.Genotype.Add(new SCTagGene(Tags[RandomUtil.Instance.Next(0, Tags.Count)]));
			
			//run pop... or not
			if (!IsPostBack) {

				//default options
				txtCrossover.Text = apo.CrossoverRatio.ToString();
				txtElitism.Text = apo.ElitismRatio.ToString();
				txtFitness.Text = apo.FitnessRatio.ToString();
				txtMutation.Text = apo.MutationRatio.ToString();
				txtScalar.Text = apo.PopScalar.ToString();
				txtTourney.Text = apo.TourneySize.ToString();

				RunAlgo();
			} else {
				apo.CrossoverRatio = float.Parse(txtCrossover.Text);
				apo.ElitismRatio = float.Parse(txtElitism.Text);
				apo.FitnessRatio = float.Parse(txtFitness.Text);
				apo.MutationRatio = float.Parse(txtMutation.Text);
				apo.PopScalar = int.Parse(txtScalar.Text);
				apo.TourneySize = int.Parse(txtTourney.Text);
			}

			ltlOut.Text = sbOut.ToString();
		}

		protected void RunAlgo() {

			//get or create the population
			SCPopulation p = SCPopulation.GetPop(apo);

			//list the chromosomes
			List<IChromosome> u = p.GetUniqueChromosomes();
			rptChromeList.DataSource = p.GetUniqueChromosomes();
			rptChromeList.DataBind(); 

			//choose best
			CurrentChromosome = p.ChooseFitChromosome();
			ltlChrome.Text = string.Format("{0}-{1}", CurrentChromosome.GeneSequence(), CurrentChromosome.Fitness);

			//wire up renderings with results
			for (int z = 0; z < Placeholders.Count; z++)
				Placeholders[z].Text = CurrentChromosome[z].GeneID;

			//list chromosome data
			ltlChromes.Text = p.Chromosomes.Count.ToString();
			ltlUChromes.Text = u.Count.ToString();

			//list all engagement values stored
			rptEV.DataSource = SCChromosome.EngagementValues;
			rptEV.DataBind();

			//evolve
			p.Evolve();
		}

		#region Log

		protected void Log() {
			Log(string.Empty);
		}

		protected void Log(string str) {
			Log(str, new string[0]);
		}

		protected void Log(string str, params object[] args) {
			sbOut.AppendLine("<br/>").AppendFormat(str, args);
		}

		#endregion Log

		protected string OddEven(int i) {
			return (i % 2 == 0) ? "odd" : "even";
		}

		/// <summary>
		/// This adds to engagement value by the tag that was clicked
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btn_Click(object sender, EventArgs e) {

			//update clicks
			Button b = (Button)sender;
			string key = b.Text; // string.Format("ltl{0}-{1}", b.CssClass, b.Text);
			if (!SCChromosome.EngagementValues.ContainsKey(key))
				SCChromosome.EngagementValues.Add(key, new List<EngagementValue>());
			SCChromosome.EngagementValues[key].Add(new EngagementValue(1));
			
			//run algo
			RunAlgo();
		}

		protected void btnClearEvents_Click(object sender, EventArgs e) {
			SCChromosome.EngagementValues.Clear();
			RunAlgo();
		}

		protected void btnNextGen_Click(object sender, EventArgs e) {
			RunAlgo();
		}

		protected void btnRestart_Click(object sender, EventArgs e) {
			SCPopulation.RestartPop(apo);
			RunAlgo();
		}
	}
}