using System.Collections.Generic;

namespace EVE_All_API.ESI
{
    public class Universe
    {
        private static RegionList regionList = new RegionList();
        public static List<int> GetRegions()
        {
            lock (regionList)
            {
                return new List<int>(regionList.items);
            }
        }
        private class RegionList : ESIList<int>
        {
            public RegionList()
            {
                url = "universe/regions/";
                autoUpdate = false;
                PageUpdated += RegionList_PageUpdated;
                ScheduleRefresh();
            }

            private void RegionList_PageUpdated(object page)
            {
                lock (regionList)
                {
                    foreach (int regionID in items)
                    {
                        GetRegion(regionID);
                    }
                }
            }
        }

        private static Dictionary<int, Region> regions = new Dictionary<int, Region>();
        public static Region GetRegion(int regionID)
        {
            if(!regionList.items.Contains(regionID))
            {
                return null;
            }
            lock(regions)
            {
                if(!regions.ContainsKey(regionID))
                {
                    regions[regionID] = new Region(regionID);
                }
                return regions[regionID];
            }
        }

        public class Region : ESIPage
        {
            public Region(int regionID)
            {
                url = "universe/regions/" + regionID.ToString() + "/";
                autoUpdate = false;
                lock(regions)
                {
                    regions[regionID] = this;
                }
                ScheduleRefresh();
            }

            public int region_id;
            public string name;
            public string description;
            List<int> constellations = new List<int>();
        }

    }
}
