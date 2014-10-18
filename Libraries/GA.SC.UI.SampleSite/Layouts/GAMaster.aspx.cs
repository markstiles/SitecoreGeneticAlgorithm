namespace GA.SC.UI.SampleSite.Layouts {
	using System.Collections.Generic;
	using System.Linq;
	using System.Web.UI;
	using Sitecore.Data.Items;
	using Sitecore.Links;

	public partial class GAMaster : Page {
		private void Page_Load(object sender, System.EventArgs e) {

			List<KeyValuePair<string, string>> navItems = new List<KeyValuePair<string,string>>();
			
			Item h = Sitecore.Context.Database.GetItem(Sitecore.Context.Site.StartPath);
			ltlContextSite.Text = string.Format("'{0}'", Sitecore.Context.Site.Name);
			lnkLogo.NavigateUrl = LinkManager.GetItemUrl(h);
			
			rptNav.DataSource = h.Children;
			rptNav.DataBind();
		}

		protected string GetTag(Item i) {
			List<string> tags = TagUtil.GetTags(i);
			return (tags.Any()) ? tags.First() : string.Empty;
		}
	}
}
