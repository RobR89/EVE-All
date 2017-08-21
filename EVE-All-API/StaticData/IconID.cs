using System;
using System.Collections.Generic;
using YamlDotNet.RepresentationModel;
using static EVE_All_API.YamlUtils;

namespace EVE_All_API.StaticData
{
    public class IconID : YamlMappingPage<IconID>
    {
        private static Dictionary<int, IconID> icons = new Dictionary<int, IconID>();
        public static IconID GetIconID(int _iconID)
        {
            if (icons.ContainsKey(_iconID))
            {
                return icons[_iconID];
            }
            return null;
        }

        public readonly int iconID;
        public readonly string description;
        public readonly string iconFile;

        public IconID(YamlNode key, YamlNode node)
        {
            iconID = Int32.Parse(key.ToString());
            YamlMappingNode mapping = (YamlMappingNode)node;
            foreach (var entry in mapping.Children)
            {
                string paramName = entry.Key.ToString();
                switch (paramName)
                {
                    case "description":
                        description = entry.Value.ToString();
                        break;
                    case "iconFile":
                        iconFile = entry.Value.ToString();
                        break;
                    default:
                        System.Diagnostics.Debug.WriteLine("IconID unknown value:" + entry.Key + " = " + entry.Value);
                        break;
                }
            }
            icons[iconID] = this;
        }

    }
}
