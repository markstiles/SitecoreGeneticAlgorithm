using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA.Lib.Chromosome {
	public class DefaultHaploid : Dictionary<string, IChromosome>, IHaploid {

		#region ctor

		public DefaultHaploid(){}

		#endregion ctor

		#region ICloneable

		public object Clone() {
			IHaploid cloner = new DefaultHaploid();
			foreach (KeyValuePair<string, IChromosome> kvp in this)
				cloner.Add(kvp.Key, kvp.Value);
			return cloner;
		}

		#endregion ICloneable
	}
}
