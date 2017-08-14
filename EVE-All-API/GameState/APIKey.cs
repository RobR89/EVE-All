using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace EVE_All_API
{
    public class APIKey
    {
        private static Dictionary<long, APIKey> keys = new Dictionary<long, APIKey>();

        /// <summary>
        /// Check if the key alrady exists.
        /// </summary>
        /// <param name="_keyID">The keyID of the key.</param>
        /// <returns>True if the key exists.</returns>
        public static bool haveKey(long _keyID)
        {
            if (keys.ContainsKey(_keyID))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Add an api key to the list of keys or update an existing key.
        /// </summary>
        /// <param name="_keyID">The keyID of the key to add or update.</param>
        /// <param name="_vCode">The verification code to add.</param>
        /// <returns>The new or updated API key.</returns>
        public static APIKey getKey(long _keyID, string _vCode)
        {
            APIKey key;
            if (keys.ContainsKey(_keyID))
            {
                key = keys[_keyID];
                key.vCode = _vCode;
                key.updateKey();
            }
            else
            {
                key = new APIKey(_keyID, _vCode);
                keys[_keyID] = key;
            }
            return key;
        }

        /// <summary>
        /// Get an API key.
        /// </summary>
        /// <param name="_keyID">The keyID of the key to get.</param>
        /// <returns>The API key or null.</returns>
        public static APIKey getKey(long _keyID)
        {
            if (keys.ContainsKey(_keyID))
            {
                return keys[_keyID];
            }
            return null;
        }

        public static List<long> getAllKeyIDs()
        {
            return new List<long>(keys.Keys.ToArray());
        }

        /// <summary>
        /// Create an XML element that holds the API keys.
        /// </summary>
        /// <param name="doc">The XML document to create the keys for.</param>
        /// <returns>The new XML element.</returns>
        public static XmlElement getSaveKeysNode(XmlDocument doc)
        {
            XmlElement rowset = doc.CreateElement("rowset");
            XmlAttribute attribute = doc.CreateAttribute("name");
            attribute.Value = "keys";
            rowset.Attributes.Append(attribute);
            foreach (APIKey key in keys.Values)
            {
                // create the row.
                XmlElement row = doc.CreateElement("row");
                rowset.AppendChild(row);
                // Add the keyName.
                xmlUtils.newAttribute(row, "keyName", key.keyName);
                // Add the keyID.
                xmlUtils.newAttribute(row, "keyID", key.keyID.ToString());
                // Add the vCode.
                xmlUtils.newAttribute(row, "vCode", key.vCode);
                // Add the vCode.
                xmlUtils.newAttribute(row, "keyActive", key.keyActive.ToString());
                // Add ignore list.
                string characters = "";
                foreach (long charID in key.ignore)
                {
                    if (characters.Length > 0)
                    {
                        characters += ",";
                    }
                    characters += charID.ToString();
                }
                xmlUtils.newAttribute(row, "ignore", characters);
            }
            return rowset;
        }

        /// <summary>
        /// Load the API keys from a rowset.
        /// </summary>
        /// <param name="rowset">The rowset to load the keys from.</param>
        public static void loadKeys(XmlNode rowset)
        {
            List<Dictionary<string, string>> rows;
            string[] columns = { "keyName", "keyID", "vCode", "ignore", "keyActive" };
            if (xmlUtils.parseRowSet(rowset, "keys", out rows, columns))
            {
                foreach (Dictionary<string, string> row in rows)
                {
                    APIKey key = getKey(Int64.Parse(row["keyID"]), row["vCode"]);
                    key.keyName = row["keyName"];
                    key.ignore.Clear();
                    if (row["ignore"].Length > 0)
                    {
                        key.ignore = row["ignore"].Split(',').Select(long.Parse).ToList();
                        foreach(long ignore in key.ignore)
                        {
                            key.useCharacters.Remove(ignore);
                        }
                    }
                    key.keyActive = bool.Parse(row["keyActive"]);
                    key.updateKey();
                }
            }
        }

        public static List<long> getAllCharacterIDs()
        {
            List<long> characterIDs = new List<long>();
            foreach (int keyID in getAllKeyIDs())
            {
                APIKey key = APIKey.getKey(keyID);
                foreach(long charID in key.useCharacters)
                {
                    if(!characterIDs.Contains(charID))
                    {
                        characterIDs.Add(charID);
                    }
                }
            }
            return characterIDs;
        }

    public string keyName;
        public readonly long keyID;
        public string vCode;
        public List<long> useCharacters = new List<long>();
        public List<long> ignore = new List<long>();
        public bool keyActive;
        // From account/APIKeyInfo
        public int accessMask;
        public DateTime expires;
        public string keyType;
        public List<Pilot> pilots = new List<Pilot>();
        public List<Corporation> corporations = new List<Corporation>();
        // From account/AccountStatus valid for characters only.
        public DateTime paidUntil;
        public DateTime createDate;
        public int logonCount;
        public int logonMinutes;
        public List<DateTime> multiCharacterTraining = new List<DateTime>();

        private APIKey(long _keyID, string _vCode)
        {
            keyID = _keyID;
            vCode = _vCode;
            updateKey();
        }

        public bool hasMask(long mask)
        {
            return (accessMask & mask) == mask;
        }

        public bool updateKey()
        {
            if(!keyActive)
            {
                return false;
            }
            if (!updateKeyInfo())
            {
                return false;
            }
            if (keyType != "Corporation")
            {
                if (!updateAccountInfo())
                {
                    return false;
                }
            }
            return true;
        }

        private bool updateKeyInfo()
        {
            XmlNode result = xmlManager.getPage("account/APIKeyInfo.xml.aspx", this);
            if (result == null)
            {
                return false;
            }
            foreach (XmlNode node in result.ChildNodes)
            {
                if (node.Name == "key")
                {
                    foreach (XmlNode attr in node.Attributes)
                    {
                        switch (attr.Name)
                        {
                            case "accessMask":
                                Int32.TryParse(attr.Value, out accessMask);
                                break;
                            case "type":
                                keyType = attr.Value;
                                break;
                            case "expires":
                                if (attr.Value.Length > 0)
                                {
                                    expires = DateTimeOffset.Parse(node.Value).UtcDateTime;
                                }
                                break;
                        }
                    }
                    foreach (XmlNode keyNode in node.ChildNodes)
                    {
                        List<Dictionary<string, string>> rows;
                        string[] columns = { "characterID", "characterName", "corporationID", "corporationName", "allianceID", "allianceName", "factionID", "factionName" };
                        if (xmlUtils.parseRowSet(keyNode, "characters", out rows, columns))
                        {
                            // Remove pilot keys in case we no longer have them.
                            foreach (Pilot p in pilots)
                            {
                                p.keys.Remove(this);
                            }
                            // Remove corporation keys in case we no longer have them.
                            foreach (Corporation c in corporations)
                            {
                                c.keys.Remove(this);
                            }
                            foreach (Dictionary<string, string> row in rows)
                            {
                                Int64 characterID = Int64.Parse(row["characterID"]);
                                if(ignore.Contains(characterID))
                                {
                                    useCharacters.Remove(characterID);
                                }
                                else if(!useCharacters.Contains(characterID))
                                {
                                    useCharacters.Add(characterID);
                                }
                                string characterName = row["characterName"];
                                Int64 corporationID = Int64.Parse(row["corporationID"]);
                                string corporationName = row["corporationName"];
                                Int64 allianceID = Int64.Parse(row["allianceID"]);
                                string allianceName = row["allianceName"];
                                Int64 factionID = Int64.Parse(row["factionID"]);
                                string factionName = row["factionName"];
                                // Add character to the list.
                                Pilot pilot = Pilot.getPilot(characterID);
                                pilot.characterName = characterName;
                                pilot.corporationID = corporationID;
                                pilot.allianceID = allianceID;
                                pilot.factionID = factionID;
                                if(keyType == "Account" || keyType == "Character")
                                {
                                    if(pilot.keys == null)
                                    {
                                        pilot.keys = new List<APIKey>();
                                    }
                                    if(!pilot.keys.Contains(this))
                                    {
                                        pilot.keys.Add(this);
                                    }
                                    pilots.Add(pilot);
                                }
                                // Update corporation name.
                                Corporation corp = Corporation.getCorporation(corporationID);
                                corp.corporationName = corporationName;
                                if (keyType == "Corporation")
                                {
                                    if (corp.keys == null)
                                    {
                                        corp.keys = new List<APIKey>();
                                    }
                                    if (!corp.keys.Contains(this))
                                    {
                                        corp.keys.Add(this);
                                    }
                                    corporations.Add(corp);
                                }
                                // Update alliance name.
                                Alliance alliance = Alliance.getAlliance(allianceID);
                                if (alliance != null)
                                {
                                    alliance.allianceName = allianceName;
                                }
                                // Update faction name.
                                Faction faction = Faction.getFaction(factionID);
                                if(faction != null)
                                {
                                    faction.factionName = factionName;
                                }
                            }
                            continue;
                        }
                    }
                }
            }
            return true;
        }

        private bool updateAccountInfo()
        {
            XmlNode result = xmlManager.getPage("account/AccountStatus.xml.aspx", this);
            if (result == null)
            {
                return false;
            }
            foreach (XmlNode node in result.ChildNodes)
            {
                switch(node.Name)
                {
                    case "paidUntil":
                        paidUntil = DateTimeOffset.Parse(node.InnerText).UtcDateTime;
                        break;
                    case "createDate":
                        createDate = DateTimeOffset.Parse(node.InnerText).UtcDateTime;
                        break;
                    case "logonCount":
                        Int32.TryParse(node.InnerText, out logonCount);
                        break;
                    case "logonMinutes":
                        Int32.TryParse(node.InnerText, out logonMinutes);
                        break;
                    case "rowset":
                        string[] columns = { "trainingEnd" };
                        List<Dictionary<string, string>> rows;
                        if (xmlUtils.parseRowSet(node, "multiCharacterTraining", out rows, columns))
                        {
                            foreach (Dictionary<string, string> row in rows)
                            {
                                multiCharacterTraining.Add(DateTimeOffset.Parse(row["trainingEnd"]).UtcDateTime);
                            }
                        }
                        break;
                }
            }
            return true;
        }

    }
}
