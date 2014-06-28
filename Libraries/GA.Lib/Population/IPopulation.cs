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
		List<IChromosome> Chromosomes { get; set; }

		#endregion Properties

		#region Methods 

		IChromosome CreateChromosome(IPopulationOptions ipo);
		void InitializePopulation(int size);
		void Evolve();
		IChromosome ChooseFitChromosome();
		
		#endregion Methods
	}
}
