using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace GA.SC.UI.SampleSite {
	public static class TagUtil {
		public static List<string> GetTags(Item i) {
			List<string> tags = (from string val in ((DelimitedField)i.Fields["Tags"]).Items
								 select i.Database.GetItem(val).DisplayName).ToList();
			if (tags == null || !tags.Any())
				return new List<string>();
			return tags;
		}
	}
}
