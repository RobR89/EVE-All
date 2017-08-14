using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.RepresentationModel;

namespace EVE_All_API.StaticData
{
    public class EveUnit
    {
        private static Dictionary<int, EveUnit> units = new Dictionary<int, EveUnit>();
        public static EveUnit getUnit(int _unitID)
        {
            if (units.ContainsKey(_unitID))
            {
                return units[_unitID];
            }
            return null;
        }

        public readonly int unitID;
        public readonly string unitName;
        public readonly string displayName;
        public readonly string description;

        private EveUnit(YamlNode node)
        {
            YamlMappingNode mapping = (YamlMappingNode)node;
            foreach (var entry in mapping.Children)
            {
                string paramName = entry.Key.ToString();
                switch (paramName)
                {
                    case "unitName":
                        unitName = entry.Value.ToString();
                        break;
                    case "unitID":
                        unitID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "displayName":
                        displayName = entry.Value.ToString();
                        break;
                    case "description":
                        description = entry.Value.ToString();
                        break;
                    default:
                        System.Diagnostics.Debug.WriteLine("EveUnit unknown value:" + entry.Key + " = " + entry.Value);
                        break;
                }
            }
        }

        public static bool loadYAML(YamlStream yaml)
        {
            if (yaml == null)
            {
                return false;
            }
            YamlSequenceNode mapping = (YamlSequenceNode)yaml.Documents[0].RootNode;
            foreach (YamlNode entry in mapping.Children)
            {
                EveUnit unit = new EveUnit(entry);
                units[unit.unitID] = unit;
            }
            return true;
        }

    }
}
