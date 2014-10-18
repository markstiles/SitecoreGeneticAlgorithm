namespace GA.SC.UI.SampleSite.Sublayouts {
	using System;
	using System.Linq;
	using System.Collections.Generic;
	using System.Web.UI.WebControls;
	using Sitecore.Data.Fields;
	using Sitecore.Data.Items;
	using GA.Nucleus.Population;
	using GA.Nucleus.Gene;
	using System.Web.UI;
	using System.Text;

	public partial class GAMonitor : UserControl {

		private void Page_Load(object sender, System.EventArgs e) {

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

		protected string GetItemName(string id) {
			Sitecore.Data.ID i = null;
			return (Sitecore.Data.ID.TryParse(id, out i))
				? Sitecore.Context.Database.Items[i].DisplayName
				: string.Empty;
		}

		protected string GetSequence(IKaryotype k) {
			StringBuilder sb = new StringBuilder();

			foreach (IGene g in k.Phenotype[ConfigUtil.Context.ChromosomeName]) {
				sb.Append(GetItemName(g.GeneID));
			}

			return sb.ToString();
		}
	}
}