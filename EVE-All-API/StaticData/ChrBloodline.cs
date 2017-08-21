using System;
using System.Collections.Generic;
using YamlDotNet.RepresentationModel;
using static EVE_All_API.YamlUtils;

namespace EVE_All_API.StaticData
{
    public class ChrBloodline : YamlSequencePage<ChrBloodline>
    {
        private static Dictionary<int, ChrBloodline> bloodlines = new Dictionary<int, ChrBloodline>();
        public static ChrBloodline GetBloodline(int _bloodlineID)
        {
            if (bloodlines.ContainsKey(_bloodlineID))
            {
                return bloodlines[_bloodlineID];
            }
            return null;
        }

        public readonly int bloodlineID;
        public readonly string bloodlineName;
        public readonly string description;
        public readonly string femaleDescription;
        public readonly string maleDescription;
        public readonly string shortDescription;
        public readonly string shortFemaleDescription;
        public readonly string shortMaleDescription;
        public readonly int iconID;
        public readonly int shipTypeID;
        public readonly long corporationID;
        public readonly int raceID;
        public readonly int charisma;
        public readonly int intelligence;
        public readonly int memory;
        public readonly int perception;
        public readonly int willpower;

        public ChrBloodline(YamlNode node)
        {
            YamlMappingNode mapping = (YamlMappingNode)node;
            foreach (var entry in mapping.Children)
            {
                string paramName = entry.Key.ToString();
                switch (paramName)
                {
                    case "bloodlineID":
                        bloodlineID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "bloodlineName":
                        bloodlineName = entry.Value.ToString();
                        break;
                    case "description":
                        description = entry.Value.ToString();
                        break;
                    case "femaleDescription":
                        femaleDescription = entry.Value.ToString();
                        break;
                    case "maleDescription":
                        maleDescription = entry.Value.ToString();
                        break;
                    case "shortDescription":
                        shortDescription = entry.Value.ToString();
                        break;
                    case "shortFemaleDescription":
                        shortFemaleDescription = entry.Value.ToString();
                        break;
                    case "shortMaleDescription":
                        shortMaleDescription = entry.Value.ToString();
                        break;
                    case "iconID":
                        iconID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "shipTypeID":
                        shipTypeID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "corporationID":
                        corporationID = long.Parse(entry.Value.ToString());
                        break;
                    case "raceID":
                        raceID = Int32.Parse(entry.Value.ToString());
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
                        System.Diagnostics.Debug.WriteLine("ChrBloodline unknown value:" + entry.Key + " = " + entry.Value);
                        break;
                }
            }
            bloodlines[bloodlineID] = this;
        }

    }
}
