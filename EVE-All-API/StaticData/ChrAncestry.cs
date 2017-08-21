using System;
using System.Collections.Generic;
using YamlDotNet.RepresentationModel;
using static EVE_All_API.YamlUtils;

namespace EVE_All_API.StaticData
{
    public class ChrAncestry : YamlSequencePage<ChrAncestry>
    {
        private static Dictionary<int, ChrAncestry> ancestries = new Dictionary<int, ChrAncestry>();
        public static ChrAncestry GetAncestry(int _ancestryID)
        {
            if (ancestries.ContainsKey(_ancestryID))
            {
                return ancestries[_ancestryID];
            }
            return null;
        }

        public readonly int ancestryID;
        public readonly string ancestryName;
        public readonly int bloodlineID;
        public readonly string description;
        public readonly string shortDescription;
        public readonly int iconID;
        public readonly int charisma;
        public readonly int intelligence;
        public readonly int memory;
        public readonly int perception;
        public readonly int willpower;

        public ChrAncestry(YamlNode node)
        {
            YamlMappingNode mapping = (YamlMappingNode)node;
            foreach (var entry in mapping.Children)
            {
                string paramName = entry.Key.ToString();
                switch (paramName)
                {
                    case "ancestryID":
                        ancestryID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "ancestryName":
                        ancestryName = entry.Value.ToString();
                        break;
                    case "bloodlineID":
                        bloodlineID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "description":
                        description = entry.Value.ToString();
                        break;
                    case "shortDescription":
                        shortDescription = entry.Value.ToString();
                        break;
                    case "iconID":
                        iconID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "charisma":
                        charisma = Int32.Parse(entry.Value.ToString());
                        break;
                    case "intelligence":
                        intelligence = Int32.Parse(entry.Value.ToString());
                        break;
                    case "memory":
                        memory = Int32.Parse(entry.Value.ToString());
                        break;
                    case "perception":
                        perception = Int32.Parse(entry.Value.ToString());
                        break;
                    case "willpower":
                        willpower = Int32.Parse(entry.Value.ToString());
                        break;
                    default:
                        System.Diagnostics.Debug.WriteLine("ChrAncestry unknown value:" + entry.Key + " = " + entry.Value);
                        break;
                }
            }
            ancestries[ancestryID] = this;
        }

    }
}
