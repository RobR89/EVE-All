using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.RepresentationModel;

namespace EVE_All_API.StaticData
{
    public class Stargate
    {
        private static Dictionary<int, Stargate> stargates = new Dictionary<int, Stargate>();
        public static Stargate getStargate(int _gateID)
        {
            lock(stargates)
            {
                if (stargates.ContainsKey(_gateID))
                {
                    return stargates[_gateID];
                }
            }
            return null;
        }

        public readonly int stargateID;
        public readonly int solarSystemID;
        public readonly int destination;
        public readonly int typeID;
        public readonly Location position;

        private Stargate(YamlNode node, int _stargateID, int _solarSystemID)
        {
            stargateID = _stargateID;
            solarSystemID = _solarSystemID;
            YamlMappingNode mapping = (YamlMappingNode)node;
            foreach (var entry in mapping.Children)
            {
                string paramName = entry.Key.ToString();
                switch (paramName)
                {
                    case "destination":
                        destination = Int32.Parse(entry.Value.ToString());
                        break;
                    case "typeID":
                        typeID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "position":
                        position = Location.parseLocation(entry.Value);
                        break;
                    default:
                        System.Diagnostics.Debug.WriteLine("Stargate unknown value:" + entry.Key + " = " + entry.Value);
                        break;
                }
            }
        }

        public static List<int> loadYAML(YamlNode yaml, int _solarSystemID)
        {
            if (yaml == null)
            {
                return null;
            }
            List<int> gates = new List<int>();
            YamlMappingNode mapping = (YamlMappingNode)yaml;
            foreach (var entry in mapping.Children)
            {
                int _stargateID = Int32.Parse(entry.Key.ToString());
                Stargate gate = new Stargate(entry.Value, _stargateID, _solarSystemID);
                lock (stargates)
                {
                    stargates[gate.stargateID] = gate;
                }
                gates.Add(_stargateID);
            }
            return gates;
        }

    }
}
