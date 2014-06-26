using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA.Lib {
	public class EngagementValue {
		
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

		public EngagementValue(double val) {
			Value = val;
		}

		/// <summary>
		/// returns the Value decayed over time
		/// </summary>
		/// <returns></returns>
		public double CurrentValue() {
			
			double startVal = this.Value;
			double timespan = Math.Abs(((TimeSpan)(DateTime.Now - this.LastUpdated)).TotalSeconds); //seconds since last event
			//put in an end of life. If timespan is greater than a year then it's value is zero
			double halfLife = 2; //half life in seconds
			double halfLifeRatio = Math.Abs(timespan / halfLife); // should be non-zero. a lower value is a faster drop off
			double minRatio = 0.1; // make the min value a percentage of start value
			double minVal = minRatio * startVal; // makes sure min values are not the same for all
			double adjStartVal = startVal - minVal; // makes the curve start at the Value not offset
			
			//$\left(0.5\right)^{\left(\frac{x}{h}\right)}$
			//DEV = EV (0.5)^T/Th
			//double newVal = Math.Pow((startVal * 0.5), (halfLifeRatio));
			
			//$ve^{-\abs \left(\frac{x}{h}\right)}+m$
			// (startVal * e^-abs(timespan/halflLife)) + minVal
			double newVal = Math.Pow((adjStartVal * Math.E), -(halfLifeRatio)) + minVal;
			
			return Math.Round(newVal, 3);
			
			//ACTION: Value
			//REASON: separate value for tags clicked through nav vs. rendering displayed (give higher start val) 

			//ACTION: e^x
			//REASON: short term vs. long term

			//ACTION: half-life ratio 
			//i think the current half life should be based on the time from your most active engagement 
			//probably a mean or median of clicks. This may allow you to filter out noise.  

			//ACTION: MinValue
			//REASON: if you're away a long time the past is still relevant but not as much as recent events.

			//ACTION: EndOfLife
			//REASON: May want to forget the past but store a relic of past. like a fossil record of what was the best.

				//ACTION: Energy vs. Mass
				//REASON: Mass decreases over time but the energy is conserved. 
				//A sufficient energy value should be preserved to affect overall weighting
				//this can only be done at a higher level counting all EV's for a Tag

			//EXAMPLES
			//if 1 is clicked 25 times ten minutes ago its still relevant and 4 you just clicked.
			//they click men's category from the nav but never click on the ads for mens
			//but when they click on shoes they click on shoe ads. The shoes category are more valuable than men's category
			//steady clicks to a tag vs. bursts to another. 
				//recent burst
				//previous burst
				//both recent and previous burst
		}
	}
}
