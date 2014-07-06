using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GA.Lib;
using GA.Lib.Gene;

namespace GA.SC {
	/// <summary>
	/// represents the content tag for any given rendering
	/// </summary>
	public class TagGene : IGene {

		public string Tag { get; set; }
		public bool IsDominant { get; set; }

		public string GeneID {
			get {
				return string.Format("{0}", Tag);
			}
		}

		public TagGene(string tag, bool dominant) {
			Tag = tag;
			IsDominant = dominant;
		}
	}
}
