using System;
using System.Collections.Generic;
using YamlDotNet.RepresentationModel;
using static EVE_All_API.YamlUtils;

namespace EVE_All_API.StaticData
{
    public class DgmTypeAttribute : YamlSequencePage<DgmTypeAttribute>
    {
        private static Dictionary<int, List<DgmTypeAttribute>> dgmTypeAttributes = new Dictionary<int, List<DgmTypeAttribute>>();
        public static List<DgmTypeAttribute> GetDgmTypeAttribute(int _typeID)
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

        public DgmTypeAttribute(YamlNode node)
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
            if (!dgmTypeAttributes.ContainsKey(typeID))
            {
                dgmTypeAttributes[typeID] = new List<DgmTypeAttribute>();
            }
            dgmTypeAttributes[typeID].Add(this);
        }

    }
}
