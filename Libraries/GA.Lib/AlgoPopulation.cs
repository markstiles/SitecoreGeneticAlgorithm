using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA.Lib {
	public class AlgoPopulation {

		#region Properties

		private const int TOURNAMENT_SIZE = 3;
		private static Random _rand = new Random(Environment.TickCount);
		public float Eliteism { get; set; }
		public float Mutation { get; set; }
		public float Crossover { get; set; }
		public List<string> Tags { get; set; }
		public int GeneCount { get; set; }
		public Dictionary<string, AlgoChromosome> Chromosomes { get; set; }

		#endregion Properties

		#region ctor

		public AlgoPopulation(int size, float crossoverRatio, float eliteismRatio, float mutationRatio, List<string> tags, int geneCount) {
			this.Crossover = crossoverRatio;
			this.Eliteism = eliteismRatio;
			this.Mutation = mutationRatio;
			this.Tags = tags;
			this.GeneCount = geneCount;

			InitializePopulation(size);
		}

		#endregion ctor

		#region Methods

		/// <summary>
		/// builds 'size' number of chromosomes and sorts
		/// </summary>
		private void InitializePopulation(int size) {
			this.Chromosomes = new Dictionary<string, AlgoChromosome>(size);
			for (int count = 0; count < size; count++) {
				this.Chromosomes.Add(string.Format("Gene {0}", count.ToString()), AlgoChromosome.GenerateRandom(Tags, GeneCount));
			}

			this.Chromosomes.OrderBy(a => a.Value.Fitness);
		}

		/// <summary>
		/// updates the elite portion by mating the most fit and mutate randomly
		/// </summary>
		public void Evolve() {
			Dictionary<string, AlgoChromosome> evolvedSet = new Dictionary<string, AlgoChromosome>(this.Chromosomes);

			//get a position in the number of chromosomes based on the percent of elitism
			int unchangedIndex = (int)Math.Round(this.Chromosomes.Count * this.Eliteism);

			//start looping through those elites
			for (int changedIndex = unchangedIndex; changedIndex < this.Chromosomes.Count - 1; changedIndex++) {
				//roll the dice. 
				if (_rand.NextDouble() <= this.Crossover) { // if a random double is less than the crossover value (high probability) then mate
					List<AlgoChromosome> parents = this.SelectParents();
					List<AlgoChromosome> children = parents.First().Mate(parents.Last());

					evolvedSet[changedIndex] = children.First(); //replace an elite

					if (_rand.NextDouble() <= this.Mutation) // if random is less than mutation rate (low probability) then mutate
						evolvedSet[changedIndex] = evolvedSet[changedIndex].Mutate();

					if (changedIndex < evolvedSet.Count - 1) { // if not the last
						changedIndex++;

						evolvedSet[changedIndex] = children.Last(); // set the next too 
						if (_rand.NextDouble() <= this.Mutation) // possibly mutate it since the next round may not mate or mutate
							evolvedSet[changedIndex] = evolvedSet[changedIndex].Mutate();
					}
				} else if (_rand.NextDouble() <= this.Mutation) { // or if the random double is less than the mutation rate (low probability) then mutate
					evolvedSet[changedIndex].Mutate();
				}
				changedIndex++;
			}
			evolvedSet.Sort();
			this.Chromosomes = evolvedSet;
		}

		/// <summary>
		/// Selects two chromosomes randomly and tries to improve odds by comparing it's fitness to other chromosomes also randomly selected
		/// </summary>
		private List<AlgoChromosome> SelectParents() {
			List<AlgoChromosome> parents = new List<AlgoChromosome>(2);

			//finds two randomly selected parents
			for (int parentIndex = 0; parentIndex < 2; parentIndex++) {
				parents.Add(this.Chromosomes[_rand.Next(this.Chromosomes.Count - 1)]);

				//it tries TOURNAMENT_SIZE times to randomly find a better parent
				for (int tournyIndex = 0; tournyIndex < TOURNAMENT_SIZE; tournyIndex++) {
					int randomIndex = _rand.Next(this.Chromosomes.Count - 1);
					if (this.Chromosomes[randomIndex].Fitness < parents[parentIndex].Fitness) { // closer to 0 is more fit
						parents[parentIndex] = this.Chromosomes[randomIndex];
					}
				}
			}

			return parents;
		}

		#endregion Methods
	}
}
