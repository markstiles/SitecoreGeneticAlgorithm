using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA.SC.UI.SampleSite.EV {
	public interface IValueProvider {

		/// <summary>
		/// All Engagement Values
		/// </summary>
		Dictionary<string, List<IValue>> Values { get; }

		/// <summary>
		/// Subset of All Values limited by time, count etc.
		/// </summary>
		Dictionary<string, List<IValue>> RelevantValues { get; }
	}
}
