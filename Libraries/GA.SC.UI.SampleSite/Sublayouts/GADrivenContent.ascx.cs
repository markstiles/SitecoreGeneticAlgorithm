namespace GA.SC.UI.SampleSite.Sublayouts {
	using System;
	using System.Linq;
	using System.Collections.Generic;
	using System.Web.UI.WebControls;
	using Sitecore.Data.Fields;
	using Sitecore.Data.Items;

	public partial class GADrivenContent : BaseSublayout {

		private void Page_Load(object sender, EventArgs e) {
			List<string> tags = TagUtil.GetTags(DataSourceItem);
			if(!tags.Any())
				return;
			string tag = tags.First();
			Sitecore.Data.ID i = null;
			string tagName = (Sitecore.Data.ID.TryParse(tag, out i))
				? Sitecore.Context.Database.GetItem(i).DisplayName
				: string.Empty;
			lnkBtn.Attributes.Add("style", string.Format("background-color:{0};", tagName));
			lnkBtn.Text = "&nbsp;";
			lnkBtn.NavigateUrl = string.Format("/{0}.aspx", tagName);
			lnkBtn.Attributes.Add("tag",tag);
			lnkBtn.Attributes.Add("value", "1");
		}
	}
}