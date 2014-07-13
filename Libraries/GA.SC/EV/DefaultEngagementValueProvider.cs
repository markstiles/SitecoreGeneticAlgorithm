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
	}
}
