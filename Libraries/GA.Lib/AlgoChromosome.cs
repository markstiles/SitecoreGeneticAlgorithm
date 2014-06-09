using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace GA.Lib {
	[DebuggerDisplay("Gene={Gene}")]
	public class AlgoChromosome : Dictionary<int, AlgoGene>, IComparable<AlgoChromosome> {

		#region Properties

		private static Random rand = new Random(Environment.TickCount);
		
		public int Fitness {
			get {
				int fitness = 0;
				foreach (KeyValuePair<string, AlgoGene> g in this) 
					fitness += g.Value.Clicks; // find the combination of most clicked on renderings

				return fitness;
			}
		}

		#endregion Properties

		#region ctor

		public AlgoChromosome() { }

		#endregion ctor
		
		#region IComparable methods

		public int CompareTo(AlgoChromosome other) {
			return this.Fitness.CompareTo(other.Fitness);
		}

		#endregion IComparable methods

		#region Methods

		/// <summary>
		/// builds new chromosome with random gene sequence
		/// </summary>
		public static AlgoChromosome GenerateRandom(List<string> tags, int geneCount) {
			AlgoChromosome ac = new AlgoChromosome();
			for (int count = 0; count < geneCount; count++) {
				AlgoGene ag = new AlgoGene();
				ag.Clicks = 0;
				ag.Tag = tags[rand.Next(0, tags.Count)];
				ac.Add(count, ag); // not sure why adds 32 after instead of before
			}

			return ac;
		}

		/// <summary>
		/// changes a random character in the gene to a random character
		/// </summary>
		public void Mutate(List<string> tags) {
			//randomly get a gene and set it to a random tag
			AlgoGene ag = new AlgoGene();
			ag.Tag = tags[rand.Next(0, tags.Count)];
			ag.Clicks = 0;
			this[rand.Next(0, this.Count)] = ag;
		}

		/// <summary>
		/// take the genes from this and a mate and split them in half and swap them
		/// </summary>
		public List<AlgoChromosome> Mate(AlgoChromosome mate) {
			int pivotIndex = rand.Next(0, this.Count - 1);

			AlgoChromosome ac1 = new AlgoChromosome();
			AlgoChromosome ac2 = new AlgoChromosome();

			for(int j = 0; j < pivotIndex; j++){
				ac1[j] = this[j];
				ac2[j] = mate[j];
			}
			// cut the genes in half and mix
			for (int i = pivotIndex; i < this.Count; i++) {
				ac1[i] = mate[i];
				ac2[i] = this[i];
			}

			return new List<AlgoChromosome>(2) { ac1, ac2 };
		}

		#endregion Methods
	}
}
