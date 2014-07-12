using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Sitecore.Configuration;

namespace GA.SC {
	public class ConfigUtil {

		public string GAName = string.Empty;

		public ConfigUtil(string gaName) {
			GAName = gaName;
		}

		public static XmlNode GARoot {
			get {
				return Factory.GetConfigNode("geneticalgorithms");
			}
		}

		public string GetGAAttribute(string prop) {
			return GARoot[GAName].Attributes[prop].Value;
		}

		public Type PopulationType {
			get {
				string v = GetGAAttribute("populationType");
				return Type.GetType(v);
			}
		}

		public Type KaryotypeType {
			get {
				string v = GetGAAttribute("karyotypeType");
				return Type.GetType(v);
			}
		}

		public string DatasourceValue {
			get {
				return GetGAAttribute("datasourceValue");
			}
		}

		public string ContentFolder {
			get {
				return GetGAAttribute("contentFolder");
			}
		}
		
		public string ContentTagField {
			get {
				return GetGAAttribute("contentTagField");
			}
		}
		
		public string TagFolder {
			get {
				return GetGAAttribute("tagFolder");
			}
		}

		public string ChromosomeName {
			get {
				return GetGAAttribute("chromosomeName"); 
			}
		}

		public float CrossoverRatio {
			get {
				string s = GetGAAttribute("crossoverRatio");
				return float.Parse(s); 
			}
		}

		public float ElitismRatio {
			get {
				return float.Parse(GetGAAttribute("elitismRatio"));
			}
		}

		public float FitnessRatio {
			get {
				return float.Parse(GetGAAttribute("fitnessRatio"));
			}
		}

		public float MutationRatio {
			get {
				return float.Parse(GetGAAttribute("mutationRatio"));
			}
		}

		public int PopSize {
			get {
				return int.Parse(GetGAAttribute("popSize"));
			}
		}

		public int TourneySize {
			get {
				return int.Parse(GetGAAttribute("tourneySize"));
			}
		}

		public static string SiteProperty {
			get {
				return GARoot.Attributes["siteProperty"].Value;
			}
		}
	}
}


