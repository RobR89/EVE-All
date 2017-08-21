using System;
using System.Collections.Generic;
using YamlDotNet.RepresentationModel;
using static EVE_All_API.YamlUtils;

namespace EVE_All_API.StaticData
{
    public class InvMarketGroup : YamlSequencePage<InvMarketGroup>
    {
        private static Dictionary<int, InvMarketGroup> marketGroups = new Dictionary<int, InvMarketGroup>();
        public static InvMarketGroup GetMarketGroup(int _marketGroupID)
        {
            if (marketGroups.ContainsKey(_marketGroupID))
            {
                return marketGroups[_marketGroupID];
            }
            return null;
        }
        public static List<InvMarketGroup> GetRootGroups()
        {
            return GetGroupChildren(0);
        }

        public static List<InvMarketGroup> GetGroupChildren(int _marketGroupID)
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

        public InvMarketGroup(YamlNode node)
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
            marketGroups[marketGroupID] = this;
        }

    }
}
