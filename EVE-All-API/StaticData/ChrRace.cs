using System;
using System.Collections.Generic;
using YamlDotNet.RepresentationModel;
using static EVE_All_API.YamlUtils;

namespace EVE_All_API.StaticData
{
    public class ChrRace : YamlSequencePage<ChrRace>
    {
        private static Dictionary<int, ChrRace> races = new Dictionary<int, ChrRace>();
        public static ChrRace GetRace(int _raceID)
        {
            if (races.ContainsKey(_raceID))
            {
                return races[_raceID];
            }
            return null;
        }

        public readonly int raceID;
        public readonly string raceName;
        public readonly string description;
        public readonly string shortDescription;
        public readonly int iconID;

        public ChrRace(YamlNode node)
        {
            YamlMappingNode mapping = (YamlMappingNode)node;
            foreach (var entry in mapping.Children)
            {
                string paramName = entry.Key.ToString();
                switch (paramName)
                {
                    case "raceID":
                        raceID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "raceName":
                        raceName = entry.Value.ToString();
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
                    default:
                        System.Diagnostics.Debug.WriteLine("ChrRace unknown value:" + entry.Key + " = " + entry.Value);
                        break;
                }
            }
            races[raceID] = this;
        }

    }
}
