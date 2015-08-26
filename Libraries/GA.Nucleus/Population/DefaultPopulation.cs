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
                foreach (KeyValuePair<string, GenePool> kvp in Manager.ChromosomePool) {
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
			List<IKaryotype> kList = Karyotypes;
				
			for (int i = elitePos; i < Karyotypes.Count; i++) { // loop through and replace or mutate non-elites

				IKaryotype p1 = Karyotypes[i];
				//possibly mate or mutate but not both
				if (IfCrossover()) {
					IKaryotype p2 = SelectMate(p1, i, kList);
					if(p2 != null) // only occurs when there are no mates of opposing gender. probably in small elite pools
						Karyotypes[i] = p1.Mate(p2); 

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
			double fitRange = topFit * Manager.FitnessRatio;
			List<IKaryotype> lk = (topFit < Manager.FitnessThreshold) // if all values have decayed below the threshold then don't filter any options out
				? u
				: (Manager.FitnessSort.Equals(FitnessSortType.DESC)) 
					? u.Where(a => a.Fitness >= fitRange).ToList()
					: u.Where(a => a.Fitness <= fitRange).ToList();
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
            c.FillRandomly(Manager.ChromosomePool[chromosomeName], Manager.ChromosomePool[chromosomeName].GeneLimit);
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
		protected virtual IKaryotype SelectMate(IKaryotype k1, int station, List<IKaryotype> kList) {

			int mobility = 4;
			int upMob = station - mobility;
			upMob = (upMob < 0) ? 0 : upMob;
			List<IKaryotype> kars = Karyotypes.Skip(upMob).Take(mobility * 2).Where(k => k.Gender != k1.Gender).ToList();
			if (!kars.Any())
				return null;
			
			IKaryotype k2 = kars[RandomUtil.Instance.Next(kars.Count)];
			
			return k2;
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
