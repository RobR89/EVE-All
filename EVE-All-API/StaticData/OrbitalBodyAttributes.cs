using System;
using YamlDotNet.RepresentationModel;

namespace EVE_All_API.StaticData
{
    public class OrbitalBodyAttributes
    {
        public readonly int heightMap1;
        public readonly int heightMap2;
        public readonly bool population;
        public readonly int shaderPreset;

        public OrbitalBodyAttributes(YamlNode node)
        {
            YamlMappingNode mapping = (YamlMappingNode)node;
            foreach (var entry in mapping.Children)
            {
                string paramName = entry.Key.ToString();
                switch (paramName)
                {
                    case "heightMap1":
                        heightMap1 = Int32.Parse(entry.Value.ToString());
                        break;
                    case "heightMap2":
                        heightMap2 = Int32.Parse(entry.Value.ToString());
                        break;
                    case "shaderPreset":
                        shaderPreset = Int32.Parse(entry.Value.ToString());
                        break;
                    case "population":
                        population = Boolean.Parse(entry.Value.ToString());
                        break;
                    default:
                        System.Diagnostics.Debug.WriteLine("PlanetAttributes unknown value:" + entry.Key + " = " + entry.Value);
                        break;
                }
            }
        }

    }
}
