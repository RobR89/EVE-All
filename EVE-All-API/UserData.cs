using System.IO;
using System.Xml;

namespace EVE_All_API
{
    public class UserData
    {
        /// <summary>
        /// The path to where cache files should be saved.  NULL if files are not to be saved.
        /// </summary>
        public static string cachePath = null;
        /// <summary>
        /// The path to where image files should be saved.  NULL if files are not to be saved.
        /// </summary>
        public static string imagePath = null;
        /// <summary>
        /// Save cache files for pages with no API key?
        /// </summary>
        public static bool saveNoKey = false;
        /// <summary>
        /// The URL to get API keys.
        /// </summary>
        public static string getKeyURL = "https://community.eveonline.com/support/api-key/";
        /// <summary>
        /// The URL for the API server.
        /// </summary>
        public static string apiURL = "https://api.eveonline.com/";
        /// <summary>
        /// The URL for the image server.
        /// </summary>
        public static string imageURL = "https://imageserver.eveonline.com/";
        /// <summary>
        /// The SDE zip file with the type icons.
        /// </summary>
        public static string typeIconZip = null;
        /// <summary>
        /// The SDE zip file with the Render images.
        /// </summary>
        public static string renderZip = null;
        /// <summary>
        /// The SDE zip file with the icon images.
        /// </summary>
        public static string iconsZip = null;
        /// <summary>
        /// The language code to use when reading language sets.
        /// </summary>
        public static string language = "en";
        /// <summary>
        /// The path to the static data export files.
        /// </summary>
        public static string sdeZip= "";
        /// <summary>
        /// The url of the ESI server to use.
        /// </summary>
        public static string esiURL = "https://esi.tech.ccp.is/latest/";
        /// <summary>
        /// The datasorce of the ESI server to use.
        /// </summary>
        public static string esiDatasource = "tranquility";
        /// <summary>
        /// The client ID for the SS) application.
        /// </summary>
        public static string sso_ClientID = "";
        /// <summary>
        /// The redirect uri for the SSO application.
        /// </summary>
        public static string sso_RedirectURI = "";
        /// <summary>
        /// The secutiry key for the SSO application.
        /// </summary>
        public static string sso_SecurityKey = "";
        /// <summary>
        /// The space delimeted string of scopes to request.
        /// </summary>
        public static string sso_Scopes = "";

        public static bool saveConfig()
        {
            if(configFile == null || configFile.Length == 0)
            {
                return false;
            }
            return saveConfig(configFile);
        }
        /// <summary>
        /// Save the EVE-All-API configuration information to an xml file.
        /// </summary>
        /// <param name="file">The file to save to.</param>
        /// <returns>True if successful.</returns>
        public static bool saveConfig(string file)
        {
            // Create the document.
            XmlDocument doc = new XmlDocument();
            doc.AppendChild(getSave(doc));
            // Save document to file.
            doc.Save(file);
            return true;
        }

        private static string configFile = "";
        /// <summary>
        /// Load the configuration from file.
        /// </summary>
        /// <param name="file">The file to load.</param>
        public static void loadConfig(string file)
        {
            configFile = file;
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(file);
            }
            catch (FileNotFoundException)
            {
                // No config file found, use defaults.
                return;
            }
            XmlElement root = doc.DocumentElement;
            if (root == null)
            {
                return;
            }
            loadConfig(root);
        }

        /// <summary>
        /// Create a xml node containing the EVE-All-API configuration inforation.
        /// </summary>
        /// <param name="doc">The document to create the node for.</param>
        /// <returns>The save node.</returns>
        public static XmlNode getSave(XmlDocument doc)
        {
            // Create the config root.
            XmlNode root = doc.CreateElement("EVE-All-API");
            // Save SSO tokens.
            XmlElement tokens = SSO.getTokenNode(doc);
            root.AppendChild(tokens);
            // Save language.
            xmlUtils.newElement(root, "language", language);
            // Save paths.
            if (!string.IsNullOrWhiteSpace(cachePath))
            {
                xmlUtils.newElement(root, "cachePath", cachePath);
            }
            if (!string.IsNullOrWhiteSpace(imagePath))
            {
                xmlUtils.newElement(root, "imagePath", imagePath);
            }
            if (!string.IsNullOrWhiteSpace(typeIconZip))
            {
                xmlUtils.newElement(root, "typeIconZip", typeIconZip);
            }
            if (!string.IsNullOrWhiteSpace(renderZip))
            {
                xmlUtils.newElement(root, "renderZip", renderZip);
            }
            if (!string.IsNullOrWhiteSpace(iconsZip))
            {
                xmlUtils.newElement(root, "iconsZip", iconsZip);
            }
            if (!string.IsNullOrWhiteSpace(apiURL))
            {
                xmlUtils.newElement(root, "apiURL", apiURL);
            }
            if (!string.IsNullOrWhiteSpace(imageURL))
            {
                xmlUtils.newElement(root, "imageURL", imageURL);
            }
            if (!string.IsNullOrWhiteSpace(sdeZip))
            {
                xmlUtils.newElement(root, "sdeZip", sdeZip);
            }
            if (!string.IsNullOrWhiteSpace(sso_ClientID))
            {
                xmlUtils.newElement(root, "ssoClientID", sso_ClientID);
            }
            if (!string.IsNullOrWhiteSpace(sso_RedirectURI))
            {
                xmlUtils.newElement(root, "ssoRedirectURI", sso_RedirectURI);
            }
            if (!string.IsNullOrWhiteSpace(sso_SecurityKey))
            {
                xmlUtils.newElement(root, "ssoSecurityKey", sso_SecurityKey);
            }
            if (!string.IsNullOrWhiteSpace(sso_Scopes))
            {
                xmlUtils.newElement(root, "ssoScopes", sso_Scopes);
            }
            return root;
        }

        /// <summary>
        /// Load the configuration from an XmlElement.
        /// </summary>
        /// <param name="root">The element to load from.</param>
        /// <returns>True if successful.</returns>
        public static bool loadConfig(XmlElement root)
        {
            XmlNode tokenNode = null;
            foreach (XmlNode node in root.ChildNodes)
            {
                if (node.Name == "rowset")
                {
                    XmlNode nameAttr = node.Attributes.GetNamedItem("name");
                    if (nameAttr?.Value == "SSO_Tokens")
                    {
                        // Save the token node to load last, we need to be sure the client_ID and security_key have been loaded.
                        tokenNode = node;
                    }
                    continue;
                }
                switch(node.Name)
                {
                    case "language":
                        language = node.InnerText;
                        break;
                    case "cachePath":
                        cachePath = node.InnerText;
                        break;
                    case "imagePath":
                        imagePath = node.InnerText;
                        break;
                    case "typeIconZip":
                        typeIconZip = node.InnerText;
                        break;
                    case "renderZip":
                        renderZip = node.InnerText;
                        break;
                    case "iconsZip":
                        iconsZip = node.InnerText;
                        break;
                    case "apiURL":
                        apiURL = node.InnerText;
                        break;
                    case "imageURL":
                        imageURL = node.InnerText;
                        break;
                    case "sdeZip":
                        sdeZip = node.InnerText;
                        break;
                    case "ssoClientID":
                        sso_ClientID = node.InnerText;
                        break;
                    case "ssoRedirectURI":
                        sso_RedirectURI = node.InnerText;
                        break;
                    case "ssoSecurityKey":
                        sso_SecurityKey = node.InnerText;
                        break;
                    case "ssoScopes":
                        sso_Scopes = node.InnerText;
                        break;
                }
            }
            // Now we can load the tokens.
            if(tokenNode != null)
            {
                SSO.loadTokens(tokenNode);
            }
            return true;
        }

    }
}
