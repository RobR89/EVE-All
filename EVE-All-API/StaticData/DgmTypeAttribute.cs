using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.RepresentationModel;

namespace EVE_All_API.StaticData
{
    public class DgmTypeAttribute
    {
        private static Dictionary<int, List<DgmTypeAttribute>> dgmTypeAttributes = new Dictionary<int, List<DgmTypeAttribute>>();
        public static List<DgmTypeAttribute> getDgmTypeAttribute(int _typeID)
        {
            if (dgmTypeAttributes.ContainsKey(_typeID))
            {
                return dgmTypeAttributes[_typeID];
            }
            return null;
        }

        public readonly int attributeID;
        public readonly int typeID;
        public readonly long valueInt;
        public readonly double valueFloat;
        public readonly bool isInt;

        private DgmTypeAttribute(YamlNode node)
        {
            YamlMappingNode mapping = (YamlMappingNode)node;
            foreach (var entry in mapping.Children)
            {
                string paramName = entry.Key.ToString();
                switch (paramName)
                {
                    case "attributeID":
                        attributeID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "typeID":
                        typeID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "valueInt":
                        valueInt = long.Parse(entry.Value.ToString());
                        isInt = true;
                        break;
                    case "valueFloat":
                        valueFloat = double.Parse(entry.Value.ToString());
                        isInt = false;
                        break;
                    default:
                        System.Diagnostics.Debug.WriteLine("DgmTypeAttribute unknown value:" + entry.Key + " = " + entry.Value);
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
            YamlSequenceNode seq = (YamlSequenceNode)yaml.Documents[0].RootNode;
            foreach (var entry in seq.Children)
            {
                DgmTypeAttribute gate = new DgmTypeAttribute(entry);
                if(!dgmTypeAttributes.ContainsKey(gate.typeID))
                {
                    dgmTypeAttributes[gate.typeID] = new List<DgmTypeAttribute>();
                }
                dgmTypeAttributes[gate.typeID].Add(gate);
            }
            return true;
        }

    }
}
