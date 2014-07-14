namespace GA.SC.UI.SampleSite.Layouts {
	using System;
	using System.Linq;
	using System.Collections.Generic;
	using System.Web;
	using System.Web.UI;
	using Sitecore.Data.Items;
	using Sitecore.Links;

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
		}
	}
}
