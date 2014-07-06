namespace GA.SC.UI.Sublayouts {
	using System;
	using System.Collections.Generic;
	using System.Web.UI.WebControls;
	using GA.Lib.Population;
	using GA.SC.Sublayouts;

	public partial class AlgoDrivenContent : GABaseSublayout {

		private void Page_Load(object sender, EventArgs e) {
			pnlOut.Attributes.Add("style", "background-color:#ccc;");
			ltlOut.Text = DataSourceItem.DisplayName;
		}

		protected override void EngagingEvent_Click(object sender, EventArgs e) {
			//update clicks

			// TODO is this a button or hyperlink or querystring url?
			Button b = (Button)sender;
			string key = b.Text; // string.Format("ltl{0}-{1}", b.CssClass, b.Text);
			if (!EngagementValue.KnownValues.ContainsKey(key))
				EngagementValue.KnownValues.Add(key, new List<EngagementValue>());
			EngagementValue.KnownValues[key].Add(new EngagementValue(1));
		}
	}
}