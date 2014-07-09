namespace GA.SC.UI.Sublayouts {
	using System;
	using System.Collections.Generic;
	using System.Web.UI.WebControls;
	using GA.Lib.Population;
	using GA.SC.Sublayouts;

	public partial class GADrivenContent : BaseSublayout {

		private void Page_Load(object sender, EventArgs e) {
			string color = DataSourceItem["Content"];
			pnlOut.Attributes.Add("style", string.Format("background-color:{0};", color));
			lnkBtn.Text = color;
			lnkBtn.NavigateUrl = string.Format("/GAPage/{0}.aspx", color);
		}
	}
}