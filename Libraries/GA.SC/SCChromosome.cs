using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Web.UI.WebControls;
using GA.Lib.Chromosome;
using GA.Lib.Gene;
using GA.Lib.Population;

namespace GA.SC {
	public class SCChromosome : BaseChromosome {

		#region Properties

		public static Dictionary<string, List<EngagementValue>> EngagementValues = new Dictionary<string, List<EngagementValue>>();

		#endregion Properties

		#region ctor

		public SCChromosome(IPopulationOptions ipo) : base(ipo) { }

		#endregion ctor

		#region IChromosome

		public override double Fitness {
			get {
				double fitness = 0;
				foreach (IGene g in this) {
					if (EngagementValues.Any() && EngagementValues.ContainsKey(g.GeneID)) { //need to change how this gets stored.
						List<EngagementValue> evl = EngagementValues[g.GeneID];
						fitness += evl.Sum(a => a.CurrentValue());
					}
				}
				return Math.Round(fitness, 3);
			}
		}

		#endregion IChromosome
	}
}
