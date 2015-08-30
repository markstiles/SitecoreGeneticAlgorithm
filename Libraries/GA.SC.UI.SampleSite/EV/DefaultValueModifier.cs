using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA.SC.UI.SampleSite.EV {

	public enum TimespanAspect { Days, Hours, Minutes, Seconds, Milliseconds }

	public class DefaultValueModifier : IValueModifier {

		#region Fields

		/// <summary>
		/// make the min value a percentage of start value 
		/// could be the inverse of the number of clicks to make a tag remain at a full engagement value
		/// </summary>
		private double _MinRatio = 0.1;
		public double MinRatio {
			get {
				return _MinRatio;
			}
			set {
				_MinRatio = value;
			}
		}
		private int _DecimalPlaces = 3;
		public int DecimalPlaces {
			get {
				return _DecimalPlaces;
			}
			set {
				_DecimalPlaces = value;
			}
		}
		/// <summary>
		/// what time frame should the half life be based on
		/// </summary>
		private TimespanAspect _HalfLifeType = TimespanAspect.Seconds;
		public TimespanAspect HalfLifeType {
			get {
				return _HalfLifeType;
			}
			set {
				_HalfLifeType = value;
			}
		}
		/// <summary>
		/// half life in terms of the HalfLifeType. IE. HalfLifeType = TimespanAspect.Seconds and HalfLife = 2 means a 2 second half life
		/// </summary>
		private int _HalfLife = 2;
		public int HalfLife {
			get {
				return _HalfLife;
			}
			set {
				_HalfLife = value;
			}
		}

		#endregion Fields

		/// <summary>
		/// returns the Value decayed over time
		/// </summary>
		/// <returns></returns>
		public double CurrentValue(IValue ev) {

			//can select from days, hours, minutes, seconds and milliseconds

			//push up to user to set
			double timespan = 0.0f;
			TimeSpan ts = ((TimeSpan)(DateTime.Now - ev.LastUpdated));
			switch (HalfLifeType) {
				case TimespanAspect.Days:
					timespan = Math.Abs(ts.TotalDays);
					break;
				case TimespanAspect.Hours:
					timespan = Math.Abs(ts.TotalHours);
					break;
				case TimespanAspect.Minutes:
					timespan = Math.Abs(ts.TotalMinutes);
					break;
				case TimespanAspect.Seconds:
					timespan = Math.Abs(ts.TotalSeconds);
					break;
				case TimespanAspect.Milliseconds:
					timespan = Math.Abs(ts.TotalMilliseconds);
					break;
			} 
			
			 //seconds since last event
			double halfLifeRatio = timespan / HalfLife; // should greater than non-zero. a lower value is a faster drop off

			double startVal = ev.Value;
			double minVal = MinRatio * startVal; // makes sure min values are not the same for all
			double adjStartVal = startVal - minVal; // makes the curve start at the Value not offset

			//https://www.desmos.com/calculator : $ve^{-\abs \left(\frac{x}{h}\right)}+m$
			// (startVal * e^-abs(timespan/halflLife)) + minVal
			double newVal = Math.Pow((adjStartVal * Math.E), -Math.Abs(halfLifeRatio)) + minVal;

			return Math.Round(newVal, DecimalPlaces);
		}

	}
}
