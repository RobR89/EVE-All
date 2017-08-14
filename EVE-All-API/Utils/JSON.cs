using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace EVE_All_API
{
    public class JSON
    {
#region Depreciated
        public class JSONPage<T>
        {
            public int totalCount;
            public string totalCount_str;
            public int pageCount;
            public string pageCount_str;

            public Dictionary<string, string> next = null;
            public Dictionary<string, string> previous = null;

            public List<T> items = null;
        }

        public class PageItem
        {
            public int id;
            public string name;
            public string href;

            public void load(JsonReader reader)
            {
                string propertyName = string.Empty;
                while (reader.Read())
                {
                    if (reader.Value != null)
                    {
                        if (reader.TokenType == JsonToken.PropertyName)
                        {
                            propertyName = reader.Value.ToString();
                            continue;
                        }
                        switch (propertyName)
                        {
                            case "name":
                                name = reader.Value.ToString();
                                break;
                            case "id_str":
                                break;
                            case "id":
                                id = Int32.Parse(reader.Value.ToString());
                                break;
                            case "href":
                                href = reader.Value.ToString();
                                break;
                        }
                    }
                    else
                    {
                        if (reader.TokenType == JsonToken.EndObject)
                        {
                            return;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get all referance items from a page.  Follows "next" references for multiple page responses.
        /// </summary>
        /// <param name="url">The url to get.</param>
        /// <returns>The page items retrieved.</returns>
        public static List<T> getPageItems<T>(string url)
        {
            List<T> items = new List<T>();
            JSONResponse resp = getJSON(url);
            if(resp.code != HttpStatusCode.OK)
            {
                return null;
            }
            JSONPage<T> page = new JSONPage<T>();
            JsonConvert.PopulateObject(resp.content, page);
            items.AddRange(page.items);
            while(page.next != null && page.next.ContainsKey("href"))
            {
                string href = page.next["href"];
                resp = getJSON(href);
                if (resp.code != HttpStatusCode.OK)
                {
                    continue;
                }
                page = new JSONPage<T>();
                JsonConvert.PopulateObject(resp.content, page);
                items.AddRange(page.items);
            }
            return items;
        }
#endregion

        public class ESIList<T>
        {
            public DateTime expires = DateTime.Now;
            public List<T> items = new List<T>();
        }

        public static ESIList<T> getESIlist<T>(string url, bool paged)
        {
            int pageNum = 1;
            List<T> found = new List<T>();
            ESIList<T> result = new ESIList<T>();
            do
            {
                found.Clear();
                // Create page reference
                string pageURL = url;
                if (paged)
                {
                    pageURL = url + "&page=" + pageNum.ToString();
                }
                JSON.JSONResponse resp = JSON.getJSON(pageURL);
                if (resp.code == System.Net.HttpStatusCode.OK)
                {
                    JsonConvert.PopulateObject(resp.content, found);
                    result.items.AddRange(found);
                    result.expires = resp.expires;
                }
                else
                {
                    // There was an error.  The response content may contain a JSON formated error string.
                    return result;
                }
                pageNum++;
            } while (found.Count > 0 && paged);
            return result;
        }

        public class ESIItem<T> where T : new()
        {
            public DateTime expires = DateTime.Now;
            public T item = new T();
        }

        public static ESIItem<T> getESIItem<T>(string url) where T : new()
        {
            ESIItem<T> result = new ESIItem<T>();
            JSON.JSONResponse resp = JSON.getJSON(url);
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

        public class JSONResponse
        {
            public string content = null;
            // Http header info.
            // Dates are converted to local time.
            public DateTime date;
            public DateTime expires;
            public HttpStatusCode code;
        }

        /// <summary>
        /// Load a JSON page.  If the url does not begin with http the json server URL is prepended.
        /// </summary>
        /// <param name="url">The url to fetch.</param>
        /// <returns>The page response.</returns>
        public static JSONResponse getJSON(string url)
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
            JSONResponse resp = new JSONResponse();
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
            }
            StreamReader reader = new StreamReader(response.GetResponseStream());
            resp.content = reader.ReadToEnd();
            response.Close();

            return resp;
        }

    }
}
