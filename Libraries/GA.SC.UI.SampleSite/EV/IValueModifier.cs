using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA.SC.UI.SampleSite.EV {
	public interface IValueModifier {
		double MinRatio { get; }
		int DecimalPlaces { get; }
		TimespanAspect HalfLifeType { get; }
		int HalfLife { get; }
		double CurrentValue(IEngagementValue ev);
	}
}
