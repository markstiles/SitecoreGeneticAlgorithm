using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using GA.Lib.Gene;
using GA.Lib.Population;

namespace GA.SC.Sublayouts {
	public abstract class GABaseSublayout : BaseSublayout {

		/// <summary>
		/// This adds to engagement value by the tag that was clicked
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected abstract void EngagingEvent_Click(object sender, EventArgs e);
	}
}
