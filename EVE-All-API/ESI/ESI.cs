using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVE_All_API.ESI
{
    public class ESI
    {
        /// <summary>
        /// Construct the url from the provided path and query/
        /// </summary>
        /// <param name="path">The endpoint path</param>
        /// <param name="query">The query parameters</param>
        /// <returns>The url</returns>
        /// <remarks>path should not begin with / or \</remarks>
        /// <remarks>path should end with /</remarks>
        /// <remarks>query should begin with &</remarks>
        public static string constructURL(string path, string query)
        {
            // Remove leading slashes.
            while (path.StartsWith("/") || path.StartsWith("\\"))
            {
                path = path.Remove(0, 1);
            }
            // Make sure it ends with a slash/
            if (!path.EndsWith("/") && !path.EndsWith("\\"))
            {
                path = path + "/";
            }
            // Make sure it starts with &
            if (!query.StartsWith("&") && query.Length > 0)
            {
                query = "&" + query;
            }
            return UserData.esiURL + path + "?datasource=" + UserData.esiDatasource + query;
        }

    }
}
