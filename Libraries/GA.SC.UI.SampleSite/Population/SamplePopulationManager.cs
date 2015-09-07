using GA.Nucleus;
using GA.Nucleus.Chromosome;
using GA.Nucleus.Gene;
using GA.Nucleus.Population;
using GA.SC.UI.SampleSite.Gene;
using Sitecore.Configuration;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace GA.SC.UI.SampleSite.Population {
    public class SamplePopulationManager : BasePopulationManager {

        public string GAName = string.Empty;
        public XmlNode GARoot = Factory.GetConfigNode("geneticalgorithms");
        protected static readonly string PopKey = "population";

        public SamplePopulationManager(string gaName){

            GAName = gaName;
            CrossoverRatio = float.Parse(GetGAAttribute("crossoverRatio"));
            ElitismRatio = float.Parse(GetGAAttribute("elitismRatio"));
            FitnessRatio = float.Parse(GetGAAttribute("fitnessRatio"));
            FitnessThreshold = float.Parse(GetGAAttribute("fitnessThreshold"));
            FitnessSort = (FitnessSortType)Enum.Parse(typeof(FitnessSortType), GetGAAttribute("fitnessSort"));
            KaryotypeType = Type.GetType(GetGAAttribute("karyotypeType"));
            MutationRatio = float.Parse(GetGAAttribute("mutationRatio"));
            PopSize = int.Parse(GetGAAttribute("popSize"));
            PopulationType = Type.GetType(GetGAAttribute("populationType"));
            TourneySize = int.Parse(GetGAAttribute("tourneySize"));
        }

        private ChromosomePool _ChromosomePool;
        public override ChromosomePool ChromosomePool {
            get {
                if (_ChromosomePool != null)
                    return _ChromosomePool;

                _ChromosomePool = new ChromosomePool();
                // get tags - TODO change with content search or api call
                Item tagBucket = GetItemFromID(Statics.TagFolder);
                List<Item> tags = Sitecore.Context.Database.SelectItems(string.Format("{0}//*", tagBucket.Paths.FullPath)).ToList();

                GenePool g = new GenePool();
                //number of genes corresponds to the number of placeholders to fill with display content
                g.GeneLimit = 8;
                for (int z = 0; z < tags.Count; z++) { //add all the tags to the genotype
                    TagGene t = new TagGene(tags[z].DisplayName, tags[z].ID.ToString(), RandomUtil.NextBool());
                    g.Add(t);
                }
                if (!ChromosomePool.ContainsKey(Statics.ChromoKey))
                    ChromosomePool.Add(Statics.ChromoKey, g);

                return _ChromosomePool;
            }
            set {
                _ChromosomePool = value;
            }
        }

        #region Methods

        public override IPopulation GetPopulation() {
            if (HttpContext.Current.Session[PopKey] != null)
                return (IPopulation)HttpContext.Current.Session[PopKey];

            IPopulation p = CreatePopulation();
            HttpContext.Current.Session[PopKey] = p;
            return p;
        }

        public override void SetPopulation(IPopulation p) {
            HttpContext.Current.Session[PopKey] = p;
        }

        public string GetGAAttribute(string prop) {
            return GARoot[GAName].Attributes[prop].Value;
        }

        protected Item GetItemFromID(string idStr) {
            if (!Sitecore.Data.ID.IsID(idStr))
                return null;
            Sitecore.Data.ID id = Sitecore.Data.ID.Parse(idStr);
            return Sitecore.Context.Database.GetItem(id);
        }

        #endregion Methods
    }
}
