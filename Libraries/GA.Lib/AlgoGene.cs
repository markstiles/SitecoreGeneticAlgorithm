using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA.Lib {
	/// <summary>
	/// represents a rendering
	/// </summary>
	public class AlgoGene : IGene {
		public string PlaceholderID { get; set; }
		public string Tag { get; set; }
		public string GeneID {
			get {
				return string.Format("{1}", PlaceholderID, Tag);
				//return string.Format("{0}-{1}", PlaceholderID, Tag);
			}
		}

		public AlgoGene(string PHID, string tag) {
			PlaceholderID = PHID;
			Tag = tag;
		}
	}
}
