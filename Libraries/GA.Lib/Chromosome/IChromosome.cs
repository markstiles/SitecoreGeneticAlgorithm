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

		double Fitness { get; }

		#endregion Properties

		#region Methods

		List<IChromosome> Mate(IChromosome mate); 
		void Mutate(GenotypeList gt);
		string GeneSequence();

		#endregion Methods
	}
}
