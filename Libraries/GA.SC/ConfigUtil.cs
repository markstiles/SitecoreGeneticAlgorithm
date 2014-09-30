using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using GA.SC.EV;
using Sitecore.Configuration;
using GA.Nucleus.Population;

namespace GA.SC {
	public class ConfigUtil {

		#region Fields 

		public string GAName = string.Empty;

		#endregion Fields

		#region ctor

		public ConfigUtil(string gaName) {
			GAName = gaName;
		}

		#endregion ctor

		#region Static Properties

		public static XmlNode GARoot {
			get {
				return Factory.GetConfigNode("geneticalgorithms");
			}
		}

		public static string SiteProperty {
			get {
				return GARoot.Attributes["siteProperty"].Value;
			}
		}

		private static ConfigUtil _Context;
		public static ConfigUtil Context {
			get {
				if(_Context == null)
					_Context = CurrentSiteContext();
				return _Context;
			}
			set {
				_Context = value;
			}
		}

		public static ConfigUtil CurrentSiteContext() {
			return new ConfigUtil(Sitecore.Context.Site.Properties[ConfigUtil.SiteProperty]);
		}

		#endregion Static Properties

		#region Properties

		public string ChromosomeName {
			get {
				return GetGAAttribute("chromosomeName");
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
		
		public float CrossoverRatio {
			get {
				return float.Parse(GetGAAttribute("crossoverRatio"));
			}
		}

		public string DatasourceValue {
			get {
				return GetGAAttribute("datasourceValue");
			}
		}

		public float ElitismRatio {
			get {
				return float.Parse(GetGAAttribute("elitismRatio"));
			}
		}

		public IEngagementValueProvider EVProvider {
			get {
				string v = GARoot[GAName].Attributes["evProviderType"].Value;
				Type t = Type.GetType(v);
				return (IEngagementValueProvider)Activator.CreateInstance(t);
			}
		}

		public float FitnessRatio {
			get {
				return float.Parse(GetGAAttribute("fitnessRatio"));
			}
		}

		public float FitnessThreshold {
			get {
				return float.Parse(GetGAAttribute("fitnessThreshold"));
			}
		}

		public FitnessSortType FitnessSort {
			get {
				return (FitnessSortType)Enum.Parse(typeof(FitnessSortType), GetGAAttribute("fitnessSort"));
			}
		}

		public Type KaryotypeType {
			get {
				string v = GetGAAttribute("karyotypeType");
				return Type.GetType(v);
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

		public Type PopulationType {
			get {
				string v = GetGAAttribute("populationType");
				return Type.GetType(v);
			}
		}

		public string TagFolder {
			get {
				return GetGAAttribute("tagFolder");
			}
		}
		
		public int TourneySize {
			get {
				return int.Parse(GetGAAttribute("tourneySize"));
			}
		}

		public IValueModifier ValueModifier {
			get {
				XmlNode x = GARoot[GAName]["evModifier"];
				string t = x.Attributes["type"].Value;
				Type vmt = Type.GetType(t);
				float m = float.Parse(x.Attributes["minRatio"].Value);
				int d = int.Parse(x.Attributes["decimalPlaces"].Value);
				TimespanAspect ta = (TimespanAspect)Enum.Parse(typeof(TimespanAspect), x.Attributes["timespanAspect"].Value);
				int hl = int.Parse(x.Attributes["halfLife"].Value);
				
				return (IValueModifier)Activator.CreateInstance(vmt, m, d, ta, hl);
			}
		}

		#endregion Properties

		#region Methods 
		
		public string GetGAAttribute(string prop) {
			return GARoot[GAName].Attributes[prop].Value;
		}

		#endregion Methods
	}
}


