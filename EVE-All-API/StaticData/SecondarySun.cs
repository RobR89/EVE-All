using System;
using YamlDotNet.RepresentationModel;

namespace EVE_All_API.StaticData
{
    public class SecondarySun
    {
        public readonly int solarSystemID;
        public readonly int itemID;
        public readonly int effectBeaconTypeID;
        public readonly int typeID;
        public readonly Location position;

        public SecondarySun(YamlNode node, int _solarSystemID)
        {
            solarSystemID = _solarSystemID;
            YamlMappingNode mapping = (YamlMappingNode)node;
            foreach (var entry in mapping.Children)
            {
                string paramName = entry.Key.ToString();
                switch (paramName)
                {
                    case "itemID":
                        itemID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "position":
                        position = Location.ParseLocation(entry.Value);
                        break;
                    case "typeID":
                        typeID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "effectBeaconTypeID":
                        effectBeaconTypeID = Int32.Parse(entry.Value.ToString());
                        break;
                    default:
                        System.Diagnostics.Debug.WriteLine("Star unknown value:" + entry.Key + " = " + entry.Value);
                        break;
                }
            }
        }

    }
}
