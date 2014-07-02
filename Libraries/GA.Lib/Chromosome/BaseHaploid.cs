using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA.Lib.Chromosome {
	public class BaseHaploid : Dictionary<string, IChromosome>, IHaploid {

		#region ctor

		public BaseHaploid(){}

		#endregion ctor

		#region ICloneable

		public object Clone() {
			IHaploid cloner = new BaseHaploid();
			foreach (KeyValuePair<string, IChromosome> kvp in this)
				cloner.Add(kvp.Key, kvp.Value);
			return cloner;
		}

		#endregion ICloneable
	}
}
