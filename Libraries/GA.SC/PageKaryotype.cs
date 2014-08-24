using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Web.UI.WebControls;
using GA.Nucleus.Chromosome;
using GA.Nucleus.Gene;
using GA.Nucleus.Population;
using GA.SC.EV;

namespace GA.SC {
	public class PageKaryotype : BaseKaryotype {
		
		#region ctor

		public PageKaryotype(IPopulationManager ipo, IHaploid mom, IHaploid dad) : base(ipo, mom, dad) { }

		#endregion ctor

		#region IKaryotype

		public override double Fitness {
			get {
				double fitness = 0;
				foreach (KeyValuePair<string, IChromosome> kvp in Phenotype){
					foreach (IGene g in kvp.Value) {
						if (ConfigUtil.Context.EVProvider.Values.Any() && ConfigUtil.Context.EVProvider.Values.ContainsKey(g.GeneID)) { //need to change how this gets stored.
							List<IEngagementValue> evl = ConfigUtil.Context.EVProvider.Values[g.GeneID];
							//fitness += evl.Sum(a => ConfigUtil.Context.ValueModifier.CurrentValue(a));
							fitness += evl.Sum(a => a.Value);
						}
					}
				}
				return Math.Round(fitness, 3);
			}
		}

		#endregion IKaryotype
	}
}
