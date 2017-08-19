using EVE_All_API.StaticData;
using System;
using System.Collections.Generic;

namespace EVE_All
{
    public class Tests
    {
        //Market.updateMarketGroups();
        //Market.updateMarketValues();
        //Market.updateRegionMarket(999);
        //Market.updateRegionMarket(10000030);
        //List<long> orderIDs = Market.getOrdersForTypeAndRegion(34, 10000030);
        //Market.MarketEntry order = Market.getOrder(orderIDs[0]);
        //Sovereignty.updateStructures();
        //Sovereignty.Structure str = Sovereignty.getStructure(1022679654375);
        //Sovereignty.updateMap();
        //Sovereignty.Map map = Sovereignty.getMap(30000721);

        public static void pathTest()
        {
            // Will only work after loading successful.
            DateTime s = DateTime.Now;
            SolarSystem start = SolarSystem.getSystem(30002510);
            List<int> path = start.findPath(30000142, true);
            if (path == null)
            {
                System.Diagnostics.Debug.WriteLine("Path not found.");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(path.Count + " jumps.");
                foreach (int systemID in path)
                {
                    //SolarSystem system = SolarSystem.getSystem(systemID);
                    System.Diagnostics.Debug.WriteLine(systemID + ": " + InvNames.getName(systemID));
                }
            }
            DateTime f = DateTime.Now;
            TimeSpan ts = f - s;
            System.Diagnostics.Debug.WriteLine("Path test took: " + ts.TotalMilliseconds + " ms");
        }

    }
}
