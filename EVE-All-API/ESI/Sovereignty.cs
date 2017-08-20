using System;
using System.Collections.Generic;

namespace EVE_All_API.ESI
{
    public class Sovereignty
    {
#region Structure
        public class Structure
        {
            public long alliance_id;
            public int solar_system_id;
            public long structure_id;
            public int structure_type_id;
            public double vulnerability_occupancy_level;
            public DateTime vulnerable_start_time;
            public DateTime vulnerable_end_time;
        }

        private class StructuresPage : ESIList<Structure>
        {
            public StructuresPage()
            {
                url = "sovereignty/structures/";
                autoUpdate = false;
            }
        }
        private static StructuresPage structuresPage = new StructuresPage();

        public static Structure GetStructure(long structureID)
        {
            lock(structuresPage)
            {
                foreach (Structure structure in structuresPage.items)
                {
                    if(structure.structure_id == structureID)
                    {
                        return structure;
                    }
                }
            }
            return null;
        }

        public static bool UpdateStructures()
        {
            lock (structuresPage)
            {
                JSON.JSONResponse resp = structuresPage.GetPage();
                if (resp.httpCode == System.Net.HttpStatusCode.OK)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region map
        public class Map
        {
            public int system_id;
            public int faction_id;
            public long alliance_id;
            public long corporation_id;
        }

        private class MapsPage : ESIList<Map>
        {
            public MapsPage()
            {
                url = "sovereignty/map/";
                autoUpdate = false;
            }
        }
        private static MapsPage mapsPage = new MapsPage();

        public static Map GetMap(int systemID)
        {
            lock(mapsPage)
            {
                foreach (Map map in mapsPage.items)
                {
                    if(map.system_id == systemID)
                    {
                        return map;
                    }
                }
            }
            return null;
        }

        public static bool UpdateMap()
        {
            lock (mapsPage)
            {
                JSON.JSONResponse resp = mapsPage.GetPage();
                if (resp.httpCode == System.Net.HttpStatusCode.OK)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}
