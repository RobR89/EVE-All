using System;
using System.Collections.Generic;
using YamlDotNet.RepresentationModel;
using static EVE_All_API.YamlUtils;

namespace EVE_All_API.StaticData
{
    public class InvNames : YamlSequencePage<InvNames>
    {
        private static Dictionary<long, string> names = new Dictionary<long, string>();
        public static string GetName(int _nameID)
        {
            if (names.ContainsKey(_nameID))
            {
                return names[_nameID];
            }
            return null;
        }

        public InvNames(YamlNode node)
        {
            long itemID = 0;
            string itemName = null;
            YamlMappingNode mapping = (YamlMappingNode)node;
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

    }
}
