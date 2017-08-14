using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.RepresentationModel;

namespace EVE_All_API.StaticData
{
    public class InvMarketGroup
    {
        private static Dictionary<int, InvMarketGroup> marketGroups = new Dictionary<int, InvMarketGroup>();
        public static InvMarketGroup getMarketGroup(int _marketGroupID)
        {
            if (marketGroups.ContainsKey(_marketGroupID))
            {
                return marketGroups[_marketGroupID];
            }
            return null;
        }
        public static List<InvMarketGroup> getRootGroups()
        {
            return getGroupChildren(0);
        }

        public static List<InvMarketGroup> getGroupChildren(int _marketGroupID)
        {
            List<InvMarketGroup> foundGroups = new List<InvMarketGroup>();
            foreach (InvMarketGroup group in marketGroups.Values)
            {
                if (group.parentGroupID == _marketGroupID)
                {
                    foundGroups.Add(group);
                }
            }
            return foundGroups;
        }

        public readonly int marketGroupID;
        public readonly string marketGroupName;
        public readonly string description;
        public readonly bool hasTypes;
        public readonly int iconID;
        public readonly int parentGroupID;

        private InvMarketGroup(YamlNode node)
        {
            YamlMappingNode mapping = (YamlMappingNode)node;
            foreach (var entry in mapping.Children)
            {
                string paramName = entry.Key.ToString();
                switch (paramName)
                {
                    case "marketGroupName":
                        marketGroupName = entry.Value.ToString();
                        break;
                    case "marketGroupID":
                        marketGroupID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "description":
                        description = entry.Value.ToString();
                        break;
                    case "hasTypes":
                        hasTypes = bool.Parse(entry.Value.ToString());
                        break;
                    case "iconID":
                        iconID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "parentGroupID":
                        parentGroupID = Int32.Parse(entry.Value.ToString());
                        break;
                    default:
                        System.Diagnostics.Debug.WriteLine("InvMarketGroup unknown value:" + entry.Key + " = " + entry.Value);
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
            YamlSequenceNode mapping = (YamlSequenceNode)yaml.Documents[0].RootNode;
            foreach (YamlNode entry in mapping.Children)
            {
                InvMarketGroup group = new InvMarketGroup(entry);
                marketGroups[group.marketGroupID] = group;
            }
            return true;
        }

    }
}
