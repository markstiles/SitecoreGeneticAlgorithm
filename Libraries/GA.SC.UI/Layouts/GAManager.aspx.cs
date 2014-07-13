using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GA.Nucleus;
using GA.Nucleus.Chromosome;
using GA.Nucleus.Gene;
using GA.Nucleus.Population;
using GA.SC;
using GA.SC.EV;
using HtmlAgilityPack;
using Sitecore.Data.Managers;

namespace GA.SC.UI.Layouts {
	public partial class GAManager : Page {

		protected List<Literal> Placeholders;
		protected List<Button> Buttons;
		protected List<Panel> Panels;

		protected List<KeyValuePair<int, string>> Chromosomes = new List<KeyValuePair<int, string>>();

		protected IKaryotype CurrentKaryotype;

		protected DefaultPopulationManager apo = new DefaultPopulationManager();

		protected void Page_Load(object sender, EventArgs e) {

			LoadIcons();

			//setup accessors
			Placeholders = new List<Literal>() { ltlOne, ltlTwo, ltlThree, ltlFour, ltlFive, ltlSix, ltlSeven, ltlEight };
			Panels = new List<Panel>() { pnlOne, pnlTwo, pnlThree, pnlFour, pnlFive, pnlSix, pnlSeven, pnlEight };
			Buttons = new List<Button>() { btnA, btnB, btnC, btnD, btnE, btnF, btnG };
			
			// TODO make a drop down to select the ga name you want to view data on and default to its own config
			ConfigUtil.Context = new ConfigUtil("gamanager");

			//setup chromosomes
			Chromosomes.Add(new KeyValuePair<int, string>(Placeholders.Count, ConfigUtil.Context.ChromosomeName)); 
			
			//setup population options
			apo.PopulationType = ConfigUtil.Context.PopulationType;
			apo.KaryotypeType = ConfigUtil.Context.KaryotypeType;
			foreach (KeyValuePair<int, string> c in Chromosomes) {
				Genotype g = new Genotype();
				//number of genes corresponds to the number of placeholders to fill with display content
				g.GeneLimit = c.Key;
				for (int z = 0; z < Buttons.Count; z++) { //add all the tags to the genotype
					TagGene t = new TagGene(Buttons[z].Text, true);
					g.Add(t);
				}
				apo.Genotype.Add(c.Value, g);
			}
			
			//run pop... or let events run it
			if (!IsPostBack) {

				//default options
				txtCrossover.Text = ConfigUtil.Context.CrossoverRatio.ToString();
				txtElitism.Text = ConfigUtil.Context.ElitismRatio.ToString();
				txtFitness.Text = ConfigUtil.Context.FitnessRatio.ToString();
				txtMutation.Text = ConfigUtil.Context.MutationRatio.ToString();
				txtTourney.Text = ConfigUtil.Context.TourneySize.ToString();
				txtPopSize.Text = ConfigUtil.Context.PopSize.ToString();

				RunAlgo();
			} else {
				apo.CrossoverRatio = float.Parse(txtCrossover.Text);
				apo.ElitismRatio = float.Parse(txtElitism.Text);
				apo.FitnessRatio = float.Parse(txtFitness.Text);
				apo.MutationRatio = float.Parse(txtMutation.Text);
				apo.TourneySize = int.Parse(txtTourney.Text);
				apo.PopSize = int.Parse(txtPopSize.Text);
			}
		}

		protected void LoadIcons() {
			imgPopEngVal.ImageUrl = GetIcon("Network/16x16/earth.png", 16, 16);
			imgPopInfo.ImageUrl = GetIcon("Network/32x32/environment.png", 16, 16);
		}

		public string GetIcon(string iconPath, int width, int height) {

			HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
			htmlDoc.LoadHtml(ThemeManager.GetImage(iconPath, width, height));
			HtmlNode img = htmlDoc.DocumentNode.FirstChild;
			return img.Attributes["src"].Value;
		}

		protected void RunAlgo() {

			//get or create the population
			SCPopulation p = SCPopulation.GetPop(apo);

			//list the chromosomes
			List<IKaryotype> u = p.GetUniqueKaryotypes();
			rptDNAList.DataSource = u;
			rptDNAList.DataBind(); 

			//choose best
			CurrentKaryotype = p.ChooseFitKaryotype();
			ltlKaryotype.Text = string.Format("{0}-{1}", CurrentKaryotype.ExpressedHaploid.DNASequence(), CurrentKaryotype.Fitness);

			//wire up renderings with results
			for (int z = 0; z < Placeholders.Count; z++) {
				string gid = CurrentKaryotype.ExpressedHaploid[Chromosomes[0].Value][z].GeneID;
				Placeholders[z].Text = gid;
				Panels[z].Attributes["style"] = string.Format("background-color:{0};", gid);
			}

			//list karyotype data
			ltlKaryos.Text = p.Karyotypes.Count.ToString();
			ltlUKaryos.Text = u.Count.ToString();

			//list all engagement values stored
			rptEV.DataSource = ConfigUtil.Context.EVProvider.Values;
			rptEV.DataBind();

			//evolve
			p.Evolve();
		}

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
			if (!ConfigUtil.Context.EVProvider.Values.ContainsKey(key))
				ConfigUtil.Context.EVProvider.Values.Add(key, new List<IEngagementValue>());
			ConfigUtil.Context.EVProvider.Values[key].Add(new EngagementValue(1));
			
			//run algo
			RunAlgo();
		}

		protected void btnClearEvents_Click(object sender, EventArgs e) {
			ConfigUtil.Context.EVProvider.Values.Clear();
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