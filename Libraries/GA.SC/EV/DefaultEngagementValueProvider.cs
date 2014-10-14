using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA.SC.EV {
	public class DefaultEngagementValueProvider : IEngagementValueProvider {

		private static Dictionary<string, List<IEngagementValue>> _Values = new Dictionary<string, List<IEngagementValue>>();
		public Dictionary<string, List<IEngagementValue>> Values {
			get {
				return _Values;
			}
		}

		public Dictionary<string, List<IEngagementValue>> RelevantValues {
			get {
				//get the most recent clicks
				int clickCountLimit = 20;
				Dictionary<string, IEngagementValue> vals = ConfigUtil.Context.EVProvider.Values.SelectMany(a => a.Value).OrderByDescending(a => a.LastUpdated).Take(clickCountLimit).ToDictionary(a => a.LastUpdated.ToString());

				Dictionary<string, List<IEngagementValue>> rv = new Dictionary<string,List<IEngagementValue>>();
				foreach(KeyValuePair<string, List<IEngagementValue>> kvp in Values) {
					IEnumerable<IEngagementValue> result = kvp.Value.Where(a => vals.ContainsKey(a.LastUpdated.ToString()));
					if(result.Any())
						rv.Add(kvp.Key, result.ToList());
				}

				return rv;
			}
		}
	}
}
