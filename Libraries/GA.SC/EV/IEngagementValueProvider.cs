using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA.SC.EV {
	public interface IEngagementValueProvider {
		Dictionary<string, List<IEngagementValue>> Values { get; }
	}
}
