namespace GA.SC.UI.SampleSite.Layouts {
	using System;
	using System.Linq;
	using System.Collections.Generic;
	using System.Web;
	using System.Web.UI;
	using Sitecore.Data.Items;
	using Sitecore.Links;
	using GA.Nucleus.Population;
	using GA.SC.EV;
	using System.Web.UI.WebControls;
	using GA.Nucleus.Gene;

	public partial class GAMaster : Page {
		private void Page_Load(object sender, System.EventArgs e) {

			List<KeyValuePair<string, string>> navItems = new List<KeyValuePair<string,string>>();
			
			Item h = Sitecore.Context.Database.GetItem(Sitecore.Context.Site.StartPath);
			
			ltlContextSite.Text = string.Format("'{0}'", Sitecore.Context.Site.Name);

			lnkLogo.NavigateUrl = LinkManager.GetItemUrl(h);
			foreach (Item i in h.Children) {
				List<string> tags = TagUtil.GetTags(i);
				string tag = (tags.Any()) ? tags.First() : string.Empty;
				navItems.Add(new KeyValuePair<string, string>(LinkManager.GetItemUrl(i), tag));
			}

			rptNav.DataSource = navItems;
			rptNav.DataBind();

			SetupMonitor();
		}

		protected string OddEven(int i) {
			return (i % 2 == 0) ? "odd" : "even";
		}

		protected void SetupMonitor() {
			SCPopulation p = SCPopulation.GetPop(null);
			IPopulationManager man = p.Manager;
			ltlCrossover.Text = man.CrossoverRatio.ToString();
			ltlElitism.Text = man.ElitismRatio.ToString();
			ltlFitness.Text = man.FitnessRatio.ToString();
			ltlFitSort.Text = man.FitnessSort.ToString();
			ltlThreshold.Text = man.FitnessThreshold.ToString();
			ltlMutation.Text = man.MutationRatio.ToString();
			ltlTourney.Text = man.TourneySize.ToString();
			ltlPopSize.Text = man.PopSize.ToString();
			rptEV.DataSource = ConfigUtil.Context.EVProvider.RelevantValues;
			rptEV.DataBind();
			List<IKaryotype> u = p.GetUniqueKaryotypes();
			ltlUKaryos.Text = u.Count.ToString();
			rptDNAList.DataSource = u;
			rptDNAList.DataBind();

			rptChromos.DataSource = man.Genotype;
			rptChromos.DataBind();
		}

		protected void rptChromos_ItemDataBound(object sender, RepeaterItemEventArgs e) {
			if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;

			KeyValuePair<string, Genotype> g = (KeyValuePair<string, Genotype>)e.Item.DataItem;
			Repeater rptGenes = (Repeater)e.Item.FindControl("rptGenes");
			rptGenes.DataSource = g.Value;
			rptGenes.DataBind();
		}

		protected void btnClearClicks_Click(object sender, EventArgs e) {
			ConfigUtil.Context.EVProvider.Values.Clear();
			Response.Redirect(Request.RawUrl);
		}

		protected void btnRestart_Click(object sender, EventArgs e) {
			SCPopulation.RestartPop(SCPopulation.GetPop(null).Manager);
			Response.Redirect(Request.RawUrl);
		}
	}
}
