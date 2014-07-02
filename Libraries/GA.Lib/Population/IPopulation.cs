using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GA.Lib.Chromosome;

namespace GA.Lib.Population {
	public interface IPopulation {
		
		#region Properties

		IPopulationOptions Options { get; set; }
		List<IKaryotype> Karyotypes { get; set; }

		#endregion Properties

		#region Methods 

		IKaryotype CreateKaryotype(IPopulationOptions ipo);
		void InitializePopulation();
		void Evolve();
		IChromosome ChooseFitChromosome(string chromosomeName);
		
		#endregion Methods
	}
}
