using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Web.UI.WebControls;
using GA.Nucleus.Chromosome;
using GA.Nucleus.Gene;
using GA.Nucleus.Population;
using GA.SC.UI.SampleSite.EV;

namespace GA.SC.UI.SampleSite.Population {
	public class PageKaryotype : BaseKaryotype {
		
		#region ctor

		public PageKaryotype(IHaploid mom, IHaploid dad) : base(mom, dad) { }

		#endregion ctor

		#region IKaryotype

		/// <summary>
		/// This now sums the values for all tags in a genome 
		/// then scales it by how much the tags in the genome differs from the tags clicked
		/// 
		/// Also need to know that if the number of tags with value outnumber the placeholders it shouldn't penalize that.
		/// 
		/// could modify the values individually as they're being summed by the weight carried by that tag in the whole 
		/// instead of weight across the whole sum by the difference in percent of both tag value and tag in genome
		/// </summary>
		public override double Fitness() {
			//filter the tags in the genome into a dictionary(hashtable) 
			//store the amount of them as the value
			//track the total so you can determine what percentage of the whole any given tag is. 
			Dictionary<string, double> tagsInGenome = new Dictionary<string, double>();
			double geneSum = 0;
			foreach (KeyValuePair<string, IChromosome> kvp in Phenotype){
				foreach (IGene g in kvp.Value) {
					if (!tagsInGenome.ContainsKey(g.GeneID)) {
						tagsInGenome.Add(g.GeneID, 1);
					} else {
						tagsInGenome[g.GeneID]++;
					}
					geneSum += 1;
				}
			}
             
            IValueProvider EVProvider = new DefaultEngagementValueProvider();

			//filter all tags used to store events, into a dictionary(hashtable) 
			//store the total value for each tag as the value
			//track the total value over all tags so you determine what percentage of the whole any given tag is.
			Dictionary<string, double> tagValues = new Dictionary<string, double>();
			double tagSum = 0;
			foreach (KeyValuePair<string, List<IValue>> kvp in EVProvider.RelevantValues) {
				double partSum = kvp.Value.Sum(a => a.Value);
				tagValues.Add(kvp.Key, partSum);
				tagSum += partSum;
			}

			//for all tags with an engagement value 
			//find the difference between the percentage the tag's value makes from the total value
			//and the percentage the tag is used in the genome.
			double deltaSum = 0;
			foreach (KeyValuePair<string, double> kvp in tagValues) {
				double tagPercent = kvp.Value/tagSum;
				double genePercent = (tagsInGenome.ContainsKey(kvp.Key))
					? tagsInGenome[kvp.Key] / geneSum
					: 0;
					
				deltaSum += Math.Abs(tagPercent - genePercent);
			}

            IValueModifier ValueModifier = new DefaultValueModifier();
                
			//default to zero if there were no tags and the value was NaN
			//round the value to the precision set in the config
			return Math.Round((double.IsNaN(deltaSum)) ? 0 : deltaSum, ValueModifier.DecimalPlaces);
		}

		#endregion IKaryotype
	}
}
