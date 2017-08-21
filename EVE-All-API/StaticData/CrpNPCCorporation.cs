using System;
using System.Collections.Generic;
using YamlDotNet.RepresentationModel;
using static EVE_All_API.YamlUtils;

namespace EVE_All_API.StaticData
{
    public class CrpNPCCorporation : YamlSequencePage<CrpNPCCorporation>
    {
        private static Dictionary<int, CrpNPCCorporation> corporations = new Dictionary<int, CrpNPCCorporation>();
        public static CrpNPCCorporation GetCorporation(int _corporationID)
        {
            if (corporations.ContainsKey(_corporationID))
            {
                return corporations[_corporationID];
            }
            return null;
        }

        public readonly int corporationID;
        public readonly string corporationName;
        public readonly string description;
        public readonly int iconID;
        public readonly int border;
        public readonly int corridor;
        public readonly int enemyID;
        public readonly string extent;
        public readonly int factionID;
        public readonly int friendID;
        public readonly int fringe;
        public readonly int hub;
        public readonly int initialPrice;
        public readonly int investorID1;
        public readonly int investorID2;
        public readonly int investorID3;
        public readonly int investorID4;
        public readonly int investorShares1;
        public readonly int investorShares2;
        public readonly int investorShares3;
        public readonly int investorShares4;
        public readonly double minSecurity;
        public readonly int publicShares;
        public readonly bool scattered;
        public readonly string size;
        public readonly double sizeFactor;
        public readonly int solarSystemID;
        public readonly int stationCount;
        public readonly int stationSystemCount;

        public CrpNPCCorporation(YamlNode node)
        {
            YamlMappingNode mapping = (YamlMappingNode)node;
            foreach (var entry in mapping.Children)
            {
                string paramName = entry.Key.ToString();
                switch (paramName)
                {
                    case "corporationID":
                        corporationID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "corporationName":
                        corporationName = entry.Value.ToString();
                        break;
                    case "description":
                        description = entry.Value.ToString();
                        break;
                    case "iconID":
                        iconID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "border":
                        border = Int32.Parse(entry.Value.ToString());
                        break;
                    case "corridor":
                        corridor = Int32.Parse(entry.Value.ToString());
                        break;
                    case "enemyID":
                        enemyID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "extent":
                        extent = entry.Value.ToString();
                        break;
                    case "factionID":
                        factionID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "friendID":
                        friendID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "fringe":
                        fringe = Int32.Parse(entry.Value.ToString());
                        break;
                    case "hub":
                        hub = Int32.Parse(entry.Value.ToString());
                        break;
                    case "initialPrice":
                        initialPrice = Int32.Parse(entry.Value.ToString());
                        break;
                    case "investorID1":
                        investorID1 = Int32.Parse(entry.Value.ToString());
                        break;
                    case "investorID2":
                        investorID2 = Int32.Parse(entry.Value.ToString());
                        break;
                    case "investorID3":
                        investorID3 = Int32.Parse(entry.Value.ToString());
                        break;
                    case "investorID4":
                        investorID4 = Int32.Parse(entry.Value.ToString());
                        break;
                    case "investorShares1":
                        investorShares1 = Int32.Parse(entry.Value.ToString());
                        break;
                    case "investorShares2":
                        investorShares2 = Int32.Parse(entry.Value.ToString());
                        break;
                    case "investorShares3":
                        investorShares3 = Int32.Parse(entry.Value.ToString());
                        break;
                    case "investorShares4":
                        investorShares4 = Int32.Parse(entry.Value.ToString());
                        break;
                    case "minSecurity":
                        minSecurity = Double.Parse(entry.Value.ToString());
                        break;
                    case "publicShares":
                        publicShares = Int32.Parse(entry.Value.ToString());
                        break;
                    case "scattered":
                        scattered = Boolean.Parse(entry.Value.ToString());
                        break;
                    case "size":
                        size = entry.Value.ToString();
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
                        System.Diagnostics.Debug.WriteLine("CrpNPCCorporation unknown value:" + entry.Key + " = " + entry.Value);
                        break;
                }
            }
            corporations[corporationID] = this;
        }

    }
}
