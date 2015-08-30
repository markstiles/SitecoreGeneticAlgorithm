using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA.SC.UI.SampleSite.EV {
	public interface IValue {
		
		double Value { get; set; }
		DateTime LastUpdated { get; }
	}
}
