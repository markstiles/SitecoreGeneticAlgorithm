using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GA.Nucleus;
using GA.Nucleus.Gene;

namespace GA.SC.UI.SampleSite.Gene {
	/// <summary>
	/// represents the content tag for any given rendering
	/// </summary>
	public class TagGene : IGene {

		public string GeneID { get; set; }
        public string GeneName { get; set; }
        public bool IsDominant { get; set; }

        public TagGene() { }

		public TagGene(string tag, string id, bool dominant) {
			GeneName = tag;
            GeneID = id;
			IsDominant = dominant;
		}
	}
}
