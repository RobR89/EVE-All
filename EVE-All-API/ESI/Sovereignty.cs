using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private static Dictionary<long, Structure> structureSov = new Dictionary<long, Structure>();
        private static DateTime structureSovExpire = DateTime.Now;
        public static Structure getStructure(long structureID)
        {
            lock(structureSov)
            {
                if(structureSov.ContainsKey(structureID))
                {
                    return structureSov[structureID];
                }
            }
            return null;
        }

        public static void updateStructures()
        {
            if (structureSovExpire > DateTime.Now)
            {
                return;
            }
            JSON.ESIList<Structure> sov = JSON.getESIlist<Structure>("sovereignty/structures/");
            lock (structureSov)
            {
                structureSovExpire = sov.expires;
                foreach (Structure structure in sov.items)
                {
                    structureSov[structure.structure_id] = structure;
                }
            }
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

        private static Dictionary<int, Map> mapSov = new Dictionary<int, Map>();
        private static DateTime mapSovExpire = DateTime.Now;
        public static Map getMap(int systemID)
        {
            lock(mapSov)
            {
                if(mapSov.ContainsKey(systemID))
                {
                    return mapSov[systemID];
                }
            }
            return null;
        }

        public static void updateMap()
        {
            if (mapSovExpire > DateTime.Now)
            {
                return;
            }
            JSON.ESIList<Map> sov = JSON.getESIlist<Map>("sovereignty/map/");
            lock (mapSov)
            {
                mapSovExpire = sov.expires;
                foreach (Map map in sov.items)
                {
                    mapSov[map.system_id] = map;
                }
            }
        }
        #endregion
    }
}
