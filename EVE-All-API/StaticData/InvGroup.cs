using System;
using System.Collections.Generic;
using YamlDotNet.RepresentationModel;
using static EVE_All_API.YamlUtils;

namespace EVE_All_API.StaticData
{
    public class InvGroup : YamlMappingPage<InvGroup>
    {
        private static Dictionary<int, InvGroup> groups = new Dictionary<int, InvGroup>();
        public static InvGroup GetGroup(int _groupID)
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

        public InvGroup(YamlNode key, YamlNode node)
        {
            groupID = Int32.Parse(key.ToString());
            YamlMappingNode mapping = (YamlMappingNode)node;
            foreach (var entry in mapping.Children)
            {
                string paramName = entry.Key.ToString();
                switch (paramName)
                {
                    case "name":
                        name = YamlUtils.GetLanguageString(YamlUtils.GetLanguageStrings(entry.Value), UserData.language);
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
            groups[groupID] = this;
        }

    }
}
