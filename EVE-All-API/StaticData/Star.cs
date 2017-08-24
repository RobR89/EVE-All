using System;
using YamlDotNet.RepresentationModel;

namespace EVE_All_API.StaticData
{
    public class Star
    {
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
