using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA.SC.UI.SampleSite.EV {
	public class DefaultEngagementValueProvider : IValueProvider {

		private static Dictionary<string, List<IValue>> _Values = new Dictionary<string, List<IValue>>();
		public Dictionary<string, List<IValue>> Values {
			get {
				return _Values;
			}
		}

		private string DateTimeFormat = "yyyyMMddhhmmssfft";

		public Dictionary<string, List<IValue>> RelevantValues {
			get {
				//get the most recent clicks
				int clickCountLimit = 20;
				Dictionary<string, IValue> vals = Values.SelectMany(a => a.Value).OrderByDescending(a => a.LastUpdated).Take(clickCountLimit).ToDictionary(a => a.LastUpdated.ToString(DateTimeFormat));

                Dictionary<string, List<IValue>> rv = new Dictionary<string, List<IValue>>();
                foreach (KeyValuePair<string, List<IValue>> kvp in Values) {
                    IEnumerable<IValue> result = kvp.Value.Where(a => vals.ContainsKey(a.LastUpdated.ToString(DateTimeFormat)));
					if(result.Any())
						rv.Add(kvp.Key, result.ToList());
				}

				return rv;
			}
		}
	}
}
