using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EVE_All_API
{
    public class xmlManager
    {
        private class PageResult
        {
            public DateTime cachedUntil;
            public XmlDocument doc;
            public XmlNode result;
        }
        private static Dictionary<string, PageResult> pages = new Dictionary<string, PageResult>();

        /// <summary>
        /// Get an XML page result.
        /// </summary>
        /// <param name="url">The page to get.</param>
        /// <param name="key">The API key to append to the url request.</param>
        /// <returns>The result node of the page.</returns>
        public static XmlNode getPage(string url, APIKey key)
        {
            if(key == null)
            {
                // No API key, try to get without.
                return getPage(url);
            }
            // Construct URL with API key parameters.
            string apiURL = url;
            string token = "?";
            // Check for existing parameters in URL.
            if(url.Contains("?"))
            {
                // There are alreay parameters so add the key values to the end.
                token = "&";
            }
            // Add the key parameters.
            apiURL += token + "keyID=" + key.keyID + "&vCode=" + key.vCode;
            // Get the page.
            return getPage(apiURL);
        }

        /// <summary>
        /// Get an XML page result.
        /// </summary>
        /// <param name="url">The page to get.</param>
        /// <returns>The result node of the page.</returns>
        public static XmlNode getPage(string url)
        {
            string loadURL = url;
            string saveFile = url;
            // Do we have a full URL?
            if (!url.StartsWith("http:"))
            {
                // No, add the server address.
                loadURL = UserData.apiURL + url;
            }
            else
            {
                // Remove the server address from the save file.
                if (saveFile.StartsWith(UserData.apiURL))
                {
                    saveFile = saveFile.Substring(UserData.apiURL.Length);
                }
            }
            saveFile = saveFile.Replace("\\", "-");
            saveFile = saveFile.Replace("/", "-");
            saveFile = saveFile.Replace("?", "_");
            saveFile = saveFile.Replace("&", "-");
            saveFile = saveFile.Replace(".xml", "");
            saveFile = saveFile.Replace(".aspx", "");
            // Check for a cached page.
            bool expired;
            XmlNode result = getCached(url, out expired);
            if (!expired)
            {
                return result;
            }
            XmlDocument doc = new XmlDocument();
            try
            {
                // Load the page.
                doc.Load(loadURL);
            }
            catch (WebException)
            {
                //Diagnostics.Debug.WriteLine(e.ToString());
                // There was a problem loading the page!
                return null;
            }
            // Try to parse the loaded document.
            XmlNode parseResult = parseDocument(doc, url, saveFile);
            if(parseResult != null)
            {
                // Return the parsed result.
                return parseResult;
            }
            // Fall back to the cache if available.
            return result;
        }

        /// <summary>
        /// Parse a document to get the result node.
        /// </summary>
        /// <param name="doc">The document to parse.</param>
        /// <param name="url">The url the document was retrieved from.</param>
        /// <param name="saveFile">The file to save the cache to.</param>
        /// <param name="save">Should we save a cache file?</param>
        /// <returns>The result node, if found.</returns>
        private static XmlNode parseDocument(XmlDocument doc, string url, string saveFile, bool save = true)
        {
            XmlNode result = null;
            XmlElement root = doc.DocumentElement;
            if (root == null)
            {
                // The result is empty!
                return result;
            }
            DateTime cached = new DateTime();
            foreach (XmlNode node in root.ChildNodes)
            {
                if (node.Name == "cachedUntil")
                {
                    // Found the cache time.
                    cached = DateTimeOffset.Parse(node.InnerText).UtcDateTime;
                }
                if (node.Name == "result")
                {
                    // Found the result.
                    result = node;
                }
            }
            // Got cache and result.
            if(cached.Ticks != 0 && result != null)
            {
                // Create a caceh entry.
                PageResult page = new PageResult();
                page.cachedUntil = cached;
                page.doc = doc;
                page.result = result;
                // Save the entry.
                pages[url] = page;
                if (!UserData.saveNoKey)
                {
                    if (!url.Contains("keyID"))
                    {
                        save = false;
                    }
                }
                if (UserData.cachePath != null && save)
                {
                    // Make sure directory exists.
                    if(!Directory.Exists(UserData.cachePath))
                    {
                        Directory.CreateDirectory(UserData.cachePath);
                    }
                    // Save the document to the cache file.
                    doc.Save(UserData.cachePath + saveFile + ".xml");
                }
            }
            // Return the result node.
            return result;
        }

        /// <summary>
        /// Gets a cached page.
        /// </summary>
        /// <param name="url">The page to get.</param>
        /// <param name="expired">Should we return expired pages?</param>
        /// <returns>The found page.</returns>
        public static XmlNode getCached(string url, out bool expired)
        {
            // Check for a cached page.
            if (pages.ContainsKey(url))
            {
                // We already got this page once before.
                PageResult page = pages[url];
                // Is the cache still valid?
                expired = page.cachedUntil < DateTime.Now;
                return page.result;
            }
            else
            {
                // We have not loaded this page this session so see if there is a cache file.
                string saveFile = url;
                // Remove the server address from the save file.
                if (saveFile.StartsWith(UserData.apiURL))
                {
                    saveFile = saveFile.Substring(UserData.apiURL.Length);
                }
                saveFile = saveFile.Replace("\\", "-");
                saveFile = saveFile.Replace("/", "-");
                saveFile = saveFile.Replace("?", "_");
                saveFile = saveFile.Replace("&", "-");
                saveFile = saveFile.Replace(".xml", "");
                saveFile = saveFile.Replace(".aspx", "");
                if (UserData.cachePath != null)
                {
                    if (File.Exists(UserData.cachePath + saveFile + ".xml"))
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.Load(UserData.cachePath + saveFile + ".xml");
                        if(parseDocument(doc, url, saveFile, false) != null)
                        {
                            PageResult page = pages[url];
                            // Is the cache still valid?
                            expired = page.cachedUntil < DateTime.Now;
                            return page.result;
                        }
                    }
                }
            }
            expired = true;
            return null;
        }
    }
}
