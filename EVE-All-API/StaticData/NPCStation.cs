using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.RepresentationModel;

namespace EVE_All_API.StaticData
{
    public class NPCStation
    {
        #region caching
        public static void SaveAll(BinaryWriter save)
        {
            lock (stations)
            {
                save.Write(stations.Count);
                foreach (NPCStation station in stations.Values)
                {
                    station.Save(save);
                }
            }
        }

        public static bool LoadAll(BinaryReader load)
        {
            lock (stations)
            {
                int count = load.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    NPCStation station = new NPCStation(load);
                    stations[station.stationID] = station;
                }
            }
            return true;
        }

        public void Save(BinaryWriter save)
        {
            save.Write(stationID);
            save.Write(solarSystemID);
            save.Write(graphicID);
            save.Write(isConquerable);
            save.Write(operationID);
            save.Write(ownerID);
            position.Save(save);
            save.Write(reprocessingEfficiency);
            save.Write(reprocessingHangarFlag);
            save.Write(reprocessingStationsTake);
            save.Write(typeID);
            save.Write(useOperationName);
        }

        private NPCStation(BinaryReader load)
        {
            stationID = load.ReadInt64();
            solarSystemID = load.ReadInt32();
            graphicID = load.ReadInt32();
            isConquerable = load.ReadBoolean();
            operationID = load.ReadInt32();
            ownerID = load.ReadInt32();
            position = new Location(load);
            reprocessingEfficiency = load.ReadDouble();
            reprocessingHangarFlag = load.ReadInt32();
            reprocessingStationsTake = load.ReadDouble();
            typeID = load.ReadInt32();
            useOperationName = load.ReadBoolean();
            lock (stations)
            {
                stations[stationID] = this;
            }
        }
        #endregion caching

        private static Dictionary<long, NPCStation> stations = new Dictionary<long, NPCStation>();
        public static NPCStation GetNPCStation(long _stationID)
        {
            lock (stations)
            {
                if (stations.ContainsKey(_stationID))
                {
                    return stations[_stationID];
                }
            }
            return null;
        }

        public readonly long stationID;
        public readonly int solarSystemID;
        public readonly int graphicID;
        public readonly bool isConquerable;
        public readonly int operationID;
        public readonly int ownerID;
        public readonly Location position;
        public readonly double reprocessingEfficiency;
        public readonly int reprocessingHangarFlag;
        public readonly double reprocessingStationsTake;
        public readonly int typeID;
        public readonly bool useOperationName;

        private NPCStation(YamlNode node, long _stationID, int _solarSystemID)
        {
            stationID = _stationID;
            solarSystemID = _solarSystemID;
            YamlMappingNode mapping = (YamlMappingNode)node;
            foreach (var entry in mapping.Children)
            {
                string paramName = entry.Key.ToString();
                switch (paramName)
                {
                    case "graphicID":
                        graphicID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "operationID":
                        operationID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "ownerID":
                        ownerID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "isConquerable":
                        isConquerable = Boolean.Parse(entry.Value.ToString());
                        break;
                    case "position":
                        position = Location.ParseLocation(entry.Value);
                        break;
                    case "reprocessingEfficiency":
                        reprocessingEfficiency = Double.Parse(entry.Value.ToString());
                        break;
                    case "reprocessingHangarFlag":
                        reprocessingHangarFlag = Int32.Parse(entry.Value.ToString());
                        break;
                    case "reprocessingStationsTake":
                        reprocessingStationsTake = Double.Parse(entry.Value.ToString());
                        break;
                    case "typeID":
                        typeID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "useOperationName":
                        useOperationName = Boolean.Parse(entry.Value.ToString());
                        break;
                    default:
                        System.Diagnostics.Debug.WriteLine("NPCStation unknown value:" + entry.Key + " = " + entry.Value);
                        break;
                }
            }
        }

        public static List<long> LoadYAML(YamlNode yaml, int _solarSystemID)
        {
            if (yaml == null)
            {
                return null;
            }
            List<long> stations = new List<long>();
            YamlMappingNode mapping = (YamlMappingNode)yaml;
            foreach (var entry in mapping.Children)
            {
                long _stationID = Int64.Parse(entry.Key.ToString());
                NPCStation station = new NPCStation(entry.Value, _stationID, _solarSystemID);
                NPCStation.stations[station.stationID] = station;
                stations.Add(_stationID);
            }
            return stations;
        }

    }
}
