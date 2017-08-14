using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.RepresentationModel;

namespace EVE_All_API.StaticData
{
    public class DgmAttributeCategory
    {
        private static Dictionary<int, DgmAttributeCategory> dgmAttributeCategories = new Dictionary<int, DgmAttributeCategory>();
        public static DgmAttributeCategory getDgmAttributeCategory(int _categoryID)
        {
            if (dgmAttributeCategories.ContainsKey(_categoryID))
            {
                return dgmAttributeCategories[_categoryID];
            }
            return null;
        }

        public readonly int categoryID;
        public readonly string categoryName;
        public readonly string categoryDescription;

        private DgmAttributeCategory(YamlNode node)
        {
            YamlMappingNode mapping = (YamlMappingNode)node;
            foreach (var entry in mapping.Children)
            {
                string paramName = entry.Key.ToString();
                switch (paramName)
                {
                    case "categoryID":
                        categoryID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "categoryName":
                        categoryName = entry.Value.ToString();
                        break;
                    case "categoryDescription":
                        categoryDescription = entry.Value.ToString();
                        break;
                    default:
                        System.Diagnostics.Debug.WriteLine("DgmAttributeCategory unknown value:" + entry.Key + " = " + entry.Value);
                        break;
                }
            }
        }

        public static bool loadYAML(YamlStream yaml)
        {
            if (yaml == null)
            {
                return false;
            }
            YamlSequenceNode seq = (YamlSequenceNode)yaml.Documents[0].RootNode;
            foreach (var entry in seq.Children)
            {
                DgmAttributeCategory cat = new DgmAttributeCategory(entry);
                dgmAttributeCategories[cat.categoryID] = cat;
            }
            return true;
        }

    }
}
