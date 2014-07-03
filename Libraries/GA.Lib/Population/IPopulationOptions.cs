using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GA.Lib.Gene;

namespace GA.Lib.Population {
	public interface IPopulationOptions {

		float CrossoverRatio { get; set; } // probability for mating
		float ElitismRatio { get; set; } // the percentage that changes as opposed to random feed stock (0.9 means top 10% will change)
		float FitnessRatio { get; set; } // how close to the fittest is enough. used to randomly select an item that is close enough to fit
		int GeneCount { get; set; } // how many genes per chromosome
		float MutationRatio { get; set; } // probability for mutatating
		int PopSize { get; } // number of chromosomes to create
		int PopScalar { get; set; } // number to population size by to get a larger number of chromosomes
		int TourneySize { get; set; }

		Type KaryotypeType { get; set; }
		Type HaploidType { get; set; }
		Type ChromosomeType { get; set; }

		GenotypeList Genotype { get; set; }
	}
}
