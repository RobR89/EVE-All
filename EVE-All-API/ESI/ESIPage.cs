using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static EVE_All_API.JSON;

namespace EVE_All_API.ESI
{
    public class ESIPage
    {
        // Update information.
        public DateTime expire;
        public bool IsPageExpired
        {
            get
            {
                return DateTime.Now >= expire;
            }
        }

        // Auto update information.
        public bool autoUpdate = true;
        public Action<Task> autoUpdateAction = null;

        private Task waitTask;
        public void ScheduleRefresh()
        {
            lock (this)
            {
                if (waitTask?.IsCompleted != true || autoUpdateAction == null)
                {
                    return;
                }
                waitTask = Task.Delay(expire.Subtract(DateTime.Now));
                waitTask.ContinueWith(autoUpdateAction);
            }
        }

        public string url = null;
        public Dictionary<string, string> query = null;
        protected string scope = null;
        public virtual JSONResponse GetPage(long characterID = 0)
        {
            lock (this)
            {
                if (url == null)
                {
                    // No URL
                    return null;
                }
                if (!IsPageExpired)
                {
                    // Page not expired, make sure refresh scheduled.
                    ScheduleRefresh();
                    return null;
                }
                AccessToken token = null;
                if (scope != null && characterID != 0)
                {
                    // Get token.
                    token = AccessToken.GetTokenForCharacterWithScope(characterID, scope);
                    if (token == null)
                    {
                        // No token available.
                        return null;
                    }
                }
                JSON.JSONResponse resp = JSON.GetJSONPage(url, query, token);
                if (resp?.httpCode == System.Net.HttpStatusCode.OK)
                {
                    JsonConvert.PopulateObject(resp.content, this);
                    expire = resp.expires;
                }
                else
                {
                    // Error, what to do?
                    if (resp?.httpCode == HttpStatusCode.Forbidden || resp?.httpCode == HttpStatusCode.Unauthorized)
                    {
                        // Not allowed, don't try again.
                        return resp;
                    }
                    // Wait 5 min before trying again.
                    expire = DateTime.Now + new TimeSpan(0, 5, 0);
                }

                // Schedule an auto update.
                if (autoUpdate)
                {
                    ScheduleRefresh();
                }
                return resp;
            }
        }

    }

    public class ESIList<T> : ESIPage
    {
        public List<T> items = new List<T>();
        public override JSONResponse GetPage(long characterID = 0)
        {
            lock (this)
            {
                if (url == null)
                {
                    // No URL
                    return null;
                }
                if (!IsPageExpired)
                {
                    // Page not expired, make sure refresh scheduled.
                    ScheduleRefresh();
                    return null;
                }
                AccessToken token = null;
                if (scope != null && characterID != 0)
                {
                    // Get token.
                    token = AccessToken.GetTokenForCharacterWithScope(characterID, scope);
                    if (token == null)
                    {
                        // No token available.
                        return null;
                    }
                }
                int pageNum = 1;
                int pageCount = 1;
                List<T> found = new List<T>();
                items.Clear();
                JSON.JSONResponse resp;
                Dictionary<string, string> pageQuery = query;
                if (pageQuery == null)
                {
                    pageQuery = new Dictionary<string, string>();
                }
                else
                {
                    pageQuery = new Dictionary<string, string>(query);
                }
                do
                {
                    found.Clear();
                    pageQuery["page"] = pageNum.ToString();
                    resp = JSON.GetJSONPage(url, pageQuery, token);
                    if (resp?.httpCode == System.Net.HttpStatusCode.OK)
                    {
                        JsonConvert.PopulateObject(resp.content, found);
                        items.AddRange(found);
                        pageCount = resp.pages;
                        expire = resp.expires;
                    }
                    else
                    {
                        // Error, what to do?
                        if (resp?.httpCode == HttpStatusCode.Forbidden || resp?.httpCode == HttpStatusCode.Unauthorized)
                        {
                            // Not allowed, don't try again.
                            return resp;
                        }
                        // Wait 5 min before trying again.
                        expire = DateTime.Now + new TimeSpan(0, 5, 0);
                    }
                    pageNum++;
                } while (found.Count > 0 && pageNum <= pageCount);
                
                // Schedule an auto update.
                if (autoUpdate)
                {
                    ScheduleRefresh();
                }
                return resp;
            }
        }

    }
}
