using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Xml;

namespace EVE_All_API
{
    public class SSO
    {
        private static HttpListener listener = null;
        private static BackgroundWorker worker = null;
        private static string _redirect_uri;

        /// <summary>
        /// Start a new Login request, starting a new listener if neccissary.
        /// </summary>
        public static void StartRequest()
        {
            // Make sure there is a listener running.
            if (_redirect_uri != UserData.sso_RedirectURI)
            {
                // Not same redirect, create new listener.
                listener?.Stop();
                listener?.Close();
                // Stop worker thread.
                worker?.CancelAsync();
                while (worker?.IsBusy == true) { }
                listener = null;
                worker = null;
            }
            if(listener == null || worker == null)
            {
                // Create new listener and worker.
                listener = new HttpListener();
                worker = new BackgroundWorker();
                // Save the new redirect URI
                _redirect_uri = UserData.sso_RedirectURI;
                // Add the prefix.
                listener.Prefixes.Add(UserData.sso_RedirectURI);
                // Start the listener.
                worker.DoWork += Worker_DoWork;
                worker.WorkerSupportsCancellation = true;
                worker.RunWorkerAsync();
            }

            // Get the request parameters.
            string state = Guid.NewGuid().ToString();
            string scopes = WebUtility.UrlEncode(UserData.sso_Scopes);

            // Construct URL
            string url = "https://login.eveonline.com/oauth/authorize/?response_type=code&client_id=";
            url += UserData.sso_ClientID + "&redirect_uri=" + WebUtility.UrlEncode(UserData.sso_RedirectURI);
            url += "&state=" + state;
            if (!String.IsNullOrEmpty(scopes))
            {
                url += "&scope=" + scopes;
            }
            // Start the web browser.
            System.Diagnostics.Process.Start(url);
        }

        private static void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            // Run the auth service.
            listener.Start();
            while (!worker.CancellationPending)
            {
                // Listen for responses.
                try
                {
                    ThreadPool.QueueUserWorkItem(Incomming, listener.GetContext());
                }
                catch (HttpListenerException ex)
                {
                    // TO-DO: respond to error...
                    System.Diagnostics.Debug.WriteLine("SSO lsitener error: " + ex.Message);
                    worker.CancelAsync();
                }
            }

            // Shut down the listener.
            listener.Stop();
            listener.Close();
            listener = null;
            worker = null;
        }

        private static void Incomming(object o)
        {
            // Handle incomming response.
            HttpListenerContext context = o as HttpListenerContext;
            if (context == null)
            {
                return;
            }
            // Get the request from the context.
            HttpListenerRequest request = context.Request;
            // Get the request values.
            NameValueCollection values = request.QueryString;
            string code = null;
            string state = null;
            // Check for the values we need.
            foreach (string key in values.AllKeys)
            {
                string value = values[key];
                if (key == "code")
                {
                    code = value;
                }
                if (key == "state")
                {
                    state = value;
                }
            }
            // Create a response to display in our browser.
            // TO-DO: make this pretty!?
            string responseString = "Logged in to EVE Online character.";
            if (code == null || state == null)
            {
                // We did not get some or all of the required info.
                responseString = "Login to EVE Online failed.";
            }
            // Genereate response.
            responseString = "<HTML><BODY>" + responseString + "</BODY></HTML>";
            HttpListenerResponse response = context.Response;
            // Add the content.
            byte[] buf = Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buf.Length;
            Stream output = response.OutputStream;
            output.Write(buf, 0, buf.Length);
            output.Close();
            if (code == null || state == null)
            {
                // Failed, we are done!
                return;
            }
            // Now get the access token.
            AccessToken token = new AccessToken();
            if (token.FetchToken(code))
            {
                AccessToken.AddToken(token);
            }
        }

        /// <summary>
        /// Get the XML node for saved tokens.
        /// </summary>
        /// <param name="doc">The document to save the tokens in.</param>
        /// <returns>The node with the save state.</returns>
        public static XmlElement GetTokenNode(XmlDocument doc)
        {
            XmlElement rowset = doc.CreateElement("rowset");
            XmlAttribute attribute = doc.CreateAttribute("name");
            attribute.Value = "SSO_Tokens";
            rowset.Attributes.Append(attribute);
            foreach (AccessToken token in AccessToken.GetAccessTokens())
            {
                if (String.IsNullOrEmpty(token.refresh_token))
                {
                    // Don't save tokens that don't refresh.
                    continue;
                }
                // create the row.
                XmlElement row = doc.CreateElement("row");
                rowset.AppendChild(row);
                xmlUtils.newAttribute(row, "access_token", token.access_token);
                xmlUtils.newAttribute(row, "token_type", token.token_type);
                xmlUtils.newAttribute(row, "refresh_token", token.refresh_token);
                xmlUtils.newAttribute(row, "expires_in", token.expires_in.ToString());
                xmlUtils.newAttribute(row, "generated", token.generated.Ticks.ToString());
            }
            return rowset;
        }

        /// <summary>
        /// Load tokens from the saved XML node.
        /// </summary>
        /// <param name="rowset">The XML node to load the tokens from.</param>
        public static void LoadTokens(XmlNode rowset)
        {
            List<Dictionary<string, string>> rows;
            string[] columns = { "access_token", "token_type", "refresh_token", "expires_in", "generated" };
            if (xmlUtils.parseRowSet(rowset, "SSO_Tokens", out rows, columns))
            {
                foreach (Dictionary<string, string> row in rows)
                {
                    AccessToken token = new AccessToken()
                    {
                        access_token = row["access_token"],
                        token_type = row["token_type"],
                        refresh_token = row["refresh_token"],
                        expires_in = long.Parse(row["expires_in"]),
                        generated = new DateTime(long.Parse(row["generated"]))
                    };
                    AccessToken.AddToken(token);
                }
            }
        }

    }
}
