using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GA.Lib.Gene;
using GA.Lib.Population;

namespace GA.Lib.Population {
	public class PopulationOptions : IPopulationOptions {
		
		private int _TourneySize = 3;
		public int TourneySize { 
			get {
				return _TourneySize;
			} 
			set { 
				_TourneySize = value;
			}
		}
		private int _PopSize = 10;
		public int PopSize {
			get {
				return _PopSize;
			}
			set {
				_PopSize = value;
			}
		}
		private int _MaxGenerations = 16384;
		public int MaxGenerations {
			get {
				return _MaxGenerations;
			}
			set {
				_MaxGenerations = value;
			}
		}
		private float _CrossoverRatio = .8f;
		public float CrossoverRatio {
			get {
				return _CrossoverRatio;
			}
			set {
				_CrossoverRatio = value;
			}
		}
		private float _ElitismRatio = .9f;
		public float ElitismRatio {
			get {
				return _ElitismRatio;
			}
			set {
				_ElitismRatio = value;
			}
		}
		private float _MutationRatio = .3f;
		public float MutationRatio {
			get {
				return _MutationRatio;
			}
			set {
				_MutationRatio = value;
			}
		}
		private float _FitnessRatio = .8f;
		public float FitnessRatio {
			get {
				return _FitnessRatio;
			}
			set {
				_FitnessRatio = value;
			}
		}
		private int _GeneCount = 0;
		public int GeneCount {
			get {
				return _GeneCount;
			}
			set {
				_GeneCount = value;
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
	}
}
