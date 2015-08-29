using GA.Nucleus.Population;
using Sitecore.Configuration;
using Sitecore.Sites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA.SC.UI.SampleSite {
    public static class Statics {

        public static readonly string GAName = "gaName";
        public static readonly string DatasourceValue = "{9F8F8F2C-2980-4961-94A1-49A48209C2FD}";
        public static readonly string TagFolder = "{DB75869F-0B03-471C-B366-5FA7DAD4DF65}";
        public static readonly string ContentFolder = "{D3A2510D-C989-485E-BFD2-8D4B651EFCE8}";
        public static readonly string ContentTagField = "Tags";
        public static readonly string ChromoKey = "pageContent";
                
        public static string GetGAName(SiteContext sc){
            return sc.Properties[GAName];
        }

        public static IPopulationManager GetPopulationManager(SiteContext sc) {
            string gaName = sc.Properties[GAName];
            string pmType = Factory.GetConfigNode("geneticalgorithms")[gaName].Attributes["populationManagerType"].Value;
            Type pmt = Type.GetType(pmType);
            return (IPopulationManager)Activator.CreateInstance(pmt, gaName);
        }
    }
}
