using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA.Nucleus.Extensions {
	public static class SystemExtensions {
		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="items"></param>
		/// <param name="pred"></param>
		/// <returns></returns>
		public static List<T> UniqueList<T>(this List<T> items, Func<T, string> pred) {
			Dictionary<string, T> d = new Dictionary<string, T>();
			foreach(T el in items) {	
				string s = pred(el);
				if(!d.ContainsKey(s))
					d.Add(s, el);
			}
			return d.Values.ToList();
		}
	}
}
