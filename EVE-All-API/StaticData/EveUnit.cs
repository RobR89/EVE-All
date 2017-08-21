using System;
using System.Collections.Generic;
using YamlDotNet.RepresentationModel;
using static EVE_All_API.YamlUtils;

namespace EVE_All_API.StaticData
{
    public class EveUnit : YamlSequencePage<EveUnit>
    {
        private static Dictionary<int, EveUnit> units = new Dictionary<int, EveUnit>();
        public static EveUnit GetUnit(int _unitID)
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

        public EveUnit(YamlNode node)
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
            units[unitID] = this;
        }

    }
}
