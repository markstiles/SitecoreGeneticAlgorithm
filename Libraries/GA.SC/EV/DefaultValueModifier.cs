﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA.SC.EV {

	public enum TimespanAspect { Days, Hours, Minutes, Seconds, Milliseconds }

	public class DefaultValueModifier : IValueModifier {

		#region Fields

		/// <summary>
		/// make the min value a percentage of start value 
		/// could be the inverse of the number of clicks to make a tag remain at a full engagement value
		/// </summary>
		private double MinRatio = 0.1; 
		private int DecimalPlaces = 3;
		/// <summary>
		/// what time frame should the half life be based on
		/// </summary>
		private TimespanAspect HalfLifeType = TimespanAspect.Seconds;
		/// <summary>
		/// half life in terms of the HalfLifeType. IE. HalfLifeType = TimespanAspect.Seconds and HalfLife = 2 means a 2 second half life
		/// </summary>
		private int HalfLife = 2;

		#endregion Fields

		#region ctor

		public DefaultValueModifier(double minRatio, int decimalPlaces, TimespanAspect ta, int halfLife) {
			MinRatio = minRatio;
			DecimalPlaces = decimalPlaces;
			HalfLifeType = ta;
			HalfLife = halfLife;
		}

		#endregion ctor

		/// <summary>
		/// returns the Value decayed over time
		/// </summary>
		/// <returns></returns>
		public double CurrentValue(IEngagementValue ev) {

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