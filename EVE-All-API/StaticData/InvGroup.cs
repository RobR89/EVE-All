using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.RepresentationModel;

namespace EVE_All_API.StaticData
{
    public class InvGroup
    {
        private static Dictionary<int, InvGroup> groups = new Dictionary<int, InvGroup>();
        public static InvGroup getGroup(int _groupID)
        {
            if (groups.ContainsKey(_groupID))
            {
                return groups[_groupID];
            }
            return null;
        }

        public readonly int groupID;
        public readonly int categoryID;
        public readonly string name;
        public readonly int iconID;
        public readonly bool published;
        public readonly bool anchorable;
        public readonly bool anchored;
        public readonly bool fittableNonSingleton;
        public readonly bool useBasePrice;

        private InvGroup(int _groupID, YamlNode node)
        {
            groupID = _groupID;
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
                    case "categoryID":
                        categoryID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "published":
                        published = Boolean.Parse(entry.Value.ToString());
                        break;
                    case "anchorable":
                        anchorable = Boolean.Parse(entry.Value.ToString());
                        break;
                    case "anchored":
                        anchored = Boolean.Parse(entry.Value.ToString());
                        break;
                    case "fittableNonSingleton":
                        fittableNonSingleton = Boolean.Parse(entry.Value.ToString());
                        break;
                    case "useBasePrice":
                        useBasePrice = Boolean.Parse(entry.Value.ToString());
                        break;
                    default:
                        System.Diagnostics.Debug.WriteLine("InvGroup unknown value:" + entry.Key + " = " + entry.Value);
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
                int groupID = Int32.Parse(entry.Key.ToString());
                InvGroup group = new InvGroup(groupID, entry.Value);
                groups[groupID] = group;
            }
            return true;
        }

    }
}
