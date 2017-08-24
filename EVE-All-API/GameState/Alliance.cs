using System.Collections.Generic;

namespace EVE_All_API
{
    public class Alliance
    {
        private static Dictionary<long, Alliance> alliances = new Dictionary<long, Alliance>();
        public static Alliance getAlliance(long _allianceID)
        {
            if(_allianceID == 0)
            {
                return null;
            }
            Alliance alliance = null;
            if (alliances.ContainsKey(_allianceID))
            {
                alliance = alliances[_allianceID];
            }
            else
            {
                alliance = new Alliance(_allianceID);
                alliances[_allianceID] = alliance;
            }
            return alliance;
        }

        public readonly long allianceID;
        public string allianceName;

        private Alliance(long _allianceID)
        {
            allianceID = _allianceID;
        }

    }
}
