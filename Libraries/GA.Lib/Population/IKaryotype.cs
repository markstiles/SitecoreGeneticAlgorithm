﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GA.Lib.Chromosome;
using GA.Lib.Gene;

namespace GA.Lib.Population {
	public interface IKaryotype {

		#region Properties

		IPopulationOptions Options { get; set; }

		bool Gender { get; set; } // true = mother, false = father

		IHaploid MothersHaploid { get; set; }
		IHaploid FathersHaploid { get; set; }

		IHaploid ExpressedHaploid { get; } // compares the dominant settings

		#endregion Properties

		#region Methods

		IKaryotype Mate(IKaryotype mate);
		void Mutate();

		#endregion Methods
	}
}
