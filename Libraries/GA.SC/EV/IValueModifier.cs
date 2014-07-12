using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA.SC.EV {
	public interface IValueModifier {
		double CurrentValue(IEngagementValue ev);
	}
}
