using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.RepresentationModel;

namespace EVE_All_API.StaticData
{
    public class OrbitalBody
    {
        #region caching
        public static void SaveAll(BinaryWriter save)
        {
            lock (planets)
            {
                save.Write(planets.Count);
                foreach (OrbitalBody planet in planets.Values)
                {
                    planet.Save(save);
                }
            }
        }

        public static bool LoadAll(BinaryReader load)
        {
            lock (planets)
            {
                int count = load.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    OrbitalBody planet = new OrbitalBody(load);
                    planets[planet.orbitalBodyID] = planet;
                }
            }
            return true;
        }

        public void Save(BinaryWriter save)
        {
            save.Write(orbitalBodyID);
            save.Write(orbitalBodyNameID);
            save.Write(solarSystemID);
            save.Write(celestialIndex);
            save.Write(typeID);
            save.Write(radius);
            position.Save(save);
            if (attributes == null)
            {
                save.Write(false);
            }
            else
            {
                save.Write(true);
                attributes.Save(save);
            }
            if (statistics == null)
            {
                save.Write(false);
            }
            else
            {
                save.Write(true);
                statistics.Save(save);
            }
            Loader.Save(moons, save);
            Loader.Save(stations, save);
            Loader.Save(asteroidBelts, save);
        }

        private OrbitalBody(BinaryReader load)
        {
            orbitalBodyID = load.ReadInt32();
            orbitalBodyNameID = load.ReadInt32();
            solarSystemID = load.ReadInt32();
            celestialIndex = load.ReadInt32();
            typeID = load.ReadInt32();
            radius = load.ReadDouble();
            position = new Location(load);
            if (load.ReadBoolean())
            {
                attributes = new OrbitalBodyAttributes(load);
            }
            else
            {
                attributes = null;
            }
            if (load.ReadBoolean())
            {
                statistics = new OrbitalBodyStatistics(load);
            }
            else
            {
                statistics = null;
            }
            Loader.Load(out moons, load);
            Loader.Load(out stations, load);
            Loader.Load(out asteroidBelts, load);
        }
        #endregion caching

        private static Dictionary<int, OrbitalBody> planets = new Dictionary<int, OrbitalBody>();
        public static OrbitalBody GetOrbitalBody(int _planetID)
        {
            lock(planets)
            {
                if (planets.ContainsKey(_planetID))
                {
                    return planets[_planetID];
                }
            }
            return null;
        }

        public readonly int orbitalBodyID;
        public readonly int orbitalBodyNameID;
        public readonly int solarSystemID;
        public readonly int celestialIndex = -1;
        public readonly int typeID;
        public readonly double radius;
        public readonly Location position;
        public readonly OrbitalBodyAttributes attributes;
        public readonly OrbitalBodyStatistics statistics;
        public readonly List<long> moons;
        public readonly List<long> stations;
        public readonly List<long> asteroidBelts;

        private OrbitalBody(YamlNode node, int _planetID, int _solarSystemID)
        {
            orbitalBodyID = _planetID;
            solarSystemID = _solarSystemID;
            YamlMappingNode mapping = (YamlMappingNode)node;
            foreach (var entry in mapping.Children)
            {
                string paramName = entry.Key.ToString();
                switch (paramName)
                {
                    case "celestialIndex":
                        celestialIndex = Int32.Parse(entry.Value.ToString());
                        break;
                    case "asteroidBeltNameID":
                    case "moonNameID":
                    case "planetNameID":
                        orbitalBodyNameID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "typeID":
                        typeID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "radius":
                        radius = Double.Parse(entry.Value.ToString());
                        break;
                    case "position":
                        position = Location.ParseLocation(entry.Value);
                        break;
                    case "planetAttributes":
                        attributes = new OrbitalBodyAttributes(entry.Value);
                        break;
                    case "statistics":
                        statistics = new OrbitalBodyStatistics(entry.Value);
                        break;
                    case "moons":
                        moons = OrbitalBody.LoadYAML(entry.Value, _solarSystemID);
                        break;
                    case "asteroidBelts":
                        asteroidBelts = OrbitalBody.LoadYAML(entry.Value, _solarSystemID);
                        break;
                    case "npcStations":
                        stations = NPCStation.LoadYAML(entry.Value, _solarSystemID);
                        break;
                    default:
                        System.Diagnostics.Debug.WriteLine("OrbitalBody unknown value:" + entry.Key + " = " + entry.Value);
                        break;
                }
            }
        }

        public static List<long> LoadYAML(YamlNode yaml, int _solarSystemID)
        {
            if (yaml == null)
            {
                return null;
            }
            List<long> systemPlanets = new List<long>();
            YamlMappingNode mapping = (YamlMappingNode)yaml;
            foreach (var entry in mapping.Children)
            {
                int _planetID = Int32.Parse(entry.Key.ToString());
                OrbitalBody planet = new OrbitalBody(entry.Value, _planetID, _solarSystemID);
                lock (planets)
                {
                    planets[planet.orbitalBodyID] = planet;
                }
                systemPlanets.Add(_planetID);
            }
            return systemPlanets;
        }

    }
}
