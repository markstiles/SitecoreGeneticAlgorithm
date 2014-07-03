using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GA.Lib.Chromosome;

namespace GA.Lib.Population {
	public abstract class BasePopulation : IPopulation {

		#region Properties

		public IPopulationOptions Options { get; set; }
		public List<IKaryotype> Karyotypes { get; set; }

		#endregion Properties

		#region ctor

		public BasePopulation(IPopulationOptions ipo) {
			Options = ipo;
			InitializePopulation();
		}

		#endregion ctor

		#region IPopulation

		public abstract IKaryotype CreateKaryotype(IPopulationOptions ipo);
			
		/// <summary>
		/// builds 'size' number of chromosomes and sorts
		/// </summary>
		public virtual void InitializePopulation() {
			Karyotypes = new List<IKaryotype>();
			for (int count = 0; count < Options.PopSize; count++) {
				IKaryotype k = CreateKaryotype(Options);
				Karyotypes.Add(k);
			}
		}

		/// <summary>
		/// updates the elite portion by mating the most fit and mutate randomly
		/// </summary>
		public virtual void Evolve() {
			List<IKaryotype> evolvedSet = new List<IKaryotype>(Karyotypes);

			//get a position in the number of karyotes based on crossover
			int elitePos = (int)Math.Round(Karyotypes.Count * Options.ElitismRatio);

			//start looping through those elites
			for (int i = elitePos; i < Karyotypes.Count - 1; i++) {
				//roll the dice. 
				if (RandomUtil.Instance.NextDouble() <= Options.CrossoverRatio) { // if a random double is less than the crossover value (high probability) then mate
					List<IKaryotype> parents = SelectParents();
					List<IKaryotype> children = parents.First().Mate(parents.Last());

					evolvedSet[i] = children.First(); //replace an elite

					if (RandomUtil.Instance.NextDouble() <= Options.MutationRatio) // if random is less than mutation rate (low probability) then mutate
						evolvedSet[i].Mutate();

					if (i < evolvedSet.Count - 1) { // if not the last item in set
						i++;

						evolvedSet[i] = children.Last(); // set the next too 
						if (RandomUtil.Instance.NextDouble() <= Options.MutationRatio) // possibly mutate it since the next round may not mate or mutate
							evolvedSet[i].Mutate();
					}
				} else if (RandomUtil.Instance.NextDouble() <= Options.MutationRatio) { // or if the random double is less than the mutation rate (low probability) then mutate
					evolvedSet[i].Mutate();
				}
				i++;
			}
			Karyotypes = evolvedSet.OrderByDescending(a => a.Fitness).ToList();
		}

		/// <summary>
		/// select a chromosome fit enough to use
		/// </summary>
		/// <returns></returns>
		public virtual IChromosome ChooseFitChromosome() {
			double topFit = Chromosomes.First().Fitness;
			List<IChromosome> u = GetUniqueChromosomes();
			List<IChromosome> lac = (topFit < 1) // if all values have decayed below 1 then don't filter any options out
				? u
				: u.Where(a => a.Fitness >= (topFit * Options.FitnessRatio)).ToList();
			int newPos = RandomUtil.Instance.Next(0, lac.Count);
			IChromosome c = (lac.Any()) // if the filter worked too well just select the first item
				? lac[newPos]
				: Chromosomes.First();

			return c;
		}

		#endregion IPopulation

		#region Methods

		public List<IChromosome> GetUniqueChromosomes() {
			Dictionary<string, IChromosome> uniqueSet = new Dictionary<string, IChromosome>();
			foreach (IChromosome c in Chromosomes) {
				string uKey = c.GeneSequence();
				if (!uniqueSet.ContainsKey(uKey))
					uniqueSet.Add(uKey, c);
			}
			return uniqueSet.Values.ToList();
		}

		/// <summary>
		/// Selects two chromosomes randomly and tries to improve odds by comparing it's fitness to other chromosomes also randomly selected
		/// </summary>
		protected virtual List<IKaryotype> SelectParents(string chromosomeName) {
			List<IKaryotype> parents = new List<IKaryotype>(2);

			//finds two randomly selected parents
			for (int parentIndex = 0; parentIndex < 2; parentIndex++) {
				parents.Add(Karyotypes[RandomUtil.Instance.Next(Karyotypes.Count - 1)]);

				//it tries TourneySize times to randomly find a better parent
				for (int tournyIndex = 0; tournyIndex < Options.TourneySize; tournyIndex++) {
					int randomIndex = RandomUtil.Instance.Next(Karyotypes.Count - 1);
					if (Karyotypes[randomIndex].ExpressedHaploid[chromosomeName].Fitness > parents[parentIndex].ExpressedHaploid[chromosomeName].Fitness) // closer to 0 is more fit
						parents[parentIndex] = Karyotypes[randomIndex];
				}
			}

			return parents;
		}

		#endregion IPopulation
	}
}
