using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GA.Nucleus.Chromosome;

namespace GA.Nucleus.Population {
	public interface IPopulation {
		
		#region Properties

		List<IKaryotype> Karyotypes { get; set; }

		#endregion Properties

		#region Methods 

		void InitializePopulation(IPopulationManager ipo);
        void Evolve(IPopulationManager ipo);
        IKaryotype ChooseFitKaryotype(double fitnessRatio, double fitnessThreshold, FitnessSortType fst);
        List<IKaryotype> GetUniqueKaryotypes(FitnessSortType fst); 

		#endregion Methods
	}
}
