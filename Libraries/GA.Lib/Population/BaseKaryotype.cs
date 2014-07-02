﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GA.Lib.Chromosome;
using GA.Lib.Gene;

namespace GA.Lib.Population {
	public abstract class BaseKaryotype : IKaryotype {

		#region Properties 

		public IPopulationOptions Options { get; set; }
		public bool Gender { get; set; } 
		public IHaploid MothersHaploid { get; set; }
		public IHaploid FathersHaploid { get; set; }
		private IHaploid _ExpressedHaploid;
		public IHaploid ExpressedHaploid {
			get {
				//create new every time because it might change due to mutation
				IHaploid _ExpressedHaploid = (IHaploid)Activator.CreateInstance(MothersHaploid.GetType());
				foreach (string key in MothersHaploid.Keys) {
					IChromosome mc = MothersHaploid[key];
					IChromosome fc = FathersHaploid[key];
					IChromosome newC = (IChromosome)Activator.CreateInstance(mc.GetType(), Options);
					//compare each gene in MothersHaploid and FathersHaploid for dominance
					for(int i = 0; i < Options.GeneCount; i++){
						if(mc[i].IsDominant && !fc[i].IsDominant) // use mother if only it is dominant
							newC.Insert(i, mc[i]);
						if (!mc[i].IsDominant && fc[i].IsDominant) // use father if only it is dominant
							newC.Insert(i, fc[i]);
						else
							newC.Insert(i, (RandomUtil.RandomBool()) ? mc[i] : fc[i]); // choose one at random
					}
					_ExpressedHaploid.Add(key, newC);
				}
				return _ExpressedHaploid;
			}
		} 

		#endregion Properties 

		#region ctor

		public BaseKaryotype(IPopulationOptions ipo, IHaploid mom, IHaploid dad) {
			Options = ipo;
			MothersHaploid = mom;
			FathersHaploid = dad;
			Gender = RandomUtil.RandomBool(); // 50/50 chance
		}

		#endregion ctor

		#region IKaryotype

		/// <summary>
		/// creates a duplication then crossover of existing gametes
		/// </summary>
		/// <param name="k"></param>
		/// <returns></returns>
		protected virtual List<IHaploid> Meiosis(IKaryotype k) {
			
			// Interphase - duplicate the haploids
			IHaploid f1 = (IHaploid)k.FathersHaploid.Clone();
			IHaploid f2 = (IHaploid)k.FathersHaploid.Clone();
			IHaploid m1 = (IHaploid)k.MothersHaploid.Clone();
			IHaploid m2 = (IHaploid)k.MothersHaploid.Clone();
			
			// Prophase - start crossover (f1 and m1) and (f2 and m2) randomly start mixing the 
			foreach (string key in k.FathersHaploid.Keys) {
				
				// Metaphase, Anaphase and Telophase - splitting up parts
				IChromosome f1c = f1[key];
				IChromosome m1c = m1[key];
				
				IChromosome f2c = f2[key];
				IChromosome m2c = m2[key];

				for (int i = 0; i < Options.GeneCount; i++) {
					//randomly choose to switch first gene on first pair
					if (RandomUtil.RandomBool()) {
						IGene tempGene = f1c[i];
						f1c[i] = m1c[i];
						m1c[i] = tempGene;
					}

					//randomly choose to switch gene on second pair
					if (RandomUtil.RandomBool()) {
						IGene tempGene = f2c[i];
						f2c[i] = m2c[i];
						m2c[i] = tempGene;
					}
				}
			}

			return new List<IHaploid> { f1, f2, m1, m2 };
		}

		/// <summary>
		/// changes a random character in the gene to a random character
		/// </summary>
		public virtual void Mutate() {
			//randomly select a chromosome from the master genotype list
			string chromoKey = Options.Genotype.Keys.ToList()[RandomUtil.Instance.Next(0, Options.Genotype.Keys.Count)];
			List<IGene> rg = Options.Genotype[chromoKey];
			//randomly get a gene from that chromosome's genotype 
			IGene g = rg[RandomUtil.Instance.Next(0, rg.Count)];
			//choose a random Haploid to modify
			IHaploid rh = (RandomUtil.RandomBool()) ? MothersHaploid : FathersHaploid;
			//choose the same chromosome on the haploid but a random gene
			rh[chromoKey][RandomUtil.Instance.Next(0, Options.GeneCount)] = g;
		}

		/// <summary>
		/// take the genes from this and a mate and split them in half and swap them
		/// </summary>
		public virtual IKaryotype Mate(IKaryotype mate) {

			// run meiosis for each
			List<IHaploid> mh = (mate.Gender) ? Meiosis(mate) : Meiosis(this);
			List<IHaploid> fh = (mate.Gender) ? Meiosis(this) : Meiosis(mate);

			// randomly pick a haploid from each and create a new karyotype
			Object[] args = { Options };
			Type t = this.GetType();
			IHaploid mom = mh[RandomUtil.Instance.Next(0, mh.Count)];
			IHaploid dad = fh[RandomUtil.Instance.Next(0, fh.Count)];
			IKaryotype newK = (IKaryotype)Activator.CreateInstance(t, Options, mom, dad);
			
			return newK;
		}

		#endregion IKaryotype
	}
}
