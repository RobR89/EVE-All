using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.RepresentationModel;

namespace EVE_All_API.StaticData
{
    public class NPCStation
    {
        private static Dictionary<long, NPCStation> stations = new Dictionary<long, NPCStation>();
        public static NPCStation getUnit(long _stationID)
        {
            lock(stations)
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
                        position = Location.parseLocation(entry.Value);
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

        public static List<long> loadYAML(YamlNode yaml, int _solarSystemID)
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
