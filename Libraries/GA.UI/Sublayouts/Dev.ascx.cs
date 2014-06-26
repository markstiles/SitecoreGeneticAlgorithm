using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GA.Lib;

namespace GA.UI.Sublayouts {
	public partial class Dev : System.Web.UI.UserControl {

		/* Got stuck with the fitness. new method of determination since i separated the buttons from the display. 
		 * need to figure out how to determine what to show. if i click one all the boxes should go to one. 
		 * then change if i go to two
		 * 
		 * 
		 * 
		 * manage the tag values in a single place
		 * locations:
		 *	Tags prop on dev.ascx.cs
		 *	
		 * 
		 *Replace passing tags and placeholders with IGene or something. they need to be generic. 
		 *The question is if you provide a list of unique gene elements and let it mix and match. 
		 *That mix and match should also be an Interface. IChromosome probably so you can give it to the population 
		 *
		 *could also just have a chromosome for each day or even time of day. a set by season. In this way you don't defeat
		 *monocultures. You can 
		 *
		 * this is just like levels of navigation. you can create categories at a top level and subcategories below them each will have it's own population. visually stored in a tree or bucket.
		 * 
		 * */

		
		private StringBuilder sbOut = new StringBuilder();

		protected List<Literal> Placeholders;

		protected List<string> Tags = new List<string> { "1", "2", "3", "4" };

		protected AlgoChromosome CurrentChromosome;

		protected AlgoPopulationOptions apo = new AlgoPopulationOptions();

		protected void Page_Load(object sender, EventArgs e) {

			Placeholders = new List<Literal>() { ltlOne, ltlTwo, ltlThree, ltlFour };

			if (!IsPostBack)
				RunAlgo();

			//RunSequence();

			ltlOut.Text = sbOut.ToString();
		}

		protected void RunAlgo() {

			//try to calculate the max number of permutations
			apo.PopSize = ((int)Math.Pow(Placeholders.Count, Tags.Count)) * 1;
			for(int z = 0; z < Tags.Count; z++)
				apo.Genes.Add(new AlgoGene(Placeholders[z].ID, Tags[rand.Next(0, Tags.Count)]));
			//get or create the population
			AlgoPopulation p = AlgoPopulation.GetPop(apo);

			//list the chromosomes
			ltlChromeList.Text = string.Empty;
			List<string> uniqueSet = new List<string>();
			foreach (AlgoChromosome c in p.Chromosomes) {
				string uKey = string.Format("{0}-{1}", c.GeneSequence, c.Fitness);
				if(!uniqueSet.Contains(uKey))
					uniqueSet.Add(uKey);
			}
			int i = 1;
			foreach (string u in uniqueSet) {
				ltlChromeList.Text += string.Format("<b>{0}</b>: {1}<br/>", i.ToString(), u);
				i++;
			}

			//choose best
			double topFit = p.Chromosomes.First().Fitness;
			List<AlgoChromosome> lac = (topFit < 1) // if all values have decayed below 1 then don't filter any options out
				? p.Chromosomes
				: p.Chromosomes.Where(a => a.Fitness >= (topFit * apo.fitnessRatio)).ToList();
			int newPos = AlgoPopulation._rand.Next(0, lac.Count);
			CurrentChromosome = (lac.Any()) // if the filter worked too well just select the first item
				? lac[newPos]
				: p.Chromosomes.First();
			ltlChrome.Text = string.Format("{0}-{1}", CurrentChromosome.GeneSequence, CurrentChromosome.Fitness);

			//wire up renderings with results
			foreach (Literal b in Placeholders)
				b.Text = ((AlgoGene)CurrentChromosome[b.ID]).Tag;

			//list all engagement values stored
			ltlEV.Text = string.Format("Gene Count is: {0}<br/>", i.ToString());
			foreach (KeyValuePair<string, List<EngagementValue>> kvp in AlgoChromosome.EngagementValues) {
				double tv = kvp.Value.Sum(a => a.CurrentValue());
				ltlEV.Text += string.Format("{0}-{1}<br/>", kvp.Key, tv);
			}

			//evolve
			p.Evolve();
		}

		//protected void RunSequence() {
		//	ltlGene.Text = targetGene;
		//	// set goal
		//	Chromosome.SetTargetGene(targetGene);
		//	// build population
		//	Population population = new Population(populationSize, crossoverRatio, elitismRatio, mutationRatio);
		//	// start with the best
		//	Chromosome topChromosome = population.Chromosomes.First();
		//	// start timing it
		//	Stopwatch sw = new Stopwatch();
		//	sw.Start();
		//	// keep evolving until you hit an optimal fitness
		//	int count = 1;
		//	while ((count++ <= maxGenerations) && (topChromosome.Fitness != 0)) {
		//		Log(topChromosome.Gene);
		//		population.Evolve();
		//		topChromosome = population.Chromosomes.First();
		//	}
		//	sw.Stop();
		//	Log(topChromosome.Gene);
		//	Log("{0} generations in {1} ms", count, sw.ElapsedMilliseconds);
		//	Log();
		//}

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
			AlgoPopulation p = AlgoPopulation.GetPop(apo);

			//update clicks
			Button b = (Button)sender;
			string key = b.Text; // string.Format("ltl{0}-{1}", b.CssClass, b.Text);
			if (!AlgoChromosome.EngagementValues.ContainsKey(key))
				AlgoChromosome.EngagementValues.Add(key, new List<EngagementValue>());
			AlgoChromosome.EngagementValues[key].Add(new EngagementValue(1));
			
			//store
			AlgoPopulation.SetPop(p);

			//run algo
			RunAlgo();
		}

		protected void btnClearEvents_Click(object sender, EventArgs e) {
			AlgoChromosome.EngagementValues.Clear();
			RunAlgo();
		}

		protected void btnNextGen_Click(object sender, EventArgs e) {
			RunAlgo();
		}

		protected void btnRestart_Click(object sender, EventArgs e) {
			apo.PopSize = ((int)Math.Pow(Placeholders.Count, Tags.Count));
			AlgoPopulation.RestartPop(apo);
			RunAlgo();
		}
	}
}