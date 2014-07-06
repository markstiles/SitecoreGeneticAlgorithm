namespace GA.SC.UI.Layouts {
	using System;
	using System.Linq;
	using System.Collections.Generic;
	using System.Web;
	using System.Web.UI;
	using Sitecore.Data.Items;
	using Sitecore.Links;

	public partial class AlgoMaster : Page {
		private void Page_Load(object sender, System.EventArgs e) {
			// TODO copy the UI files to the modules folder instead of the root and update the sublayout/layout items
			List<KeyValuePair<string, string>> navItems = new List<KeyValuePair<string,string>>();
			
			Item h = Sitecore.Context.Database.GetItem(Sitecore.Context.Site.StartPath);
			Item ap = h.Children.Where(a => a.Name == "AlgoPage").First();

			lnkLogo.NavigateUrl = LinkManager.GetItemUrl(ap);
			foreach (Item i in ap.Children) 
				navItems.Add(new KeyValuePair<string, string>(LinkManager.GetItemUrl(i), i.DisplayName));

			rptNav.DataSource = navItems;
			rptNav.DataBind();
		}
	}
}
