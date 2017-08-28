using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.RepresentationModel;
using static EVE_All_API.YamlUtils;

namespace EVE_All_API.StaticData
{
    public class CrpNPCCorporation : YamlSequencePage<CrpNPCCorporation>
    {
        #region caching
        public static void SaveAll(BinaryWriter save)
        {
            lock (corporations)
            {
                Loader.SaveDict<CrpNPCCorporation>(corporations, save, Save);
            }
        }

        public static bool LoadAll(BinaryReader load)
        {
            lock (corporations)
            {
                corporations = Loader.LoadDict<CrpNPCCorporation>(load, Load);
            }
            return true;
        }

        public static void Save(CrpNPCCorporation attrib, BinaryWriter save)
        {
            attrib.Save(save);
        }

        public static CrpNPCCorporation Load(BinaryReader load)
        {
            return new CrpNPCCorporation(load);
        }

        public void Save(BinaryWriter save)
        {
            save.Write(corporationID);
            Loader.Save(corporationName, save);
            Loader.Save(description, save);
            save.Write(iconID);
            save.Write(border);
            save.Write(corridor);
            save.Write(enemyID);
            Loader.Save(extent, save);
            save.Write(factionID);
            save.Write(friendID);
            save.Write(fringe);
            save.Write(hub);
            save.Write(initialPrice);
            save.Write(investorID1);
            save.Write(investorID2);
            save.Write(investorID3);
            save.Write(investorID4);
            save.Write(investorShares1);
            save.Write(investorShares2);
            save.Write(investorShares3);
            save.Write(investorShares4);
            save.Write(minSecurity);
            save.Write(publicShares);
            save.Write(scattered);
            Loader.Save(size, save);
            save.Write(sizeFactor);
            save.Write(solarSystemID);
            save.Write(stationCount);
            save.Write(stationSystemCount);
        }

        private CrpNPCCorporation(BinaryReader load)
        {
            corporationID = load.ReadInt32();
            Loader.Load(out corporationName, load);
            Loader.Load(out description, load);
            iconID = load.ReadInt32();
            border = load.ReadInt32();
            corridor = load.ReadInt32();
            enemyID = load.ReadInt32();
            Loader.Load(out extent, load);
            factionID = load.ReadInt32();
            friendID = load.ReadInt32();
            fringe = load.ReadInt32();
            hub = load.ReadInt32();
            initialPrice = load.ReadInt32();
            investorID1 = load.ReadInt32();
            investorID2 = load.ReadInt32();
            investorID3 = load.ReadInt32();
            investorID4 = load.ReadInt32();
            investorShares1 = load.ReadInt32();
            investorShares2 = load.ReadInt32();
            investorShares3 = load.ReadInt32();
            investorShares4 = load.ReadInt32();
            minSecurity = load.ReadDouble();
            publicShares = load.ReadInt32();
            scattered = load.ReadBoolean();
            Loader.Load(out size, load);
            sizeFactor = load.ReadDouble();
            solarSystemID = load.ReadInt32();
            stationCount = load.ReadInt32();
            stationSystemCount = load.ReadInt32();
        }
        #endregion caching

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
