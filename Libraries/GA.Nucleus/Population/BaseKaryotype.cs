using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GA.Nucleus.Chromosome;
using GA.Nucleus.Gene;

namespace GA.Nucleus.Population {
	public abstract class BaseKaryotype : IKaryotype {

		#region Properties 

		public bool Gender { get; set; }
		public int Age { get; set; }
		public IHaploid MothersHaploid { get; set; }
		public IHaploid FathersHaploid { get; set; }
		private IHaploid _Phenotype;
		public IHaploid Phenotype {
			get {
				if (_Phenotype != null)
                    return _Phenotype;

				_Phenotype = (IHaploid)Activator.CreateInstance(MothersHaploid.GetType());
				foreach (string key in MothersHaploid.Keys) {
					IChromosome mc = MothersHaploid[key];
					IChromosome fc = FathersHaploid[key];
					IChromosome newC = (IChromosome)Activator.CreateInstance(mc.GetType());
					//compare each gene in MothersHaploid and FathersHaploid for dominance
					for (int i = 0; i < mc.Count; i++) {
						if (mc[i].IsDominant && !fc[i].IsDominant) // use mother if it's the only dominant one
							newC.Insert(i, mc[i]);
						else if (!mc[i].IsDominant && fc[i].IsDominant) // use father if it's' the only dominant one
							newC.Insert(i, fc[i]);
						else
							newC.Insert(i, (RandomUtil.NextBool()) ? mc[i] : fc[i]); // or just choose one at random
					}
					_Phenotype.Add(key, newC);
				}
				return _Phenotype;
			}
		}
		
		#endregion Properties 

		#region ctor

		public BaseKaryotype(IHaploid mom, IHaploid dad) {
			MothersHaploid = mom;
			FathersHaploid = dad;
			Gender = RandomUtil.NextBool(); // 50/50 chance
			Age = 0;
		}

		#endregion ctor

		#region IKaryotype

		/// <summary>
		/// changes a random character in the gene to a random character
		/// </summary>
		public virtual void Mutate(IPopulationManager ipo) {
			//randomly select a chromosome from the master genotype list
            string chromoKey = ipo.ChromosomePool.Keys.ToList()[RandomUtil.Instance.Next(ipo.ChromosomePool.Keys.Count)];
            GenePool rg = ipo.ChromosomePool[chromoKey];
			//randomly get a gene from that chromosome's genotype 
			IGene g = rg[RandomUtil.Instance.Next(rg.Count)];
			//choose a random Haploid to modify
			IHaploid rh = (RandomUtil.NextBool()) ? MothersHaploid : FathersHaploid;
			//choose the same chromosome from the genotype on the haploid but a random gene to replace it
            rh[chromoKey][RandomUtil.Instance.Next(rh[chromoKey].Count)] = g;
		}

		/// <summary>
		/// take the genes from this and a mate and split them in half and swap them
		/// </summary>
		public virtual IKaryotype Mate(IPopulationManager ipo, IKaryotype mate) {

			// run meiosis for each to generate some seedlings
			List<IHaploid> mh = (mate.Gender) ? Meiosis(mate) : Meiosis(this);
			List<IHaploid> fh = (mate.Gender) ? Meiosis(this) : Meiosis(mate);

			// randomly pick a haploid from each and create a new karyotype
			IHaploid mom = mh[RandomUtil.Instance.Next(mh.Count)];
			IHaploid dad = fh[RandomUtil.Instance.Next(fh.Count)];
            IKaryotype newK = ipo.CreateKaryotype(mom, dad);
			
			return newK;
		}

		#endregion IKaryotype

		#region IComparable

		public int CompareTo(IKaryotype other) {
			return Fitness().CompareTo(other.Fitness());
		}

		#endregion IComparable

		#region Methods

        public abstract double Fitness();

		/// <summary>
		/// creates a duplication then crossover of existing haploids to make seeds for the next generation
		/// </summary>
		/// <param name="k"></param>
		/// <returns></returns>
		protected virtual List<IHaploid> Meiosis(IKaryotype k) {

			// Interphase - duplicate the haploids
			IHaploid f1 = (IHaploid)k.FathersHaploid.Clone();
			IHaploid f2 = (IHaploid)k.FathersHaploid.Clone();
			IHaploid m1 = (IHaploid)k.MothersHaploid.Clone();
			IHaploid m2 = (IHaploid)k.MothersHaploid.Clone();

			// Prophase - start crossover (f1 and m1) and (f2 and m2) by randomly start mixing the genes in each chromosome
			foreach (string key in k.FathersHaploid.Keys) {

				// Metaphase, Anaphase and Telophase - splitting up parts
				IChromosome f1c = f1[key];
				IChromosome m1c = m1[key];

				IChromosome f2c = f2[key];
				IChromosome m2c = m2[key];

                for (int i = 0; i < f1c.Count; i++) { // randomly choose to switch genes
					Crossover(f1c, m1c, i);
					Crossover(f2c, m2c, i);
				}
			}

			return new List<IHaploid> { f1, f2, m1, m2 };
		}

		/// <summary>
		/// randomly decide whether or not to switch a gene at a specific position
		/// </summary>
		/// <param name="father"></param>
		/// <param name="mother"></param>
		/// <param name="pos"></param>
		protected void Crossover(IChromosome father, IChromosome mother, int pos) {
			if (RandomUtil.NextBool()) {
				IGene tempGene = father[pos];
				father[pos] = mother[pos];
				mother[pos] = tempGene;
			}
		}
		
		#endregion Methods
	}
}
