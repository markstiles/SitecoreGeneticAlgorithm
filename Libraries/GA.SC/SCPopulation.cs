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
	public class SCPopulation : DefaultPopulation {

		#region ctor

		public SCPopulation() : base() { }

		#endregion ctor

		/// <summary>
		/// This is a temporary storage solution but each user's population should probaby be stored in a bucket.
		/// </summary>
		#region Storage and Retrieval

		protected static readonly string PopKey = "population";

		public static void RestartPop(IPopulationManager apo) {
			HttpContext.Current.Session[PopKey] = CreatePop(apo); 
		}

		public static SCPopulation GetPop(IPopulationManager apo) {
			if (HttpContext.Current.Session[PopKey] != null)
				return (SCPopulation)HttpContext.Current.Session[PopKey];

			SCPopulation p = CreatePop(apo);
			HttpContext.Current.Session[PopKey] = p;
			return p;
		}

		public static SCPopulation CreatePop(IPopulationManager apo) {
			SCPopulation p = new SCPopulation();
			p.InitializePopulation(apo);
			return p;
		}

		public static void SetPop(SCPopulation p) {
			HttpContext.Current.Session[PopKey] = p;
		}

		#endregion Storage and Retrieval
	}
}
