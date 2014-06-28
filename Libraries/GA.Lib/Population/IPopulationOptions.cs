using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GA.Lib.Gene;

namespace GA.Lib.Population {
	public interface IPopulationOptions {

		int TourneySize { get; set; }
		int PopSize { get; set; } // number of chromosomes to create
		int MaxGenerations { get; set; } // limit lifetime like a fuse
		float CrossoverRatio { get; set; } // probability for mating
		float ElitismRatio { get; set; } // the percentage that changes as opposed to random feed stock (0.9 means top 10% will change)
		float MutationRatio { get; set; } // probability for mutatating
		float FitnessRatio { get; set; } // how close to the fittest is enough. used to randomly select an item that is close enough to fit
		int GeneCount { get; set; } // how many genes per chromosome

		GenotypeList Genotype { get; set; }
	}
}
