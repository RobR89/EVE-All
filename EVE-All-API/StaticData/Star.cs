using System;
using System.IO;
using YamlDotNet.RepresentationModel;

namespace EVE_All_API.StaticData
{
    public class Star
    {
        #region caching
        public void Save(BinaryWriter save)
        {
            save.Write(starID);
            save.Write(solarSystemID);
            save.Write(radius);
            save.Write(typeID);
            if (statistics == null)
            {
                save.Write(false);
            }
            else
            {
                save.Write(true);
                statistics.Save(save);
            }
        }

        public Star(BinaryReader load)
        {
            starID = load.ReadInt32();
            solarSystemID = load.ReadInt32();
            radius = load.ReadDouble();
            typeID = load.ReadInt32();
            if (load.ReadBoolean())
            {
                statistics = new StarStatistics(load);
            }
            else
            {
                statistics = null;
            }
        }
        #endregion caching

        public readonly int starID;
        public readonly int solarSystemID;
        public readonly double radius;
        public readonly int typeID;
        public readonly StarStatistics statistics;

        public Star(YamlNode node, int _solarSystemID)
        {
            solarSystemID = _solarSystemID;
            YamlMappingNode mapping = (YamlMappingNode)node;
            foreach (var entry in mapping.Children)
            {
                string paramName = entry.Key.ToString();
                switch (paramName)
                {
                    case "id":
                        starID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "radius":
                        radius = Double.Parse(entry.Value.ToString());
                        break;
                    case "typeID":
                        typeID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "statistics":
                        statistics = new StarStatistics(entry.Value);
                        break;
                    default:
                        System.Diagnostics.Debug.WriteLine("Star unknown value:" + entry.Key + " = " + entry.Value);
                        break;
                }
            }
        }

        public class StarStatistics
        {
            #region caching
            public void Save(BinaryWriter save)
            {
                save.Write(age);
                save.Write(life);
                save.Write(locked);
                save.Write(luminosity);
                save.Write(radius);
                save.Write(spectralClass);
                save.Write(temperature);
            }

            public StarStatistics(BinaryReader load)
            {
                age = load.ReadDouble();
                life = load.ReadDouble();
                locked = load.ReadBoolean();
                luminosity = load.ReadDouble();
                radius = load.ReadDouble();
                spectralClass = load.ReadString();
                temperature = load.ReadDouble();
            }
            #endregion caching

            public readonly double age;
            public readonly double life;
            public readonly bool locked;
            public readonly double luminosity;
            public readonly double radius;
            public readonly string spectralClass;
            public readonly double temperature;

            public StarStatistics(YamlNode node)
            {
                YamlMappingNode mapping = (YamlMappingNode)node;
                foreach (var entry in mapping.Children)
                {
                    string paramName = entry.Key.ToString();
                    switch (paramName)
                    {
                        case "age":
                            age = Double.Parse(entry.Value.ToString());
                            break;
                        case "life":
                            life = Double.Parse(entry.Value.ToString());
                            break;
                        case "locked":
                            locked = Boolean.Parse(entry.Value.ToString());
                            break;
                        case "luminosity":
                            luminosity = Double.Parse(entry.Value.ToString());
                            break;
                        case "radius":
                            radius = Double.Parse(entry.Value.ToString());
                            break;
                        case "spectralClass":
                            spectralClass = entry.Value.ToString();
                            break;
                        case "temperature":
                            temperature = Double.Parse(entry.Value.ToString());
                            break;
                        default:
                            System.Diagnostics.Debug.WriteLine("StarStatistics unknown value:" + entry.Key + " = " + entry.Value);
                            break;
                    }
                }
            }
        }
    }
}
