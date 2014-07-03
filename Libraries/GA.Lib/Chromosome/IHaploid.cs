using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA.Lib.Chromosome {
	public interface IHaploid : IDictionary<string, IChromosome>, ICloneable {

		#region Methods

		string DNASequence();

		#endregion Methods
	}
}
