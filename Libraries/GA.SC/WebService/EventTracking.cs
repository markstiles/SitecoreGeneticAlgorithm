using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services;
using GA.SC.EV;
using Sitecore.Sites;

namespace GA.SC.WebService {
	[WebService(Namespace = "http://www.ga-clicktrack.com/services")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[System.ComponentModel.ToolboxItem(false)]
	[System.Web.Script.Services.ScriptService]
	public class EventTracking : System.Web.Services.WebService {
		[WebMethod]
		public void TrackEvent(string TagClick, string Site) {
			SiteContext sc = Sitecore.Configuration.Factory.GetSite(Site);
			if(sc == null)
				throw new NullReferenceException(string.Format("The Site '{0}' provided doesn't exist", Site));
			ConfigUtil cu = new ConfigUtil(sc.Properties[ConfigUtil.SiteProperty]);
			if (!cu.EVProvider.Values.ContainsKey(TagClick))
				cu.EVProvider.Values.Add(TagClick, new List<IEngagementValue>());
			cu.EVProvider.Values[TagClick].Add(new EngagementValue(1));
		}
	}
}
