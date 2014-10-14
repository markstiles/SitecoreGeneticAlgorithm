using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA.SC.EV {
	public interface IEngagementValueProvider {

		/// <summary>
		/// All Engagement Values
		/// </summary>
		Dictionary<string, List<IEngagementValue>> Values { get; }

		/// <summary>
		/// Subset of All Values limited by time, count etc.
		/// </summary>
		Dictionary<string, List<IEngagementValue>> RelevantValues { get; }
	}
}
