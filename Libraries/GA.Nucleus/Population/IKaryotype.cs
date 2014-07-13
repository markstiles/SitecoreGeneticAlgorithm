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

		IPopulationManager Manager { get; set; }

		bool Gender { get; set; } // true = mother, false = father

		IHaploid MothersHaploid { get; set; }
		IHaploid FathersHaploid { get; set; }

		IHaploid ExpressedHaploid { get; } // compares the dominance settings

		double Fitness { get; }

		#endregion Properties

		#region Methods

		IKaryotype Mate(IKaryotype mate);
		void Mutate();

		#endregion Methods
	}
}
