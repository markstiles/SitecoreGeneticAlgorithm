using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GA.Lib.Gene;
using GA.Lib.Population;

namespace GA.Lib.Chromosome {
	public class DefaultChromosome : List<IGene>, IChromosome {
	
		#region ctor

		public DefaultChromosome(){ }

		#endregion ctor

		#region IChromosome

		public virtual string GeneSequence() {
			StringBuilder sb = new StringBuilder();
			foreach (IGene g in this)
				sb.Append(g.GeneID);
			return sb.ToString();
		}

		public virtual void FillRandomly(List<IGene> genepool, int count) {
			for (int j = 0; j < count; j++) {
				IGene g = genepool[RandomUtil.Instance.Next(genepool.Count)];
				this.Add(g);
			}
		}

		#endregion IChromosome

		#region Methods
		
		public void AddRange(IEnumerable<KeyValuePair<string, IGene>> g) {
			foreach (KeyValuePair<string, IGene> kvp in g)
				this.Add(kvp.Value);
		}

		#endregion Methods
	}
}
