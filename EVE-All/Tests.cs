using EVE_All_API.StaticData;
using System;
using System.Collections.Generic;

namespace EVE_All
{
    public class Tests
    {
        /*
            Market.UpdateMarketGroups();
            Market.UpdateMarketValues();
            //Market.UpdateRegionMarket(999);
            Market.UpdateRegionMarket(10000030);
            List<long> orderIDs = Market.GetOrdersForTypeAndRegion(34, 10000030);
            Market.MarketOrder order = Market.GetOrder(orderIDs[0]);
            Sovereignty.UpdateStructures();
            Sovereignty.Structure str = Sovereignty.GetStructure(1035629924);
            Sovereignty.UpdateMap();
            Sovereignty.Map map = Sovereignty.GetMap(30000721);
        */
        public static void pathTest()
        {
            // Will only work after loading successful.
            DateTime s = DateTime.Now;
            SolarSystem start = SolarSystem.GetSystem(30002510);
            List<int> path = start.FindPath(30000142, true);
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
                    System.Diagnostics.Debug.WriteLine(systemID + ": " + InvNames.GetName(systemID));
                }
            }
            DateTime f = DateTime.Now;
            TimeSpan ts = f - s;
            System.Diagnostics.Debug.WriteLine("Path test took: " + ts.TotalMilliseconds + " ms");
        }

    }
}
