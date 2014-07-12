using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services;

namespace GA.SC.WebService {
	[WebService(Namespace = "http://www.diagnoseplatform.nl/services")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[System.ComponentModel.ToolboxItem(false)]
	[System.Web.Script.Services.ScriptService]
	public class EventTracking : System.Web.Services.WebService {
		[WebMethod]
		public void TrackEvent(string TagClick) {
			if (!EngagementValue.KnownValues.ContainsKey(TagClick))
				EngagementValue.KnownValues.Add(TagClick, new List<EngagementValue>());
			EngagementValue.KnownValues[TagClick].Add(new EngagementValue(1));
		}
	}
}
