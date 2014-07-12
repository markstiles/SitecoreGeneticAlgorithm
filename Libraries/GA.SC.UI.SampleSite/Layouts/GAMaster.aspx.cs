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
			// TODO copy the UI files to the modules folder instead of the root and update the sublayout/layout items
			List<KeyValuePair<string, string>> navItems = new List<KeyValuePair<string,string>>();
			
			Item h = Sitecore.Context.Database.GetItem(Sitecore.Context.Site.StartPath);
			
			lnkLogo.NavigateUrl = LinkManager.GetItemUrl(h);
			foreach (Item i in h.Children) 
				navItems.Add(new KeyValuePair<string, string>(LinkManager.GetItemUrl(i), i.DisplayName));

			rptNav.DataSource = navItems;
			rptNav.DataBind();
		}
	}
}
