using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace EVE_All_API
{
    public class JSON
    {

        public class JSONResponse
        {
            public string content = null;
            // Http header info.
            public DateTime date;
            public DateTime expires;
            public int pages = 0;
            public HttpStatusCode httpCode;
        }

        /// <summary>
        /// Load a JSON page.  If the url does not begin with http the ESI server URL is prepended.
        /// </summary>
        /// <param name="url">The url to fetch.</param>
        /// <param name="query">The query parameters.</param>
        /// <param name="token">The access token to use, or null if no token is to be used.</param>
        /// <returns>The page response, or null if the token is expired and can't refresh.</returns>
        /// <remarks>path should not begin with / or \</remarks>
        /// <remarks>path should end with /</remarks>
        /// <remarks>query should begin with &</remarks>
        public static JSONResponse GetJSONPage(string url, Dictionary<string, string> query, AccessToken token = null)
        {
            Dictionary<string, string> _query = query;
            if (_query == null)
            {
                _query = new Dictionary<string, string>();
            }
            else
            {
                _query = new Dictionary<string, string>(query);
            }
            _query["datasource"] = UserData.esiDatasource;
            string useURL = url.Trim();
            if (!url.StartsWith("http"))
            {
                // Remove leading slashes.
                while (useURL.StartsWith("/") || useURL.StartsWith("\\"))
                {
                    useURL = useURL.Remove(0, 1);
                }
                // Make sure it ends with a slash/
                if (!useURL.EndsWith("/") && !useURL.EndsWith("\\"))
                {
                    useURL = useURL + "/";
                }
                useURL = UserData.esiURL + useURL;
            }
            string pageQuery = "";
            if (_query?.Count > 0)
            {
                // Make sure query starts with &
                string delim = "?";
                foreach(KeyValuePair<string, string> param in _query)
                {
                    pageQuery += delim + param.Key + "=" + param.Value;
                    delim = "&";
                }
            }
            Uri uri = new Uri(useURL + pageQuery);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
            request.Method = WebRequestMethods.Http.Get;
            if(token != null)
            {
                if(token.IsExpired())
                {
                    if(!token.Refresh())
                    {
                        // Token refresh failed.
                        return null;
                    }
                }
                // Add the authorization header.
                request.Headers.Add("Authorization", token.token_type + " " + token.access_token);
            }
            HttpWebResponse response = null;
            JSONResponse resp = new JSONResponse();
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch(WebException e)
            {
                response = (HttpWebResponse)e.Response;
            }
            if(response == null)
            {
                return null;
            }
            resp.httpCode = response.StatusCode;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                resp.date = DateTime.Parse(response.Headers.Get("date"));
                string expiresString = response.Headers.Get("expires");
                if (!String.IsNullOrEmpty(expiresString))
                {
                    resp.expires = DateTime.Parse(expiresString);
                }
                string pageString = response.Headers.Get("x-pages");
                if (!String.IsNullOrEmpty(pageString))
                {
                    resp.pages = Int32.Parse(pageString);
                }
            }
            StreamReader reader = new StreamReader(response.GetResponseStream());
            resp.content = reader.ReadToEnd();
            response.Close();

            return resp;
        }

    }
}
