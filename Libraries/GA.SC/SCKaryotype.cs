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
	public class SCKaryotype : BaseKaryotype {
		
		#region ctor

		public SCKaryotype(IPopulationManager ipo, IHaploid mom, IHaploid dad) : base(ipo, mom, dad) { }

		#endregion ctor

		#region IKaryotype

		public override double Fitness {
			get {
				double fitness = 0;
				foreach (KeyValuePair<string, IChromosome> kvp in ExpressedHaploid){
					foreach (IGene g in kvp.Value) {
						if (EngagementValue.KnownValues.Any() && EngagementValue.KnownValues.ContainsKey(g.GeneID)) { //need to change how this gets stored.
							List<EngagementValue> evl = EngagementValue.KnownValues[g.GeneID];
							fitness += evl.Sum(a => a.CurrentValue());
						}
					}
				}
				return Math.Round(fitness, 3);
			}
		}

		#endregion IKaryotype
	}
}
