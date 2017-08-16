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
        /// <param name="paged">True if page numbers should be used.</param>
        /// <returns>The List of items.</returns>
        public static ESIList<T> getESIlist<T>(string url)
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
                JSON.ESIResponse resp = JSON.getESIPage(pageURL);
                if (resp.code == System.Net.HttpStatusCode.OK)
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
        /// <returns>The Item information.</returns>
        public static ESIItem<T> getESIItem<T>(string url) where T : new()
        {
            ESIItem<T> result = new ESIItem<T>();
            JSON.ESIResponse resp = JSON.getESIPage(url);
            if (resp.code == System.Net.HttpStatusCode.OK)
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
        /// <returns>The page response.</returns>
        public static ESIResponse getESIPage(string url)
        {
            string useURL = url.Trim();
            if (!url.StartsWith("http"))
            {
                useURL = UserData.esiURL + url;
            }
            Uri uri = new Uri(useURL);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
            request.Method = WebRequestMethods.Http.Get;
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
                resp.expires = DateTime.Parse(response.Headers.Get("expires"));
                string pageString = response.Headers.Get("x-pages");
                if (pageString != null)
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
