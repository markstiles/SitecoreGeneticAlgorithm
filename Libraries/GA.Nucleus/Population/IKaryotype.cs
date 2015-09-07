using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GA.Nucleus.Chromosome;
using GA.Nucleus.Gene;

namespace GA.Nucleus.Population {
	public interface IKaryotype : IComparable<IKaryotype> {

		#region Properties

		bool Gender { get; set; } // true = mother, false = father
		int Age { get; set; }
		IHaploid MothersHaploid { get; set; }
		IHaploid FathersHaploid { get; set; }

		IHaploid Phenotype { get; } // compares the dominance settings

		#endregion Properties

		#region Methods

        IKaryotype Mate(IPopulationManager ipo, IKaryotype mate);
		void Mutate(IPopulationManager ipo);
        double Fitness();

		#endregion Methods
	}
}
