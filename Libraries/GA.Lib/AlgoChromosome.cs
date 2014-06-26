using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Web.UI.WebControls;

namespace GA.Lib {
	public class AlgoChromosome : Dictionary<string, IGene>, IComparable<AlgoChromosome> {

		#region Properties

		private static Random rand = new Random(Environment.TickCount);

		public static Dictionary<string, List<EngagementValue>> EngagementValues = new Dictionary<string, List<EngagementValue>>();

		public double Fitness {
			get {
				double fitness = 0;
				foreach (KeyValuePair<string, IGene> g in this) {
					if (EngagementValues.ContainsKey(((AlgoGene)g.Value).Tag)) { //need to change how this gets stored.
						List<EngagementValue> evl = EngagementValues[((AlgoGene)g.Value).Tag];
						fitness += evl.Sum(a => a.CurrentValue());
					}
				}
				return Math.Round(fitness, 3);
			}
		}

		public string GeneSequence {
			get {
				StringBuilder sb = new StringBuilder();
				foreach (KeyValuePair<string, IGene> g in this) {
					//if (sb.Length > 0)
					//	sb.Append("-");
					sb.Append(((AlgoGene)g.Value).GeneID);
				}
				return sb.ToString();
			}
		}

		#endregion Properties

		#region ctor

		public AlgoChromosome() { }

		public void AddRange(IEnumerable<KeyValuePair<string, IGene>> g) {
			foreach (KeyValuePair<string, IGene> kvp in g)
				this.Add(kvp.Key, kvp.Value);
		}

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
		public static AlgoChromosome GenerateRandom(List<IGene> Genotype, int geneCount) {
			AlgoChromosome ac = new AlgoChromosome();
			var a = from IGene g in Genotype
					select new KeyValuePair<string, IGene>(g.GeneID, g);
			
			ac.AddRange(a);
			for (int c = 0; c < geneCount; c++) {
				IGene g = Genotype[rand.Next(0, Genotype.Count)];
				AlgoGene ag = g.GetRandom(); new AlgoGene(placeholders[count].ID, tags[rand.Next(0, tags.Count)]);
				ac.Add(ag.PlaceholderID, ag); 
			}

			return ac;
		}

		/// <summary>
		/// changes a random character in the gene to a random character
		/// </summary>
		public void Mutate(List<Literal> placeholders, List<string> tags) {
			//randomly get a gene and set it to a random placeholder and tag
			string ph = placeholders[rand.Next(0, placeholders.Count)].ID;
			AlgoGene ag = new AlgoGene(ph, tags[rand.Next(0, tags.Count)]);
			this[ph] = ag;
		}

		/// <summary>
		/// take the genes from this and a mate and split them in half and swap them
		/// </summary>
		public List<AlgoChromosome> Mate(AlgoChromosome mate) {
			int pivotIndex = rand.Next(0, this.Count - 1);

			AlgoChromosome ac1 = new AlgoChromosome();
			AlgoChromosome ac2 = new AlgoChromosome();

			int i = 0;
			// cut the genes in half and mix
			foreach(KeyValuePair<string, IGene> g in this){
				if(i < pivotIndex) {
					ac1.Add(g.Key, g.Value);
					ac2.Add(g.Key, mate[g.Key]);
				} else {
					ac1.Add(g.Key, mate[g.Key]);
					ac2.Add(g.Key, g.Value);
				}
				i++;
			}
			
			return new List<AlgoChromosome>(2) { ac1, ac2 };
		}

		#endregion Methods
	}
}
