using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.RepresentationModel;

namespace EVE_All_API.StaticData
{
    public class InvTypeMaterial
    {
        private static Dictionary<int, List<InvTypeMaterial>> typeMaterials = new Dictionary<int, List<InvTypeMaterial>>();
        public static List<InvTypeMaterial> getMarketGroup(int _typeID)
        {
            if (typeMaterials.ContainsKey(_typeID))
            {
                return typeMaterials[_typeID];
            }
            return null;
        }

        public readonly int typeID;
        public readonly int quantity;
        public readonly int materialTypeID;

        private InvTypeMaterial(YamlNode node)
        {
            YamlMappingNode mapping = (YamlMappingNode)node;
            foreach (var entry in mapping.Children)
            {
                string paramName = entry.Key.ToString();
                switch (paramName)
                {
                    case "typeID":
                        typeID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "quantity":
                        quantity = Int32.Parse(entry.Value.ToString());
                        break;
                    case "materialTypeID":
                        materialTypeID = Int32.Parse(entry.Value.ToString());
                        break;
                    default:
                        System.Diagnostics.Debug.WriteLine("InvTypeMaterial unknown value:" + entry.Key + " = " + entry.Value);
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
                InvTypeMaterial material = new InvTypeMaterial(entry);
                if(!typeMaterials.ContainsKey(material.typeID))
                {
                    typeMaterials[material.typeID] = new List<InvTypeMaterial>();
                }
                typeMaterials[material.typeID].Add(material);
            }
            return true;
        }

    }
}
