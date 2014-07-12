using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA.SC.EV {
	public class DefaultEngagementValueProvider : IEngagementValueProvider {

		private Dictionary<string, List<IEngagementValue>> _Values;
		public Dictionary<string, List<IEngagementValue>> Values {
			get {
				if (_Values == null)
					_Values = new Dictionary<string, List<IEngagementValue>>();
				return _Values;
			}
		}
	}
}
