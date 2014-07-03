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

		/// <summary>
		/// builds 'size' number of chromosomes and sorts
		/// </summary>
		public virtual void InitializePopulation() {
			Karyotypes = new List<IKaryotype>();
			for (int count = 0; count < Options.PopSize; count++) {
				//add in the random population of chromosomes for each type in the genotype
				IKaryotype k = (IKaryotype)Activator.CreateInstance(Options.KaryotypeType, Options, mom, dad);
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
			
			for (int i = elitePos; i < Karyotypes.Count; i++) { // loop through and replace or mutate elites
				//possibly mate or mutate but not both
				if (IfCrossover()) { 
					List<IKaryotype> parents = SelectParents();
					evolvedSet[i] = parents.First().Mate(parents.Last()); //replace an elite

					if (IfMutate()) 
						evolvedSet[i].Mutate();
				} else if (IfMutate()) { 
					evolvedSet[i].Mutate();
				}
			}
			Karyotypes = evolvedSet.OrderByDescending(a => a.Fitness).ToList();
		}
		
		/// <summary>
		/// select a chromosome fit enough to use
		/// </summary>
		/// <returns></returns>
		public virtual IKaryotype ChooseFitKaryotype() {
			double topFit = Karyotypes.First().Fitness;
			List<IKaryotype> u = GetUniqueKaryotypes();
			List<IKaryotype> lk = (topFit < 1) // if all values have decayed below 1 then don't filter any options out
				? u
				: u.Where(a => a.Fitness >= (topFit * Options.FitnessRatio)).ToList();
			int newPos = RandomUtil.Instance.Next(0, lk.Count);
			IKaryotype k = (lk.Any()) // if the filter worked too well just select the first item
				? lk[newPos]
				: Karyotypes.First();

			return k;
		}

		#endregion IPopulation

		#region Methods

		public List<IKaryotype> GetUniqueKaryotypes() {
			Dictionary<string, IKaryotype> uniqueSet = new Dictionary<string, IKaryotype>();
			foreach (IKaryotype k in Karyotypes) {
				string uKey = k.ExpressedHaploid.DNASequence();
				if (!uniqueSet.ContainsKey(uKey))
					uniqueSet.Add(uKey, k);
			}
			return uniqueSet.Values.ToList();
		}

		/// <summary>
		/// Selects two chromosomes randomly and tries to improve odds by comparing it's fitness to other chromosomes also randomly selected
		/// </summary>
		protected virtual List<IKaryotype> SelectParents() {
			List<IKaryotype> parents = new List<IKaryotype>(2);

			//finds two randomly selected parents
			for (int parentIndex = 0; parentIndex < 2; parentIndex++) {
				parents.Add(Karyotypes[RandomUtil.Instance.Next(Karyotypes.Count - 1)]);

				//it tries TourneySize times to randomly find a better parent
				for (int tournyIndex = 0; tournyIndex < Options.TourneySize; tournyIndex++) {
					int randomIndex = RandomUtil.Instance.Next(Karyotypes.Count - 1);
					if (Karyotypes[randomIndex].Fitness > parents[parentIndex].Fitness) // higher is more fit
						parents[parentIndex] = Karyotypes[randomIndex];
				}
			}

			return parents;
		}

		/// <summary>
		/// if a random double is less than the crossover value (high probability) then mate
		/// </summary>
		/// <returns></returns>
		protected bool IfCrossover() { return RandomUtil.Instance.NextDouble() <= Options.CrossoverRatio; }
		
		/// <summary>
		/// if a random double is less than the mutation rate (low probability) then mutate
		/// </summary>
		/// <returns></returns>
		protected bool IfMutate() { return RandomUtil.Instance.NextDouble() <= Options.MutationRatio; }
				
		#endregion IPopulation
	}
}
