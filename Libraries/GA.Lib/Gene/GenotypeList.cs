using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA.Lib.Gene {
	public class GenotypeList : List<IGene> {

		public IGene GetRandom() {
			return this[RandomUtil.Instance.Next(0, this.Count)];
		}
	}
}
