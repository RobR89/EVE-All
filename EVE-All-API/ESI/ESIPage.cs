using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Threading;
using System.Timers;
using static EVE_All_API.JSON;

namespace EVE_All_API.ESI
{
    public class ESIPage
    {
        // Update information.
        public DateTime expire = new DateTime(0);
        public bool IsPageExpired
        {
            get
            {
                return DateTime.Now >= expire;
            }
        }

        // Auto update information.
        public bool autoUpdate = true;
        public delegate void PageHandler(object page);
        public event PageHandler PageUpdated;

        private static List<ESIPage> pending = new List<ESIPage>();
        private static List<ESIPage> updateing = new List<ESIPage>();
        private static List<ESIPage> updated = new List<ESIPage>();
        private static BackgroundWorker refreshWorker = null;
        private static EventWaitHandle waitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);
        public void ScheduleRefresh()
        {
            lock (pending)
            {
                if (refreshWorker == null)
                {
                    // Set up the refresh timer only once.
                    refreshWorker = new BackgroundWorker();
                    refreshWorker.DoWork += RefreshWorker_DoWork;
                    refreshWorker.RunWorkerAsync();
                }
                if (pending.Contains(this) || updateing.Contains(this))
                {
                    // The update is already pending or in progress.
                    return;
                }
                // Add to pending list.
                pending.Add(this);
                waitHandle.Set();
            }
        }

        private void RefreshWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!refreshWorker.CancellationPending)
            {
                waitHandle.WaitOne(100);
                List<ESIPage> pages = new List<ESIPage>();
                lock (pending)
                {
                    pages = new List<ESIPage>(pending);
                }
                while (pages.Count > 0)
                {
                    List<ESIPage> done = new List<ESIPage>();
                    foreach (ESIPage page in pages)
                    {
                        if (page.IsPageExpired)
                        {
                            System.Diagnostics.Debug.WriteLine("Starting thread ESI->GetPage() " + page.url);
                            new Thread(() => page.GetPage()).Start();
                            done.Add(page);
                        }
                    }
                    lock (pending)
                    {
                        foreach (ESIPage page in done)
                        {
                            pending.Remove(page);
                            // Track which pages are updating.
                            updateing.Add(page);
                        }
                        pages = new List<ESIPage>(pending);
                    }
                }
                waitHandle.Reset();
            }
        }

        protected void UpdatePage()
        {
            // Add minimun 10 seconds to allow for slight differences or the refreshed page might be the same.
            expire += new TimeSpan(0, 0, 10);
            lock (pending)
            {
                // Remove from updates list.
                pending.Remove(this);
                updateing.Remove(this);
            }
            // Send from new thread to prevent delays and deadlocking with this thread if the event handler calls back to us.
            System.Diagnostics.Debug.WriteLine("Starting thread ESI->PageUpdated " + url);
            new Thread(() => PageUpdated?.Invoke(this)).Start();
        }

        public string url = null;
        public Dictionary<string, string> query = null;
        protected string scope = null;
        public long characterID = 0;
        protected virtual JSONResponse GetPage()
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
                lock (this)
                {
                    JsonConvert.PopulateObject(resp.content, this);
                    expire = resp.expires;
                }
                UpdatePage();
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

    public class ESIList<T> : ESIPage
    {
        public List<T> items = new List<T>();
        protected override JSONResponse GetPage()
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
            List<T> allFound = new List<T>();
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
                    allFound.AddRange(found);
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
            if (resp?.httpCode == System.Net.HttpStatusCode.OK)
            {
                lock (this)
                {
                    // Finaly, fill the items list.
                    items.Clear();
                    items.AddRange(allFound);
                }
                UpdatePage();
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
