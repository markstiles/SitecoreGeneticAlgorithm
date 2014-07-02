using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GA.Lib.Gene;
using GA.Lib.Population;

namespace GA.Lib.Chromosome {
	public interface IChromosome : IList<IGene>, IComparable<IChromosome> {

		#region Properties

		IPopulationOptions Options { get; set; }

		double Fitness { get; }

		#endregion Properties

		#region Methods

		string GeneSequence();
		void FillRandomly(string chromosomeName);

		#endregion Methods
	}
}
