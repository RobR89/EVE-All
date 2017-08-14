using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.RepresentationModel;

namespace EVE_All_API.StaticData
{
    public class InvCategory
    {
        private static Dictionary<int, InvCategory> categories = new Dictionary<int, InvCategory>();
        public static InvCategory getCategory(int _categoryID)
        {
            if (categories.ContainsKey(_categoryID))
            {
                return categories[_categoryID];
            }
            return null;
        }

        public readonly int categoryID;
        public readonly string name;
        public readonly int iconID;
        public readonly bool published;

        private InvCategory(int _categoryID, YamlNode node)
        {
            categoryID = _categoryID;
            YamlMappingNode mapping = (YamlMappingNode)node;
            foreach (var entry in mapping.Children)
            {
                string paramName = entry.Key.ToString();
                switch (paramName)
                {
                    case "name":
                        name = YamlUtils.getLanguageString(YamlUtils.getLanguageStrings(entry.Value), UserData.language);
                        break;
                    case "iconID":
                        iconID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "published":
                        published = Boolean.Parse(entry.Value.ToString());
                        break;
                    default:
                        System.Diagnostics.Debug.WriteLine("InvCategory unknown value:" + entry.Key + " = " + entry.Value);
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
            YamlMappingNode mapping = (YamlMappingNode)yaml.Documents[0].RootNode;
            foreach (var entry in mapping.Children)
            {
                int categoryID = Int32.Parse(entry.Key.ToString());
                InvCategory category = new InvCategory(categoryID, entry.Value);
                categories[categoryID] = category;
            }
            return true;
        }

    }
}
