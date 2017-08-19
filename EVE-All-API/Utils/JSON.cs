using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace EVE_All_API
{
    public class JSON
    {

        public class ESIList<T>
        {
            public DateTime expires = DateTime.Now;
            public List<T> items = new List<T>();
        }

        /// <summary>
        /// Get a List of items from the EVE Swagger interface.
        /// </summary>
        /// <typeparam name="T">The object type to be filled.</typeparam>
        /// <param name="url">The url to fetch.</param>
        /// <param name="query">The query parameters.</param>
        /// <param name="token">The access token to use, or null if no token is to be used.</param>
        /// <returns>The List of items.</returns>
        public static ESIList<T> getESIlist<T>(string url, string query = null, AccessToken token = null)
        {
            int pageNum = 1;
            int pageCount = 1;
            List<T> found = new List<T>();
            ESIList<T> result = new ESIList<T>();
            do
            {
                found.Clear();
                // Create page reference
                string pageURL = url;
                pageURL = url + "&page=" + pageNum.ToString();
                JSON.ESIResponse resp = JSON.getESIPage(pageURL, query, token);
                if (resp?.code == System.Net.HttpStatusCode.OK)
                {
                    JsonConvert.PopulateObject(resp.content, found);
                    result.items.AddRange(found);
                    result.expires = resp.expires;
                    pageCount = resp.pages;
                }
                else
                {
                    // There was an error.  The response content may contain a JSON formated error string.
                    return result;
                }
                pageNum++;
            } while (found.Count > 0 && pageNum <= pageCount);
            return result;
        }

        public class ESIItem<T> where T : new()
        {
            public DateTime expires = DateTime.Now;
            public T item = new T();
        }

        /// <summary>
        /// Get a single item from the EVE Swagger Interface.
        /// </summary>
        /// <typeparam name="T">The Item Type to get.</typeparam>
        /// <param name="url">The URL to get the item from.</param>
        /// <param name="query">The query parameters.</param>
        /// <param name="token">The access token to use, or null if no token is to be used.</param>
        /// <returns>The Item information.</returns>
        public static ESIItem<T> getESIItem<T>(string url, string query = null, AccessToken token = null) where T : new()
        {
            ESIItem<T> result = new ESIItem<T>();
            JSON.ESIResponse resp = JSON.getESIPage(url, query, token);
            if (resp?.code == System.Net.HttpStatusCode.OK)
            {
                //result.item = new T();
                JsonConvert.PopulateObject(resp.content, result.item);
                result.expires = resp.expires;
                return result;
            }
            // There was an error.  The response content may contain a JSON formated error string.
            return null;
        }

        public class ESIResponse
        {
            public string content = null;
            // Http header info.
            // Dates are converted to local time.
            public DateTime date;
            public DateTime expires;
            public int pages = 0;
            public HttpStatusCode code;
        }

        /// <summary>
        /// Load a JSON page.  If the url does not begin with http the json server URL is prepended.
        /// </summary>
        /// <param name="url">The url to fetch.</param>
        /// <param name="query">The query parameters.</param>
        /// <param name="token">The access token to use, or null if no token is to be used.</param>
        /// <returns>The page response, or null if the token is expired and can't refresh.</returns>
        /// <remarks>path should not begin with / or \</remarks>
        /// <remarks>path should end with /</remarks>
        /// <remarks>query should begin with &</remarks>
        public static ESIResponse getESIPage(string url, string query, AccessToken token = null)
        {
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
                useURL = UserData.esiURL + useURL + "?datasource=" + UserData.esiDatasource;
            }
            if (query?.Length > 0)
            {
                // Make sure query starts with &
                if (!query.StartsWith("&"))
                {
                    useURL += "&";
                }
                useURL += query;
            }
            Uri uri = new Uri(useURL + query);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
            request.Method = WebRequestMethods.Http.Get;
            if(token != null)
            {
                if(token.isExpired())
                {
                    if(!token.refresh())
                    {
                        // Token refresh failed.
                        return null;
                    }
                }
                // Add the authorization header.
                request.Headers.Add("Authorization", token.token_type + " " + token.access_token);
            }
            HttpWebResponse response = null;
            ESIResponse resp = new ESIResponse();
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch(WebException e)
            {
                response = (HttpWebResponse)e.Response;
            }
            resp.code = response.StatusCode;
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
