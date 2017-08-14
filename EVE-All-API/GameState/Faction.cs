using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVE_All_API
{
    public class Faction
    {
        private static Dictionary<long, Faction> factions = new Dictionary<long, Faction>();
        public static Faction getFaction(long _factionID)
        {
            if(_factionID == 0)
            {
                return null;
            }
            Faction faction = null;
            if (factions.ContainsKey(_factionID))
            {
                faction = factions[_factionID];
            }
            else
            {
                faction = new Faction(_factionID);
                factions[_factionID] = faction;
            }
            return faction;
        }

        public readonly long factionID;
        public string factionName;

        private Faction(long _factionID)
        {
            factionID = _factionID;
        }

    }
}
