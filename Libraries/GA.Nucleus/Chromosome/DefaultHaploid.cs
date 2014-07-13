using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA.Nucleus.Chromosome {
	public class DefaultHaploid : Dictionary<string, IChromosome>, IHaploid {

		#region ctor

		public DefaultHaploid(){}

		#endregion ctor

		#region IHaploid

		public string DNASequence() {
			StringBuilder sb = new StringBuilder();
			foreach (IChromosome c in this.Values) {
				if (sb.Length > 0)
					sb.Append("-");
				sb.Append(c.GeneSequence());
			}
			return sb.ToString();
		}

		#endregion IHaploid

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
