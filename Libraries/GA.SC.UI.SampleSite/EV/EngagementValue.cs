using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA.SC.UI.SampleSite.EV {
	
	public class EngagementValue : IEngagementValue {

		#region Properties 
		
		private double _Value;
		public double Value {
			get {
				return _Value;
			}
			set {
				LastUpdated = DateTime.Now;
				_Value = value;
			}
		}

		public DateTime LastUpdated { get; internal set; }

		#endregion Properties

		#region ctor

		public EngagementValue(double val) {
			Value = val;
		}

		#endregion ctor
	}
}
