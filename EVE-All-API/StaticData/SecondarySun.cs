using System;
using System.IO;
using YamlDotNet.RepresentationModel;

namespace EVE_All_API.StaticData
{
    public class SecondarySun
    {
        #region caching
        public void Save(BinaryWriter save)
        {
            save.Write(solarSystemID);
            save.Write(itemID);
            save.Write(effectBeaconTypeID);
            save.Write(typeID);
            position.Save(save);
        }

        public SecondarySun(BinaryReader load)
        {
            solarSystemID = load.ReadInt32();
            itemID = load.ReadInt32();
            effectBeaconTypeID = load.ReadInt32();
            typeID = load.ReadInt32();
            position = new Location(load);
        }
        #endregion caching

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
