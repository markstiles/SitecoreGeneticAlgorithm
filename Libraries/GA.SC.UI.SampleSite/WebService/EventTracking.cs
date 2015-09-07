using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services;
using GA.SC.UI.SampleSite.EV;
using Sitecore.Sites;

namespace GA.SC.UI.SampleSite.WebService {
	[WebService(Namespace = "http://www.ga-clicktrack.com/services")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[System.ComponentModel.ToolboxItem(false)]
	[System.Web.Script.Services.ScriptService]
	public class EventTracking : System.Web.Services.WebService {
		[WebMethod]
		public void TrackEvent(string TagClick, string Site, string Value) {
			SiteContext sc = Sitecore.Configuration.Factory.GetSite(Site);
			if(sc == null)
				throw new NullReferenceException(string.Format("The Site '{0}' provided doesn't exist", Site));
			double value = 1;
			if(!double.TryParse(Value, out value)) 
				value = 1;
            IValueProvider evp = new DefaultEngagementValueProvider();
            if (!evp.Values.ContainsKey(TagClick))
				evp.Values.Add(TagClick, new List<IValue>());
			evp.Values[TagClick].Add(new EngagementValue(value));
		}
	}
}
