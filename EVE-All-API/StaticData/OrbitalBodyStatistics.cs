using System;
using System.IO;
using YamlDotNet.RepresentationModel;

namespace EVE_All_API.StaticData
{
    public class OrbitalBodyStatistics
    {
        #region caching
        public void Save(BinaryWriter save)
        {
            save.Write(density);
            save.Write(eccentricity);
            save.Write(escapeVelocity);
            save.Write(fragmented);
            save.Write(life);
            save.Write(locked);
            save.Write(massDust);
            save.Write(massGas);
            save.Write(orbitPeriod);
            save.Write(orbitRadius);
            save.Write(pressure);
            save.Write(radius);
            save.Write(rotationRate);
            save.Write(spectralClass);
            save.Write(surfaceGravity);
            save.Write(temperature);
        }

        public OrbitalBodyStatistics(BinaryReader load)
        {
            density = load.ReadDouble();
            eccentricity = load.ReadDouble();
            escapeVelocity = load.ReadDouble();
            fragmented = load.ReadBoolean();
            life = load.ReadDouble();
            locked = load.ReadBoolean();
            massDust = load.ReadDouble();
            massGas = load.ReadDouble();
            orbitPeriod = load.ReadDouble();
            orbitRadius = load.ReadDouble();
            pressure = load.ReadDouble();
            radius = load.ReadDouble();
            rotationRate = load.ReadDouble();
            spectralClass = load.ReadString();
            surfaceGravity = load.ReadDouble();
            temperature = load.ReadDouble();
        }
        #endregion caching

        public readonly double density;
        public readonly double eccentricity;
        public readonly double escapeVelocity;
        public readonly bool fragmented;
        public readonly double life;
        public readonly bool locked;
        public readonly double massDust;
        public readonly double massGas;
        public readonly double orbitPeriod;
        public readonly double orbitRadius;
        public readonly double pressure;
        public readonly double radius;
        public readonly double rotationRate;
        public readonly string spectralClass;
        public readonly double surfaceGravity;
        public readonly double temperature;

        public OrbitalBodyStatistics(YamlNode node)
        {
            YamlMappingNode mapping = (YamlMappingNode)node;
            foreach (var entry in mapping.Children)
            {
                string paramName = entry.Key.ToString();
                switch (paramName)
                {
                    case "density":
                        density = Double.Parse(entry.Value.ToString());
                        break;
                    case "eccentricity":
                        eccentricity = Double.Parse(entry.Value.ToString());
                        break;
                    case "escapeVelocity":
                        escapeVelocity = Double.Parse(entry.Value.ToString());
                        break;
                    case "life":
                        life = Double.Parse(entry.Value.ToString());
                        break;
                    case "massDust":
                        massDust = Double.Parse(entry.Value.ToString());
                        break;
                    case "massGas":
                        massGas = Double.Parse(entry.Value.ToString());
                        break;
                    case "orbitPeriod":
                        orbitPeriod = Double.Parse(entry.Value.ToString());
                        break;
                    case "orbitRadius":
                        orbitRadius = Double.Parse(entry.Value.ToString());
                        break;
                    case "pressure":
                        pressure = Double.Parse(entry.Value.ToString());
                        break;
                    case "radius":
                        radius = Double.Parse(entry.Value.ToString());
                        break;
                    case "rotationRate":
                        rotationRate = Double.Parse(entry.Value.ToString());
                        break;
                    case "surfaceGravity":
                        surfaceGravity = Double.Parse(entry.Value.ToString());
                        break;
                    case "temperature":
                        temperature = Double.Parse(entry.Value.ToString());
                        break;
                    case "fragmented":
                        fragmented = Boolean.Parse(entry.Value.ToString());
                        break;
                    case "locked":
                        locked = Boolean.Parse(entry.Value.ToString());
                        break;
                    case "spectralClass":
                        spectralClass = entry.Value.ToString();
                        break;
                    default:
                        System.Diagnostics.Debug.WriteLine("PlanetStatistics unknown value:" + entry.Key + " = " + entry.Value);
                        break;
                }
            }
        }

    }
}
