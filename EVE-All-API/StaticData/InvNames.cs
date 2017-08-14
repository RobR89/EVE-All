using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.RepresentationModel;

namespace EVE_All_API.StaticData
{
    public class InvNames
    {
        private static Dictionary<long, string> names = new Dictionary<long, string>();
        public static string getName(int _nameID)
        {
            if (names.ContainsKey(_nameID))
            {
                return names[_nameID];
            }
            return null;
        }

        public static bool loadYAML(YamlStream yaml)
        {
            if (yaml == null)
            {
                return false;
            }
            YamlSequenceNode sequence = (YamlSequenceNode)yaml.Documents[0].RootNode;
            foreach (YamlNode entry in sequence.Children)
            {
                long itemID = 0;
                string itemName = null;
                YamlMappingNode mapping = (YamlMappingNode)entry;
                foreach (var map in mapping.Children)
                {
                    string paramName = map.Key.ToString();
                    switch (paramName)
                    {
                        case "itemName":
                            itemName = map.Value.ToString();
                            break;
                        case "itemID":
                            itemID = Int64.Parse(map.Value.ToString());
                            break;
                        default:
                            System.Diagnostics.Debug.WriteLine("InvNames unknown value:" + map.Key + " = " + map.Value);
                            break;
                    }
                }
                names[itemID] = itemName;
            }
            return true;
        }

    }
}
