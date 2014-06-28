using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;
using GA.Lib;
using GA.Lib.Chromosome;
using GA.Lib.Population;

namespace GA.SC {
	public class SCPopulation : BasePopulation {

		#region ctor

		public SCPopulation(IPopulationOptions apo) : base(apo) { }

		#endregion ctor

		/// <summary>
		/// This is a temporary storage solution but each user's population should probaby be stored in a bucket.
		/// </summary>
		#region Storage and Retrieval

		protected static readonly string PopKey = "population";

		public static void RestartPop(IPopulationOptions apo) {
			SCChromosome.EngagementValues.Clear();
			HttpContext.Current.Session[PopKey] = CreatePop(apo); 
		}

		public static SCPopulation GetPop(IPopulationOptions apo) {
			if (HttpContext.Current.Session[PopKey] != null)
				return (SCPopulation)HttpContext.Current.Session[PopKey];

			SCPopulation p = CreatePop(apo);
			HttpContext.Current.Session[PopKey] = p;
			return p;
		}

		public static SCPopulation CreatePop(IPopulationOptions apo) {
			return new SCPopulation(apo);
		}

		public static void SetPop(SCPopulation p) {
			HttpContext.Current.Session[PopKey] = p;
		}

		#endregion Storage and Retrieval

		#region IPopulation

		public override IChromosome CreateChromosome(IPopulationOptions ipo){
			return (IChromosome)new SCChromosome(ipo);
		}

		#endregion IPopulation
	}
}
