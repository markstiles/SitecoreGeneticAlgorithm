using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GA.Lib.Gene;
using GA.Lib.Population;

namespace GA.Lib.Chromosome {
	public abstract class AbstractChromosome : List<IGene>, IChromosome {

		public IPopulationOptions Options;

		#region ctor

		public AbstractChromosome(IPopulationOptions ipo){
			Options = ipo;
			for (int j = 0; j < Options.GeneCount; j++) {
				IGene g = Options.Genotype.GetRandom();
				this.Add(g);
			}
		}

		#endregion ctor

		#region IComparable

		public int CompareTo(IChromosome other) {
			return this.Fitness.CompareTo(other.Fitness);
		}

		#endregion IComparable

		#region IChromosome

		public abstract double Fitness { get; }

		public virtual string GeneSequence() {
			StringBuilder sb = new StringBuilder();
			foreach (IGene g in this)
				sb.Append(g.GeneID);
			return sb.ToString();
		}

		/// <summary>
		/// changes a random character in the gene to a random character
		/// </summary>
		public virtual void Mutate(GenotypeList gt) {
			//randomly get a gene and set it to a random location
			IGene g = gt[RandomUtil.Instance.Next(0, gt.Count)];
			int mutPos = RandomUtil.Instance.Next(0, this.Count);
			this[mutPos] = g;
		}

		/// <summary>
		/// take the genes from this and a mate and split them in half and swap them
		/// </summary>
		public virtual List<IChromosome> Mate(IChromosome mate) {
			int pivotIndex = RandomUtil.Instance.Next(1, this.Count - 1);

			Object[] args = { Options };
			Type t = this.GetType();
			IChromosome ac1 = (IChromosome)Activator.CreateInstance(t, Options);
			IChromosome ac2 = (IChromosome)Activator.CreateInstance(t, Options);

			// cut the genes in half and mix
			for (int i = 0; i < this.Count; i++) {
				ac1[i] = (i < pivotIndex) ? this[i] : mate[i];
				ac2[i] = (i < pivotIndex) ? mate[i] : this[i];
			}

			return new List<IChromosome>(2) { ac1, ac2 };
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
