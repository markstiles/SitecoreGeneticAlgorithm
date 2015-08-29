using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA.Nucleus.Gene {
	public interface IGene {

		#region Properties

		string GeneID { get; }
        string GeneName { get; }
		bool IsDominant { get; set; }

		#endregion Properties
	}
}
