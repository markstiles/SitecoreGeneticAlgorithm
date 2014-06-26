﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA.Lib {
	public class AlgoPopulationOptions {

		// set starting values
		public int PopSize = 10; // number of chromosomes to create
		public int maxGenerations = 16384; // limit lifetime like a fuse
		public float crossoverRatio = .8f; // probability for mating
		public float elitismRatio = .9f; // the percentage that changes as opposed to random feed stock (0.9 means top 10% will change)
		public float mutationRatio = .3f; // probability for mutatating
		public string targetGene = "mark";
		public float fitnessRatio = .8f; // how close to the fittest is enough. used to randomly select an item that is close enough to fit

		public List<IGene> Genotype = new List<IGene>();
	}
}
