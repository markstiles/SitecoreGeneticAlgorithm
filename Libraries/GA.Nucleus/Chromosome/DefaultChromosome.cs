using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GA.Nucleus.Gene;
using GA.Nucleus.Population;

namespace GA.Nucleus.Chromosome {
	public class DefaultChromosome : List<IGene>, IChromosome {
	
		#region ctor

		public DefaultChromosome(){ }

		#endregion ctor

		#region IChromosome

		public virtual string GeneSequence() {
            StringBuilder sbGenes = new StringBuilder();
            foreach (IGene g in this) {
                if (sbGenes.Length > 0)
                    sbGenes.Append(",");
                sbGenes.AppendFormat("\"{0}\"", g.GeneName);
            }
            return sbGenes.ToString();
		}

		public virtual void FillRandomly(List<IGene> genepool, int count) {
			for (int j = 0; j < count; j++) {
				IGene g = genepool[RandomUtil.Instance.Next(genepool.Count)];
				this.Add(g);
			}
		}

		#endregion IChromosome

        #region ICloneable

        public object Clone() {
            IChromosome cloner = (IChromosome)Activator.CreateInstance(this.GetType());
            foreach (IGene g in this) {
                IGene newG = (IGene)Activator.CreateInstance(g.GetType());
                newG.GeneID = g.GeneID;
                newG.GeneName = g.GeneName;
                newG.IsDominant = g.IsDominant;
                cloner.Add(newG);
            }
            return cloner;
        }

        #endregion ICloneable

	}
}
