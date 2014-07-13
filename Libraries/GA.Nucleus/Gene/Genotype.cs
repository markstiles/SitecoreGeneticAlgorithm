using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA.Nucleus.Gene {
	public class Genotype : List<IGene> {

		private int _GeneLimit = 0;
		public int GeneLimit {
			get {
				return _GeneLimit;
			}
			set {
				_GeneLimit = value;
			}
		}
	}
}
