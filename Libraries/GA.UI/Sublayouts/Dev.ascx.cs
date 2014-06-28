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
		 * */

		private StringBuilder sbOut = new StringBuilder();

		protected List<Literal> Placeholders;
		protected List<Button> Buttons;

		protected List<string> Tags = new List<string> { "1", "2", "3", "4" };

		protected IChromosome CurrentChromosome;

		protected PopulationOptions apo = new PopulationOptions();

		protected void Page_Load(object sender, EventArgs e) {

			//setup accessors
			Placeholders = new List<Literal>() { ltlOne, ltlTwo, ltlThree, ltlFour };
			Buttons = new List<Button>() { btnA, btnB, btnC, btnD };
			//setup button text
			for (int i = 0; i < Buttons.Count; i++)
				Buttons[i].Text = Tags[i];

			//setup population options
			apo.PopSize = (int)Math.Pow(Placeholders.Count, Tags.Count); //try to calculate the max number of permutations
			apo.GeneCount = Placeholders.Count; //number of genes corresponds to the number of placeholders to fill with display content
			for (int z = 0; z < Tags.Count; z++) //add all the tags to the genotype
				apo.Genotype.Add(new SCTagGene(Tags[RandomUtil.Instance.Next(0, Tags.Count)]));
			
			//run pop
			if (!IsPostBack)
				RunAlgo();

			ltlOut.Text = sbOut.ToString();
		}

		protected void RunAlgo() {

			//get or create the population
			SCPopulation p = SCPopulation.GetPop(apo);

			//list the chromosomes
			ltlChromeList.Text = string.Empty;
			List<string> uniqueSet = new List<string>();
			foreach (IChromosome c in p.Chromosomes) {
				string uKey = string.Format("{0}-{1}", c.GeneSequence(), c.Fitness);
				if(!uniqueSet.Contains(uKey))
					uniqueSet.Add(uKey);
			}
			int i = 1;
			foreach (string u in uniqueSet) {
				ltlChromeList.Text += string.Format("<b>{0}</b>: {1}<br/>", i.ToString(), u);
				i++;
			}

			//choose best
			CurrentChromosome = p.ChooseFitChromosome();
			ltlChrome.Text = string.Format("{0}-{1}", CurrentChromosome.GeneSequence(), CurrentChromosome.Fitness);

			//wire up renderings with results
			for (int z = 0; z < Placeholders.Count; z++)
				Placeholders[z].Text = CurrentChromosome[z].GeneID;

			//list all engagement values stored
			ltlEV.Text = string.Format("Gene Count is: {0}<br/>", i.ToString());
			foreach (KeyValuePair<string, List<EngagementValue>> kvp in SCChromosome.EngagementValues) {
				double tv = kvp.Value.Sum(a => a.CurrentValue());
				ltlEV.Text += string.Format("{0}-{1}<br/>", kvp.Key, tv);
			}

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

		/// <summary>
		/// This adds to engagement value by the tag that was clicked
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btn_Click(object sender, EventArgs e) {

			//get pop
			SCPopulation p = SCPopulation.GetPop(apo);

			//update clicks
			Button b = (Button)sender;
			string key = b.Text; // string.Format("ltl{0}-{1}", b.CssClass, b.Text);
			if (!SCChromosome.EngagementValues.ContainsKey(key))
				SCChromosome.EngagementValues.Add(key, new List<EngagementValue>());
			SCChromosome.EngagementValues[key].Add(new EngagementValue(1));
			
			//store
			SCPopulation.SetPop(p);

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
			apo.PopSize = ((int)Math.Pow(Placeholders.Count, Tags.Count));
			SCPopulation.RestartPop(apo);
			RunAlgo();
		}
	}
}