using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.RepresentationModel;

namespace EVE_All_API.StaticData
{
    public class IconID
    {
        private static Dictionary<int, IconID> icons = new Dictionary<int, IconID>();
        public static IconID getIconID(int _iconID)
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

        private IconID(int _iconID, YamlNode node)
        {
            iconID = _iconID;
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
        }

        public static bool loadYAML(YamlStream yaml)
        {
            if (yaml == null)
            {
                return false;
            }
            YamlMappingNode mapping = (YamlMappingNode)yaml.Documents[0].RootNode;
            foreach (var entry in mapping.Children)
            {
                int iconID = Int32.Parse(entry.Key.ToString());
                IconID icon = new IconID(iconID, entry.Value);
                icons[iconID] = icon;
            }
            return true;
        }

    }
}
