using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.RepresentationModel;

namespace EVE_All_API.StaticData
{
    public class SolarSystem
    {
        private static Dictionary<int, SolarSystem> solarSystems = new Dictionary<int, SolarSystem>();
        public static SolarSystem getSystem(int _solarSystemID)
        {
            lock(solarSystems)
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
                        center = Location.parseLocation(entry.Value);
                        break;
                    case "max":
                        max = Location.parseLocation(entry.Value);
                        break;
                    case "min":
                        min = Location.parseLocation(entry.Value);
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
                        foreach(YamlNode seqNode in seq.Children)
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
                        disallowedAnchorGroups = YamlUtils.loadIntList(entry.Value);
                        break;
                    default:
                        System.Diagnostics.Debug.WriteLine("SolarSystem unknown value:" + entry.Key + " = " + entry.Value);
                        break;
                }
            }
            // Parse these here so we can know we have the solarSystemID.
            if (gateNode != null)
            {
                stargates = Stargate.loadYAML(gateNode, solarSystemID);
            }
            if (planetNode != null)
            {
                planets = OrbitalBody.loadYAML(planetNode, solarSystemID);
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

        public static bool loadYAML(YamlStream yaml)
        {
            if (yaml == null)
            {
                return false;
            }
            SolarSystem sys = new SolarSystem(yaml.Documents[0].RootNode);
            lock(solarSystems)
            {
                solarSystems[sys.solarSystemID] = sys;
            }
            return true;
        }

        /*
        TO-DO: The path if high sec is selected is sometimes longer than necissary.
        I.e. Rens to Jita is 2 jumps longer.
        */
        /*
28 jumps.
30002510: Rens
30002526: Frarn
30002529: Gyng
30002568: Onga
30002544: Pator
30002543: Eystur
30002053: Hek
30002049: Uttindar
30002048: Bei
30002682: Colelie
30002681: Deltole

30002680: Augnais
30002676: Parchanier
30002706: Doussivitte
30002711: Stetille
30002710: Adiere
30002709: Auberulle
30002707: Unel
30002763: Tennen
30002766: Iivinen

30002765: Sivala
30002768: Uedama
30002803: Juunigaishi
30002805: Anttiri
30002791: Sirppala
30000139: Urlen
30000144: Perimeter
30000142: Jita
*/
        /*
        In Game
30002510: Rens
30002526: Frarn
30002529: Gyng
30002568: Onga
Lustravik
30002543: Eystur
30002053: Hek
30002049: Uttindar
30002048: Bei
30002682: Colelie
30002681: Deltole

            Aufay
            Balle
            DuAnnes
            Renyn
            Algogille
            Kassigainen
30002764: Hatakani

30002765: Sivala
30002768: Uedama
30002803: Juunigaishi
30002805: Anttiri
30002791: Sirppala
30000139: Urlen
30000144: Perimeter
30000142: Jita
*/

        /// <summary>
        /// Find the path from this system to destination system.
        /// </summary>
        /// <param name="destinationID">solarSystemID of destination system.</param>
        /// <param name="highSec">True if high sec path prefered.</param>
        /// <returns>The path or null if not found.</returns>
        public List<int> findPath(int destinationID, bool highSec = false)
        {
            List<int> path = new List<int>();
            Dictionary<int, int> distanceMap = getDistanceMap(highSec);
            if(!distanceMap.ContainsKey(destinationID))
            {
                return null;
            }
            int distance = distanceMap[destinationID];
            int currentSystemID = destinationID;
            path.Add(currentSystemID);
            int lowestSystemID = currentSystemID;
            int lowestDistance = distance;
            while(distance > 0)
            {
                Dictionary<int, int> gateDistance = new Dictionary<int, int>();
                // Get the solar system.
                SolarSystem system = SolarSystem.getSystem(currentSystemID);
                foreach(int gateID in system.stargates)
                {
                    // Get the stargate.
                    Stargate gate = Stargate.getStargate(gateID);
                    if (gate != null)
                    {
                        // Get the destination gate.
                        gate = Stargate.getStargate(gate.destination);
                        if (gate != null)
                        {
                            // Does the system exist in the map?
                            if (distanceMap.ContainsKey(gate.solarSystemID))
                            {
                                int dist = distanceMap[gate.solarSystemID];
                                gateDistance[gate.solarSystemID] = dist;
                                lowestDistance = Math.Min(lowestDistance, dist);
                            }
                        }
                    }
                }
                // Remove longer jumps.
                var longer = gateDistance.Where(g => g.Value > lowestDistance).ToArray();
                foreach(var dest in longer)
                {
                    gateDistance.Remove(dest.Key);
                }
                bool lowestCorridor = false;
                foreach (var dest in gateDistance)
                {
                    SolarSystem destSystem = SolarSystem.getSystem(dest.Key);
                    // If it's closer go there.
                    if (lowestSystemID == currentSystemID)
                    {
                        // save this as currently shortest path.
                        lowestDistance = dest.Value;
                        lowestSystemID = dest.Key;
                        if (destSystem?.corridor == true)
                        {
                            lowestCorridor = true;
                        }
                        //break;
                    }
                    // If it's same distance check for coridor.
                    else if (destSystem?.corridor == true && !lowestCorridor)
                    {
                        // Save this as corridor prefered.
                        lowestDistance = dest.Value;
                        lowestSystemID = dest.Key;
                        break;
                    }
                }
                if (lowestSystemID != currentSystemID)
                {
                    // Add next point in path.
                    distance = lowestDistance;
                    currentSystemID = lowestSystemID;
                    path.Add(lowestSystemID);
                }
                else
                {
                    if (highSec)
                    {
                        // Cannot find path with current restrictions, try none.
                        return findPath(destinationID);
                    }
                    return null;
                }
            }
            // Reverse the list so the first system begins the list.
            path.Reverse();
            return path;
        }

        public Dictionary<int, int> getDistanceMap(bool highSec = false)
        {
            Dictionary<int, int> distanceMap = new Dictionary<int, int>();
            List<int> systems = new List<int>();
            // Add this system as the seed location.
            distanceMap[solarSystemID] = 0;
            systems.Add(solarSystemID);
            // Start interating the systems.
            while(systems.Count > 0)
            {
                // Get the first unproccessed system.
                int _solarSystemID = systems[0];
                systems.Remove(_solarSystemID);
                // Get the solar system.
                SolarSystem system = getSystem(_solarSystemID);
                if(system == null)
                {
                    continue;
                }
                // Get the next distance.
                int distance = distanceMap[_solarSystemID] + 1;
                // Iterate the gates in this system.
                List<int> gateSystems = new List<int>();
                foreach (int gateID in system.stargates)
                {
                    // Get the stargate.
                    Stargate gate = Stargate.getStargate(gateID);
                    if (gate != null)
                    {
                        // Get the destination gate.
                        gate = Stargate.getStargate(gate.destination);
                        if (gate != null)
                        {
                            int destinationSystemID = gate.solarSystemID;
                            SolarSystem destinationSystem = getSystem(destinationSystemID);
                            // If this is a high security system or we don't care.
                            if (destinationSystem != null && (destinationSystem.security >= 0.5 || !highSec))
                            {
                                // Add the system.
                                gateSystems.Add(destinationSystemID);
                            }
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
