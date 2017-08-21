using System;
using System.Collections.Generic;
using YamlDotNet.RepresentationModel;
using static EVE_All_API.YamlUtils;

namespace EVE_All_API.StaticData
{
    public class DgmAttributeCategory : YamlSequencePage<DgmAttributeCategory>
    {
        private static Dictionary<int, DgmAttributeCategory> dgmAttributeCategories = new Dictionary<int, DgmAttributeCategory>();
        public static DgmAttributeCategory GetDgmAttributeCategory(int _categoryID)
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

        public DgmAttributeCategory(YamlNode node)
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
            dgmAttributeCategories[categoryID] = this;
        }

    }
}
