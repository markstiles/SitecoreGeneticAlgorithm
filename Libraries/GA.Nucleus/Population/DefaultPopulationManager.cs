using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GA.Nucleus.Chromosome;
using GA.Nucleus.Gene;
using GA.Nucleus.Population;

namespace GA.Nucleus.Population {
	public class DefaultPopulationManager : IPopulationManager {

		#region Properties

		private float _CrossoverRatio = .8f;
		public float CrossoverRatio {
			get {
				return _CrossoverRatio;
			}
			set {
				_CrossoverRatio = value;
			}
		}
		private float _ElitismRatio = .1f;
		public float ElitismRatio {
			get {
				return _ElitismRatio;
			}
			set {
				_ElitismRatio = value;
			}
		}
		private float _FitnessRatio = 0.9f;
		public float FitnessRatio {
			get {
				return _FitnessRatio;
			}
			set {
				_FitnessRatio = value;
			}
		}

		private float _FitnessThreshold = 1.0f;
		public float FitnessThreshold {
			get {
				return _FitnessThreshold;
			}
			set {
				_FitnessThreshold = value;
			}
		}

		private float _MutationRatio = .1f;
		public float MutationRatio {
			get {
				return _MutationRatio;
			}
			set {
				_MutationRatio = value;
			}
		}
		private int _PopSize = 300;
		public int PopSize {
			get {
				return _PopSize;
				////get the highest number from the chromosome with the most genes
				//int i = 0;
				//foreach (string s in Genotype.Keys) {
				//	int j = (int)Math.Pow(Genotype[s].GeneLimit, Genotype[s].Count);
				//	i = (i < j) ? j : i;
				//}
				//return i * Genotype.Count; //try to calculate the max number of permutations
			}
			set {
				_PopSize = value;
			}
		}
		private int _TourneySize = 5;
		public int TourneySize { 
			get {
				return _TourneySize;
			} 
			set { 
				_TourneySize = value;
			}
		}

		private Type _PopulationType = new DefaultPopulation().GetType();
		public Type PopulationType {
			get {
				return _PopulationType;
			}
			set {
				_PopulationType = value;
			} 
		}
		private Type _KaryotypeType;
		public Type KaryotypeType {
			get {
				if (_KaryotypeType == null)
					throw new NullReferenceException("You must provide a Type object for the KaryotypeType property in the DefaultPopulationManager. The KaryotypeType should implements the IKaryotype interface.");
				return _KaryotypeType;
			}
			set {
				_KaryotypeType = value;
			}
		}
		private Type _HaploidType = new DefaultHaploid().GetType();
		public Type HaploidType {
			get {
				return _HaploidType;
			}
			set {
				_HaploidType = value;
			}
		}
		private Type _ChromosomeType = new DefaultChromosome().GetType();
		public Type ChromosomeType {
			get {
				return _ChromosomeType;
			}
			set {
				_ChromosomeType = value;
			}
		}

		private GenotypeList _Genotype = new GenotypeList();
		public GenotypeList Genotype {
			get {
				return _Genotype;
			}
			set {
				_Genotype = value;
			}
		}

		#endregion Properties

		#region Methods

		public IPopulation CreatePopulation() {
			return (IPopulation)Activator.CreateInstance(PopulationType, this);
		}

		public IKaryotype CreateKaryotype(IHaploid mom, IHaploid dad) {
			return (IKaryotype)Activator.CreateInstance(KaryotypeType, this, mom, dad);
		}

		public IHaploid CreateHaploid() {
			return (IHaploid)Activator.CreateInstance(HaploidType);
		}

		public IChromosome CreateChromosome() {
			return (IChromosome)Activator.CreateInstance(ChromosomeType);
		}

		#endregion Methods
	}
}
