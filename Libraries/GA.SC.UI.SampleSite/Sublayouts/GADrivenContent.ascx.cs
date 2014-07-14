namespace GA.SC.UI.SampleSite.Sublayouts {
	using System;
	using System.Linq;
	using System.Collections.Generic;
	using System.Web.UI.WebControls;
	using GA.SC.Sublayouts;
	using Sitecore.Data.Fields;
	using Sitecore.Data.Items;

	public partial class GADrivenContent : BaseSublayout {

		private void Page_Load(object sender, EventArgs e) {
			string content = DataSourceItem["Content"];
			List<string> tags = TagUtil.GetTags(DataSourceItem);
			if(!tags.Any())
				return;
			string tag = tags.First();
			lnkBtn.Attributes.Add("style", string.Format("background-color:{0};", tag));
			lnkBtn.Text = "&nbsp;";
			lnkBtn.NavigateUrl = string.Format("/{0}.aspx", tag);
			lnkBtn.Attributes.Add("tag",tag); 
		}
	}
}