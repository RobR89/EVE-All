using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.RepresentationModel;
using static EVE_All_API.YamlUtils;

namespace EVE_All_API.StaticData
{
    public class ChrFaction : YamlSequencePage<ChrFaction>
    {
        #region caching
        public static void SaveAll(BinaryWriter save)
        {
            lock (factions)
            {
                Loader.SaveDict<ChrFaction>(factions, save, Save);
            }
        }

        public static bool LoadAll(BinaryReader load)
        {
            lock (factions)
            {
                factions = Loader.LoadDict<ChrFaction>(load, Load);
            }
            return true;
        }

        public static void Save(ChrFaction attrib, BinaryWriter save)
        {
            attrib.Save(save);
        }

        public static ChrFaction Load(BinaryReader load)
        {
            return new ChrFaction(load);
        }

        public void Save(BinaryWriter save)
        {
            save.Write(factionID);
            Loader.Save(factionName, save);
            Loader.Save(description, save);
            save.Write(iconID);
            save.Write(corporationID);
            save.Write(militiaCorporationID);
            save.Write(raceIDs);
            save.Write(sizeFactor);
            save.Write(solarSystemID);
            save.Write(stationCount);
            save.Write(stationSystemCount);
        }

        private ChrFaction(BinaryReader load)
        {
            factionID = load.ReadInt32();
            Loader.Load(out factionName, load);
            Loader.Load(out description, load);
            iconID = load.ReadInt32();
            corporationID = load.ReadInt32();
            militiaCorporationID = load.ReadInt32();
            raceIDs = load.ReadInt32();
            sizeFactor = load.ReadDouble();
            solarSystemID = load.ReadInt32();
            stationCount = load.ReadInt32();
            stationSystemCount = load.ReadInt32();
        }
        #endregion caching

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
