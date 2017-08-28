using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.RepresentationModel;
using static EVE_All_API.YamlUtils;

namespace EVE_All_API.StaticData
{
    public class InvNames : YamlSequencePage<InvNames>
    {
        #region caching
        public static void SaveAll(BinaryWriter save)
        {
            lock (names)
            {
                save.Write(names.Count);
                foreach (var name in names)
                {
                    save.Write(name.Key);
                    save.Write(name.Value);
                }
            }
        }

        public static bool LoadAll(BinaryReader load)
        {
            lock (names)
            {
                int count = load.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    long key = load.ReadInt64();
                    names[key] = load.ReadString();
                }
            }
            return true;
        }
        #endregion caching

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
