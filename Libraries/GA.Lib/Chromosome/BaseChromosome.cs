using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GA.Lib.Gene;
using GA.Lib.Population;

namespace GA.Lib.Chromosome {
	public abstract class BaseChromosome : List<IGene>, IChromosome {
	
		#region ctor

		public BaseChromosome(IPopulationOptions ipo){
			Options = ipo;
		}

		#endregion ctor

		#region IComparable

		public int CompareTo(IChromosome other) {
			return this.Fitness.CompareTo(other.Fitness);
		}

		#endregion IComparable

		#region IChromosome

		public IPopulationOptions Options { get; set; }

		public abstract double Fitness { get; }

		public virtual string GeneSequence() {
			StringBuilder sb = new StringBuilder();
			foreach (IGene g in this)
				sb.Append(g.GeneID);
			return sb.ToString();
		}

		public virtual void FillRandomly(string chromosomeName) {
			List<IGene> genepool = Options.Genotype[chromosomeName];
			for (int j = 0; j < Options.GeneCount; j++) {
				IGene g = genepool[RandomUtil.Instance.Next(0, genepool.Count)];
				this.Add(g);
			}
		}

		#endregion IChromosome

		#region Methods
		
		public void AddRange(IEnumerable<KeyValuePair<string, IGene>> g) {
			foreach (KeyValuePair<string, IGene> kvp in g)
				this.Add(kvp.Value);
		}

		#endregion Methods
	}
}
