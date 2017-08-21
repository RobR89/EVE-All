using System;
using System.Collections.Generic;
using YamlDotNet.RepresentationModel;
using static EVE_All_API.YamlUtils;

namespace EVE_All_API.StaticData
{
    public class ChrFaction : YamlSequencePage<ChrFaction>
    {
        private static Dictionary<int, ChrFaction> factions = new Dictionary<int, ChrFaction>();
        public static ChrFaction GetFaction(int _factionID)
        {
            if (factions.ContainsKey(_factionID))
            {
                return factions[_factionID];
            }
            return null;
        }

        public readonly int factionID;
        public readonly string factionName;
        public readonly string description;
        public readonly int iconID;
        public readonly int corporationID;
        public readonly int militiaCorporationID;
        public readonly int raceIDs;
        public readonly double sizeFactor;
        public readonly int solarSystemID;
        public readonly int stationCount;
        public readonly int stationSystemCount;

        public ChrFaction(YamlNode node)
        {
            YamlMappingNode mapping = (YamlMappingNode)node;
            foreach (var entry in mapping.Children)
            {
                string paramName = entry.Key.ToString();
                switch (paramName)
                {
                    case "factionID":
                        factionID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "factionName":
                        factionName = entry.Value.ToString();
                        break;
                    case "description":
                        description = entry.Value.ToString();
                        break;
                    case "iconID":
                        iconID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "corporationID":
                        corporationID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "militiaCorporationID":
                        militiaCorporationID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "raceIDs":
                        raceIDs = Int32.Parse(entry.Value.ToString());
                        break;
                    case "sizeFactor":
                        sizeFactor = Double.Parse(entry.Value.ToString());
                        break;
                    case "solarSystemID":
                        solarSystemID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "stationCount":
                        stationCount = Int32.Parse(entry.Value.ToString());
                        break;
                    case "stationSystemCount":
                        stationSystemCount = Int32.Parse(entry.Value.ToString());
                        break;
                    default:
                        System.Diagnostics.Debug.WriteLine("ChrFaction unknown value:" + entry.Key + " = " + entry.Value);
                        break;
                }
            }
            factions[factionID] = this;
        }

    }
}
