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
			Karyotypes = new List<IKaryotype>();
            for (int count = 0; count < ipo.PopSize; count++) {
                IHaploid mom = ipo.CreateHaploid();
                IHaploid dad = ipo.CreateHaploid();
				//add in the random population of chromosomes for each type in the genotype to each haploid
                foreach (KeyValuePair<string, GenePool> kvp in ipo.ChromosomePool) {
					mom.Add(kvp.Key, GetRandomChromosome(ipo, kvp.Key));
                    dad.Add(kvp.Key, GetRandomChromosome(ipo, kvp.Key));
				}
                IKaryotype k = ipo.CreateKaryotype(mom, dad);
				Karyotypes.Add(k);
			}
            Karyotypes = (ipo.FitnessSort.Equals(FitnessSortType.DESC)) 
				? Karyotypes.OrderByDescending(a => a.Fitness()).ToList() 
				: Karyotypes.OrderBy(a => a.Fitness()).ToList();
		}

		/// <summary>
		/// updates the elite portion by mating the most fit and mutate randomly
		/// </summary>
		public virtual void Evolve(IPopulationManager ipo) {

			//get a position in the number of karyotes based on crossover
            int elitePos = (int)Math.Round(Karyotypes.Count * ipo.ElitismRatio);
			List<IKaryotype> kList = Karyotypes;
				
			for (int i = elitePos; i < Karyotypes.Count; i++) { // loop through and replace or mutate non-elites

				IKaryotype p1 = Karyotypes[i];
				//possibly mate or mutate but not both
                if (IfCrossover(ipo.CrossoverRatio)) {
					IKaryotype p2 = SelectMate(p1, i, kList);
					if(p2 != null) // only occurs when there are no mates of opposing gender. probably in small elite pools
						Karyotypes[i] = p1.Mate(ipo, p2);

                    if (IfMutate(ipo.MutationRatio))
						Karyotypes[i].Mutate(ipo);
                } else if (IfMutate(ipo.MutationRatio)) {
					Karyotypes[i].Mutate(ipo);
				}
			}
			
			Karyotypes = (ipo.FitnessSort.Equals(FitnessSortType.DESC))
                ? Karyotypes.OrderByDescending(a => a.Fitness()).ToList()
                : Karyotypes.OrderBy(a => a.Fitness()).ToList();
		}
		
		/// <summary>
		/// select a chromosome fit enough to use
		/// </summary>
		/// <returns></returns>
		public virtual IKaryotype ChooseFitKaryotype(double fitnessRatio, double fitnessThreshold, FitnessSortType fst) {
			double topFit = Karyotypes.First().Fitness();
			List<IKaryotype> u = GetUniqueKaryotypes(fst);
			double fitRange = topFit * fitnessRatio;
			List<IKaryotype> lk = (topFit < fitnessThreshold) // if all values have decayed below the threshold then don't filter any options out
				? u
				: (fst.Equals(FitnessSortType.DESC))
                    ? u.Where(a => a.Fitness() >= fitRange).ToList()
                    : u.Where(a => a.Fitness() <= fitRange).ToList();
			int newPos = RandomUtil.Instance.Next(lk.Count);
			IKaryotype k = (lk.Any()) // if the filter worked too well just select the first item
				? lk[newPos]
				: Karyotypes.First();

			return k;
		}

		#endregion IPopulation

		#region Methods

		protected IChromosome GetRandomChromosome(IPopulationManager ipo, string chromosomeName){
			IChromosome c = ipo.CreateChromosome();
            c.FillRandomly(ipo.ChromosomePool[chromosomeName], ipo.ChromosomePool[chromosomeName].GeneLimit);
			return c;	
		}

		public List<IKaryotype> GetUniqueKaryotypes(FitnessSortType fst) {
			Dictionary<string, IKaryotype> uniqueSet = new Dictionary<string, IKaryotype>();
			foreach (IKaryotype k in Karyotypes) {
                string uKey = k.Phenotype.ChromosomeSequence();
				if (!uniqueSet.ContainsKey(uKey))
					uniqueSet.Add(uKey, k);
			}

            return (fst.Equals(FitnessSortType.DESC))
                ? uniqueSet.Values.OrderByDescending(a => a.Fitness()).ToList()
                : uniqueSet.Values.OrderBy(a => a.Fitness()).ToList();
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
        protected bool IfCrossover(double crossoverRatio) { return RandomUtil.Instance.NextDouble() <= crossoverRatio; }
		
		/// <summary>
		/// if a random double is less than the mutation rate (low probability) then mutate
		/// </summary>
		/// <returns></returns>
        protected bool IfMutate(double mutationRatio) { return RandomUtil.Instance.NextDouble() <= mutationRatio; }
				
		#endregion IPopulation
	}
}
