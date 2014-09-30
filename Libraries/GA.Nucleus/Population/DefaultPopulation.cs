using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GA.Nucleus.Chromosome;
using GA.Nucleus.Gene;

namespace GA.Nucleus.Population {
	public class DefaultPopulation : IPopulation {

		#region Properties

		public IPopulationManager Manager { get; set; }
		public List<IKaryotype> Karyotypes { get; set; }

		#endregion Properties

		#region ctor

		public DefaultPopulation() {}

		#endregion ctor

		#region IPopulation

		/// <summary>
		/// builds 'size' number of chromosomes and sorts
		/// </summary>
		public virtual void InitializePopulation(IPopulationManager ipo) {
			Manager = ipo;
			Karyotypes = new List<IKaryotype>();
			for (int count = 0; count < Manager.PopSize; count++) {
				IHaploid mom = Manager.CreateHaploid();
				IHaploid dad = Manager.CreateHaploid();
				//add in the random population of chromosomes for each type in the genotype to each haploid
				foreach (KeyValuePair<string, Genotype> kvp in Manager.Genotype) {
					mom.Add(kvp.Key, GetRandomChromosome(kvp.Key));
					dad.Add(kvp.Key, GetRandomChromosome(kvp.Key));
				}
				IKaryotype k = Manager.CreateKaryotype(mom, dad);
				Karyotypes.Add(k);
			}
			Karyotypes = (Manager.FitnessSort.Equals(FitnessSortType.DESC)) 
				? Karyotypes.OrderByDescending(a => a.Fitness).ToList() 
				: Karyotypes.OrderBy(a => a.Fitness).ToList();
		}

		/// <summary>
		/// updates the elite portion by mating the most fit and mutate randomly
		/// </summary>
		public virtual void Evolve() {

			//get a position in the number of karyotes based on crossover
			int elitePos = (int)Math.Round(Karyotypes.Count * Manager.ElitismRatio);

			for (int i = elitePos; i < Karyotypes.Count; i++) { // loop through and replace or mutate non-elites
				//possibly mate or mutate but not both
				if (IfCrossover()) {
					List<IKaryotype> parents = SelectParents();
					Karyotypes[i] = parents.First().Mate(parents.Last()); //replace an elite

					if (IfMutate())
						Karyotypes[i].Mutate();
				} else if (IfMutate()) {
					Karyotypes[i].Mutate();
				}
			}
			
			Karyotypes = (Manager.FitnessSort.Equals(FitnessSortType.DESC)) 
				? Karyotypes.OrderByDescending(a => a.Fitness).ToList() 
				: Karyotypes.OrderBy(a => a.Fitness).ToList();
		}
		
		/// <summary>
		/// select a chromosome fit enough to use
		/// </summary>
		/// <returns></returns>
		public virtual IKaryotype ChooseFitKaryotype() {
			double topFit = Karyotypes.First().Fitness;
			List<IKaryotype> u = GetUniqueKaryotypes();
			List<IKaryotype> lk = (topFit < Manager.FitnessThreshold) // if all values have decayed below the threshold then don't filter any options out
				? u
				: (Manager.FitnessSort.Equals(FitnessSortType.DESC)) 
					? u.Where(a => a.Fitness >= (topFit * Manager.FitnessRatio)).ToList()
					: u.Where(a => a.Fitness <= (topFit * Manager.FitnessRatio)).ToList();
			int newPos = RandomUtil.Instance.Next(lk.Count);
			IKaryotype k = (lk.Any()) // if the filter worked too well just select the first item
				? lk[newPos]
				: Karyotypes.First();

			return k;
		}

		#endregion IPopulation

		#region Methods

		protected IChromosome GetRandomChromosome(string chromosomeName){
			IChromosome c = Manager.CreateChromosome();
			c.FillRandomly(Manager.Genotype[chromosomeName], Manager.Genotype[chromosomeName].GeneLimit);
			return c;	
		}

		public List<IKaryotype> GetUniqueKaryotypes() {
			Dictionary<string, IKaryotype> uniqueSet = new Dictionary<string, IKaryotype>();
			foreach (IKaryotype k in Karyotypes) {
				string uKey = k.Phenotype.DNASequence();
				if (!uniqueSet.ContainsKey(uKey))
					uniqueSet.Add(uKey, k);
			}

			return (Manager.FitnessSort.Equals(FitnessSortType.DESC)) 
				? uniqueSet.Values.OrderByDescending(a => a.Fitness).ToList()
				: uniqueSet.Values.OrderBy(a => a.Fitness).ToList();
		}

		/// <summary>
		/// Selects two chromosomes randomly and tries to improve odds by comparing it's fitness to other chromosomes also randomly selected
		/// </summary>
		protected virtual List<IKaryotype> SelectParents() {
			List<IKaryotype> parents = new List<IKaryotype>(2);

			bool pGender = true;
			//finds two randomly selected parents
			for (int p = 0; p < 2; p++) {
				List<IKaryotype> kars = Karyotypes.Where(k => k.Gender == pGender).ToList();
				parents.Add(kars[RandomUtil.Instance.Next(kars.Count)]);
				
				//it tries TourneySize times to randomly find a better parent
				for (int tournyIndex = 0; tournyIndex < Manager.TourneySize; tournyIndex++) {
					int randomIndex = RandomUtil.Instance.Next(Karyotypes.Count);
					if (Karyotypes[randomIndex].Fitness > parents[p].Fitness) // higher is more fit
						parents[p] = Karyotypes[randomIndex];
				}
				pGender = parents[p].Gender;
			}

			return parents;
		}

		/// <summary>
		/// if a random double is less than the crossover value (high probability) then mate
		/// </summary>
		/// <returns></returns>
		protected bool IfCrossover() { return RandomUtil.Instance.NextDouble() <= Manager.CrossoverRatio; }
		
		/// <summary>
		/// if a random double is less than the mutation rate (low probability) then mutate
		/// </summary>
		/// <returns></returns>
		protected bool IfMutate() { return RandomUtil.Instance.NextDouble() <= Manager.MutationRatio; }
				
		#endregion IPopulation
	}
}
