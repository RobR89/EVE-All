﻿using System.Collections.Generic;

namespace EVE_All_API
{
    public class Corporation
    {
        private static Dictionary<long, Corporation> corporations = new Dictionary<long, Corporation>();
        public static Corporation getCorporation(long _corporationID)
        {
            if(_corporationID == 0)
            {
                return null;
            }
            Corporation corp = null;
            if (corporations.ContainsKey(_corporationID))
            {
                corp = corporations[_corporationID];
            }
            else
            {
                corp = new Corporation(_corporationID);
                corporations[_corporationID] = corp;
            }
            return corp;
        }

        public readonly long corporationID;
        public string corporationName;

        private Corporation(long _corporationID)
        {
            corporationID = _corporationID;
        }

    }
}
