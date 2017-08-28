using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YamlDotNet.RepresentationModel;

namespace EVE_All_API.StaticData
{
    public class SolarSystem
    {
        #region caching
        public static void SaveAll(BinaryWriter save)
        {
            lock (solarSystems)
            {
                Loader.SaveDict(solarSystems, save, Save);
            }
        }

        public static bool LoadAll(BinaryReader load)
        {
            lock (solarSystems)
            {
                solarSystems = Loader.LoadDict<SolarSystem>(load, Load);
            }
            return true;
        }

        public static void Save(SolarSystem system, BinaryWriter save)
        {
            system.Save(save);
        }

        public static SolarSystem Load(BinaryReader load)
        {
            return new SolarSystem(load);
        }

        public void Save(BinaryWriter save)
        {
            save.Write(solarSystemID);
            save.Write(solarSystemNameID);
            save.Write(regional);
            save.Write(border);
            save.Write(corridor);
            save.Write(fringe);
            save.Write(hub);
            save.Write(international);
            save.Write(luminosity);
            save.Write(radius);
            save.Write(security);
            save.Write(sunTypeID);
            center.Save(save);
            max.Save(save);
            min.Save(save);
            Loader.Save(securityClass, save);
            save.Write(wormholeClassID);
            Loader.Save(disallowedAnchorCategories, save);
            save.Write(descriptionID);
            save.Write(factionID);
            Loader.Save(stargates, save);
            Loader.Save(planets, save);
            star.Save(save);
            if(secondarySun == null)
            {
                save.Write(false);
            }
            else
            {
                save.Write(true);
                secondarySun.Save(save);
            }
            Loader.Save(visualEffect, save);
            Loader.Save(disallowedAnchorGroups, save);
        }

        private SolarSystem(BinaryReader load)
        {
            solarSystemID = load.ReadInt32();
            solarSystemNameID = load.ReadInt32();
            regional = load.ReadBoolean();
            border = load.ReadBoolean();
            corridor = load.ReadBoolean();
            fringe = load.ReadBoolean();
            hub = load.ReadBoolean();
            international = load.ReadBoolean();
            luminosity = load.ReadDouble();
            radius = load.ReadDouble();
            security = load.ReadDouble();
            sunTypeID = load.ReadInt32();
            center = new Location(load);
            max = new Location(load);
            min = new Location(load);
            Loader.Load(out securityClass, load);
            wormholeClassID = load.ReadInt32();
            Loader.Load(out disallowedAnchorCategories, load);
            descriptionID = load.ReadInt32();
            factionID = load.ReadInt32();
            Loader.Load(out stargates, load);
            Loader.Load(out planets, load);
            star = new Star(load);
            if (load.ReadBoolean())
            {
                secondarySun = new SecondarySun(load);
            }
            else
            {
                secondarySun = null;
            }
            Loader.Load(out visualEffect, load);
            Loader.Load(out disallowedAnchorGroups, load);
        }
        #endregion caching

        private static Dictionary<int, SolarSystem> solarSystems = new Dictionary<int, SolarSystem>();
        public static SolarSystem GetSystem(int _solarSystemID)
        {
            lock (solarSystems)
            {
                if (solarSystems.ContainsKey(_solarSystemID))
                {
                    return solarSystems[_solarSystemID];
                }
                return null;
            }
        }

        public readonly int solarSystemID;
        public readonly int solarSystemNameID;
        public readonly bool regional;
        public readonly bool border;
        public readonly bool corridor;
        public readonly bool fringe;
        public readonly bool hub;
        public readonly bool international;
        public readonly double luminosity;
        public readonly double radius;
        public readonly double security;
        public readonly int sunTypeID;
        public readonly Location center;
        public readonly Location max;
        public readonly Location min;
        public readonly string securityClass;
        public readonly int wormholeClassID;
        public readonly List<int> disallowedAnchorCategories;
        public readonly int descriptionID;
        public readonly int factionID;
        public readonly List<int> stargates;
        public readonly List<long> planets;
        public readonly Star star;
        public readonly SecondarySun secondarySun;
        public readonly string visualEffect;
        public readonly List<int> disallowedAnchorGroups;

        private SolarSystem(YamlNode node)
        {
            YamlNode gateNode = null;
            YamlNode planetNode = null;
            YamlNode starNode = null;
            YamlNode secondarySunNode = null;
            YamlMappingNode mapping = (YamlMappingNode)node;
            foreach (var entry in mapping.Children)
            {
                string paramName = entry.Key.ToString();
                switch (paramName)
                {
                    case "solarSystemID":
                        solarSystemID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "solarSystemNameID":
                        solarSystemNameID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "regional":
                        regional = Boolean.Parse(entry.Value.ToString());
                        break;
                    case "border":
                        border = Boolean.Parse(entry.Value.ToString());
                        break;
                    case "corridor":
                        corridor = Boolean.Parse(entry.Value.ToString());
                        break;
                    case "fringe":
                        fringe = Boolean.Parse(entry.Value.ToString());
                        break;
                    case "hub":
                        hub = Boolean.Parse(entry.Value.ToString());
                        break;
                    case "international":
                        international = Boolean.Parse(entry.Value.ToString());
                        break;
                    case "luminosity":
                        luminosity = Double.Parse(entry.Value.ToString());
                        break;
                    case "radius":
                        radius = Double.Parse(entry.Value.ToString());
                        break;
                    case "security":
                        security = Double.Parse(entry.Value.ToString());
                        break;
                    case "sunTypeID":
                        sunTypeID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "center":
                        center = Location.ParseLocation(entry.Value);
                        break;
                    case "max":
                        max = Location.ParseLocation(entry.Value);
                        break;
                    case "min":
                        min = Location.ParseLocation(entry.Value);
                        break;
                    case "wormholeClassID":
                        wormholeClassID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "descriptionID":
                        descriptionID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "factionID":
                        factionID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "securityClass":
                        securityClass = entry.Value.ToString();
                        break;
                    case "disallowedAnchorCategories":
                        disallowedAnchorCategories = new List<int>();
                        YamlSequenceNode seq = (YamlSequenceNode)entry.Value;
                        foreach (YamlNode seqNode in seq.Children)
                        {
                            disallowedAnchorCategories.Add(Int32.Parse(seqNode.ToString()));
                        }
                        break;
                    case "planets":
                        planetNode = entry.Value;
                        break;
                    case "star":
                        starNode = entry.Value;
                        break;
                    case "stargates":
                        gateNode = entry.Value;
                        break;
                    case "secondarySun":
                        secondarySunNode = entry.Value;
                        break;
                    case "visualEffect":
                        visualEffect = entry.Value.ToString();
                        break;
                    case "disallowedAnchorGroups":
                        disallowedAnchorGroups = YamlUtils.LoadIntList(entry.Value);
                        break;
                    default:
                        System.Diagnostics.Debug.WriteLine("SolarSystem unknown value:" + entry.Key + " = " + entry.Value);
                        break;
                }
            }
            // Parse these here so we can know we have the solarSystemID.
            if (gateNode != null)
            {
                stargates = Stargate.LoadYAML(gateNode, solarSystemID);
            }
            if (planetNode != null)
            {
                planets = OrbitalBody.LoadYAML(planetNode, solarSystemID);
            }
            if (starNode != null)
            {
                star = new Star(starNode, solarSystemID);
            }
            if (secondarySunNode != null)
            {
                secondarySun = new SecondarySun(secondarySunNode, solarSystemID);
            }
        }

        public static bool LoadYAML(YamlStream yaml)
        {
            if (yaml == null)
            {
                return false;
            }
            SolarSystem sys = new SolarSystem(yaml.Documents[0].RootNode);
            lock (solarSystems)
            {
                solarSystems[sys.solarSystemID] = sys;
            }
            return true;
        }

        public bool IsHighSec()
        {
            // Security is rounded up by the half.
            return security >= 0.45;
        }

        public bool IsLowSec()
        {
            // Security is rounded up by the half.
            return security >= 0.05 && !IsHighSec();
        }

        public bool IsNULLSec()
        {
            return !IsLowSec() && !IsHighSec();
        }

        //----------------------------------------------------------------------
        // Path finding
        private static Dictionary<int, List<int>> systemDestinations = new Dictionary<int, List<int>>();
        private static bool GenerateDestinations()
        {
            // Prevent multiple initializations.
            lock (systemDestinations)
            {
                if (systemDestinations.Count > 0)
                {
                    // Already initialized.
                    return true;
                }
                foreach (SolarSystem system in SolarSystem.solarSystems.Values)
                {
                    // Are there any stargates?
                    if (system.stargates.Count > 0)
                    {
                        // Create the destination list.
                        systemDestinations[system.solarSystemID] = new List<int>();
                        foreach (int gateID in system.stargates)
                        {
                            // Get the stargate.
                            Stargate gate = Stargate.GetStargate(gateID);
                            if (gate != null)
                            {
                                // Get the destination gate.
                                gate = Stargate.GetStargate(gate.destination);
                                if (gate != null)
                                {
                                    int destinationSystemID = gate.solarSystemID;
                                    SolarSystem destinationSystem = GetSystem(destinationSystemID);
                                    // If this is a high security system or we don't care.
                                    if (destinationSystem != null)
                                    {
                                        // Add the system.
                                        systemDestinations[system.solarSystemID].Add(destinationSystemID);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }

        class PathParse
        {
            public PathParse()
            {
            }
            public PathParse(PathParse pp)
            {
                distance = pp.distance;
                currentSystemID = pp.currentSystemID;
                // Save the distance reference.
                distanceMap = pp.distanceMap;
                // Copy the current path.
                path = new List<int>(pp.path);
            }
            // Distance for current system.
            public int distance;
            // Current system.
            public int currentSystemID;
            // Found path.
            public List<int> path;
            public Dictionary<int, int> distanceMap;
            // Split paths note.
            public List<int> splitsUsed = new List<int>();
        }

        /// <summary>
        /// Find the path from this system to destination system.
        /// </summary>
        /// <param name="destinationID">solarSystemID of destination system.</param>
        /// <param name="highSec">True if high sec path prefered.</param>
        /// <returns>The path or null if not found.</returns>
        public List<int> FindPath(int destinationID, bool highSec = false)
        {
            // Initialize the found path.
            PathParse pp = new PathParse()
            {
                // Get the distance map.
                distanceMap = GetDistanceMap(highSec)
            };
            if (!pp.distanceMap.ContainsKey(destinationID))
            {
                // Unreachable system!
                return null;
            }
            pp.path = new List<int>();
            if (destinationID == solarSystemID)
            {
                // We are already there!
                return pp.path;
            }
            // Init path finding.
            pp.distance = pp.distanceMap[destinationID];
            pp.currentSystemID = destinationID;
            // Add destination system.
            pp.path.Add(pp.currentSystemID);
            // Find the shortest path.
            pp = FindShortest(pp, highSec);
            // Check results.
            if (pp == null)
            {
                // No path found.
                if (highSec)
                {
                    // Cannot find path with current restrictions, try none.
                    return FindPath(destinationID);
                }
                return null;
            }
            // Reverse the list so the first system begins the list.
            pp.path.Reverse();
            return pp.path;
        }

        private PathParse FindShortest(PathParse pp, bool highSec)
        {
            // Find starting system.
            while (pp.distance > 0)
            {
                // Check that the system has jumps.
                if (!systemDestinations.ContainsKey(pp.currentSystemID))
                {
                    // Error, system not found.
                    return null;
                }
                // Get the distances for each jump.
                Dictionary<int, int> jumpDistances = new Dictionary<int, int>();
                int lowestDistance = pp.distance;
                foreach (int destinationSystemID in systemDestinations[pp.currentSystemID])
                {
                    // Does the system exist in the map?
                    if (pp.distanceMap.ContainsKey(destinationSystemID))
                    {
                        int dist = pp.distanceMap[destinationSystemID];
                        jumpDistances[destinationSystemID] = dist;
                        lowestDistance = Math.Min(lowestDistance, dist);
                    }
                }
                // Set distance to lowest.
                pp.distance = lowestDistance;
                // Remove longer jumps.
                var longer = jumpDistances.Where(g => g.Value > lowestDistance).ToArray();
                foreach (var dest in longer)
                {
                    jumpDistances.Remove(dest.Key);
                }
                if (jumpDistances.Count == 0)
                {
                    // Error, jumps not found.
                    return null;
                }
                else if (jumpDistances.Count == 1)
                {
                    // Save this as currently shortest path.
                    int dest = jumpDistances.ElementAt(0).Key;
                    if (dest != pp.currentSystemID)
                    {
                        // Add next point in path.
                        pp.currentSystemID = dest;
                        if (pp.path.Contains(pp.currentSystemID))
                        {
                            // We have somehow looped back on ourself.
                            return null;
                        }
                        pp.path.Add(pp.currentSystemID);
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    PathParse spp = null;
                    // 2 or more systems of same distance but not necessarily equal paths.
                    foreach (int dest in jumpDistances.Keys)
                    {
                        // Create a copy of the path.
                        PathParse npp = new PathParse(pp)
                        {
                            // Add this point in path.
                            currentSystemID = dest
                        };
                        if (npp.path.Contains(npp.currentSystemID))
                        {
                            // We have somehow looped back on ourself.
                            continue;
                        }
                        npp.path.Add(npp.currentSystemID);
                        // Find the shortest distance if this option is taken.
                        npp = FindShortest(npp, highSec);
                        if (spp == null)
                        {
                            // First successful path, keep it.
                            spp = npp;
                        }
                        else
                        {
                            if (npp != null)
                            {
                                // Another successful path, is it shorter?
                                if (npp.path.Count < spp.path.Count)
                                {
                                    // Yes!  Keep it.
                                    spp = npp;
                                }
                            }
                        }
                    }
                    return spp;
                }
            }
            return pp;
        }

        public Dictionary<int, int> GetDistanceMap(bool highSec = false)
        {
            if (systemDestinations == null || systemDestinations.Count == 0)
            {
                GenerateDestinations();
            }
            Dictionary<int, int> distanceMap = new Dictionary<int, int>();
            List<int> systems = new List<int>();
            // Add this system as the seed location.
            distanceMap[solarSystemID] = 0;
            systems.Add(solarSystemID);
            // Start interating the systems.
            while (systems.Count > 0)
            {
                // Get the first unproccessed system.
                int _solarSystemID = systems[0];
                systems.Remove(_solarSystemID);
                // Get the solar system.
                SolarSystem system = GetSystem(_solarSystemID);
                if (system == null)
                {
                    continue;
                }
                // Get the next distance.
                int distance = distanceMap[_solarSystemID] + 1;
                // Iterate the gates in this system.
                List<int> gateSystems = new List<int>();
                if (!systemDestinations.ContainsKey(system.solarSystemID))
                {
                    // No jumps in this system.
                    continue;
                }
                foreach (int destinationSystemID in systemDestinations[system.solarSystemID])
                {
                    if (!highSec)
                    {
                        // We don't care about security.
                        gateSystems.Add(destinationSystemID);
                    }
                    else
                    {
                        // We want to stay safe!
                        SolarSystem destinationSystem = GetSystem(destinationSystemID);
                        if (destinationSystem != null && destinationSystem.IsHighSec())
                        {
                            // Add the system.
                            gateSystems.Add(destinationSystemID);
                        }
                    }
                }
                // Process the destination systems.
                foreach (int destinationSystemID in gateSystems)
                {
                    // Does the system exist in the map?
                    if (distanceMap.ContainsKey(destinationSystemID))
                    {
                        // Was this a shorter route?
                        if (distanceMap[destinationSystemID] > distance)
                        {
                            // Yes, save the new distance.
                            distanceMap[destinationSystemID] = distance;
                            // Are we still waiting to map this system?
                            if (!systems.Contains(destinationSystemID))
                            {
                                // No, so we need to redo this system.
                                systems.Add(destinationSystemID);
                            }
                        }
                    }
                    else
                    {
                        // We have not reached this system before.
                        distanceMap[destinationSystemID] = distance;
                        // Add the system to parse list.
                        systems.Add(destinationSystemID);
                    }
                }
            }
            return distanceMap;
        }

    }
}
