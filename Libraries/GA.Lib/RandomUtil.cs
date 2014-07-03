using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA.Lib {
	public sealed class RandomUtil {

		private static Random _rand = new Random(Environment.TickCount);

		public static Random Instance {
			get { return _rand; }
		}

		public static bool NextBool() {
			return (RandomUtil.Instance.Next(0, 1) == 0);
		}
	}
}
