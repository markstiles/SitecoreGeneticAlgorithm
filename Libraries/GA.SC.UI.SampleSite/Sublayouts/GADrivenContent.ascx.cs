namespace GA.SC.UI.SampleSite.Sublayouts {
	using System;
	using System.Collections.Generic;
	using System.Web.UI.WebControls;
	using GA.SC.Sublayouts;
	using Sitecore.Data.Items;

	public partial class GADrivenContent : BaseSublayout {

		private void Page_Load(object sender, EventArgs e) {
			string content = DataSourceItem["Content"];
			Item tag = DataSourceItem.Database.GetItem(DataSourceItem["Tags"]);
			lnkBtn.Attributes.Add("style", string.Format("background-color:{0};", tag.DisplayName));
			lnkBtn.Text = "&nbsp;";
			lnkBtn.NavigateUrl = string.Format("/GAPage/{0}.aspx", tag.DisplayName);
		}
	}
}