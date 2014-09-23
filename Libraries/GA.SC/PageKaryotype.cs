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
				
				Dictionary<string, double> tagsInGenome = new Dictionary<string, double>();
				double geneSum = 0;
				double fitness = 0;
				foreach (KeyValuePair<string, IChromosome> kvp in Phenotype){
					foreach (IGene g in kvp.Value) {
						if (!tagsInGenome.ContainsKey(g.GeneID)) {
							tagsInGenome.Add(g.GeneID, 1);
						} else {
							tagsInGenome[g.GeneID]++;
						}
						geneSum += 1;

						if (ConfigUtil.Context.EVProvider.Values.Any() && ConfigUtil.Context.EVProvider.Values.ContainsKey(g.GeneID)) { 
							fitness += ConfigUtil.Context.EVProvider.Values[g.GeneID].Sum(a => a.Value);
						}
					}
				}

				Dictionary<string, double> tagValues = new Dictionary<string, double>();
				double tagSum = 0;
				foreach (KeyValuePair<string, List<IEngagementValue>> kvp in ConfigUtil.Context.EVProvider.Values) {
					double partSum = kvp.Value.Sum(a => a.Value);
					tagValues.Add(kvp.Key, partSum);
					tagSum += partSum;
				}

				double deltaSum = 0;
				foreach (KeyValuePair<string, double> kvp in tagValues) {
					double tagPercent = kvp.Value/tagSum;
					double genePercent = (tagsInGenome.ContainsKey(kvp.Key))
						? tagsInGenome[kvp.Key] / geneSum
						: 0;
					
					deltaSum += Math.Abs(tagPercent - genePercent);
				}

				double deltaMod = 1 - (deltaSum / tagValues.Count);
				double newFitness = deltaMod * fitness;

				//then multiply the fitness by the % of tags clicked it contains then again by the amount per tag
				//take the genes and run to a dictionary with a count
				//ConfigUtil.Context.EVProvider.Values

				return Math.Round(newFitness, ConfigUtil.Context.ValueModifier.DecimalPlaces);
			}
		}

		#endregion IKaryotype
	}
}
