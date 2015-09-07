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
    using GA.SC.UI.SampleSite.EV;
    using Sitecore.Configuration;
    using GA.SC.UI.SampleSite.Population;
    using GA.Nucleus.Chromosome;

	public partial class GAMonitor : UserControl {

        protected IPopulationManager popman;

        #region Events

        private void Page_Load(object sender, System.EventArgs e) {

			SetupMonitor();
		}

        protected void rptChromos_ItemDataBound(object sender, RepeaterItemEventArgs e) {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;

            KeyValuePair<string, GenePool> g = (KeyValuePair<string, GenePool>)e.Item.DataItem;
            Repeater rptGenes = (Repeater)e.Item.FindControl("rptGenes");
            rptGenes.DataSource = g.Value;
            rptGenes.DataBind();
        }

        protected void btnClearClicks_Click(object sender, EventArgs e) {
            DefaultEngagementValueProvider d = new DefaultEngagementValueProvider();
            d.Values.Clear();
            Response.Redirect(Request.RawUrl);
        }

        protected void btnRestart_Click(object sender, EventArgs e) {
            popman.SetPopulation(popman.CreatePopulation());
            Response.Redirect(Request.RawUrl);
        }

        #endregion Events

        protected string OddEven(int i) {
			return (i % 2 == 0) ? "odd" : "even";
		}

		protected void SetupMonitor() {

            popman = Statics.GetPopulationManager(Sitecore.Context.Site);

            IPopulation p = popman.GetPopulation();

            ltlCrossover.Text = popman.CrossoverRatio.ToString();
            ltlElitism.Text = popman.ElitismRatio.ToString();
            ltlFitness.Text = popman.FitnessRatio.ToString();
            ltlFitSort.Text = popman.FitnessSort.ToString();
            ltlThreshold.Text = popman.FitnessThreshold.ToString();
            ltlMutation.Text = popman.MutationRatio.ToString();
            ltlTourney.Text = popman.TourneySize.ToString();
            ltlPopSize.Text = popman.PopSize.ToString();

            List<IKaryotype> u = p.GetUniqueKaryotypes(popman.FitnessSort);
            
            ltlUKaryos.Text = u.Count.ToString();
			
            rptEV.DataSource = new DefaultEngagementValueProvider().RelevantValues;
			rptEV.DataBind();

            rptDNAList.DataSource = u;
			rptDNAList.DataBind();

			rptChromos.DataSource = popman.ChromosomePool;
			rptChromos.DataBind();
		}

        protected string GetItemName(string id) {
            Sitecore.Data.ID i = null;
            return (Sitecore.Data.ID.TryParse(id, out i))
                ? Sitecore.Context.Database.Items[i].DisplayName
                : string.Empty;
        }

	}
}