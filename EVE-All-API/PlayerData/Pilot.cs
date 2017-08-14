using EVE_All_API.StaticData;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EVE_All_API
{
    public class Pilot
    {
        private static Dictionary<long, Pilot> pilots = new Dictionary<long, Pilot>();
        public static Pilot getPilot(long _characterID)
        {
            Pilot pilot = null;
            if(pilots.ContainsKey(_characterID))
            {
                pilot = pilots[_characterID];
            }
            else
            {
                pilot = new Pilot(_characterID);
            }
            return pilot;
        }

        public class JumpClone
        {
            public long jumpCloneID;
            public int typeID;
            public long locationID;
            public string cloneName;
            public List<int> implantTypeIDs = new List<int>();
        }

        public class Skill
        {
            public int typeID;
            public int level;
            public int skillPoints;
        }

        public readonly long characterID;
        public string characterName;
        public List<APIKey> keys = null;
        public long corporationID;
        public long allianceID;
        public long factionID;
        // From characterSheet
        public long homeStationID;
        public DateTime birthday = new DateTime(0);
        public string race;
        public int bloodLineID;
        public string bloodLine;
        public int ancestryID;
        public string ancestry;
        public bool male;
        public int freeSkillPoints;
        public int freeRespecs;
        public string cloneName;
        public int cloneSkillPoints;
        public int cloneTypeID;
        public DateTime cloneJumpDate = new DateTime(0);
        public DateTime lastRespecDate = new DateTime(0);
        public DateTime lastTimedRespec = new DateTime(0);
        public DateTime remoteStationDate = new DateTime(0);
        public Dictionary<long, JumpClone> jumpClones = new Dictionary<long, JumpClone>();
        public DateTime jumpActivation = new DateTime(0);
        public DateTime jumpFatigue = new DateTime(0);
        public DateTime jumpLastUpdate = new DateTime(0);
        public double balance;
        public int intelligence;
        public int memory;
        public int charisma;
        public int perception;
        public int willpower;
        public int effectiveIntelligence;
        public int effectiveMemory;
        public int effectiveCharisma;
        public int effectivePerception;
        public int effectiveWillpower;
        public List<int> implants = new List<int>();
        public Dictionary<int, Skill> skills = new Dictionary<int, Skill>();
        public Dictionary<long, string> corporationRoles = new Dictionary<long, string>();
        public Dictionary<long, string> corporationRolesAtHQ = new Dictionary<long, string>();
        public Dictionary<long, string> corporationRolesAtBase = new Dictionary<long, string>();
        public Dictionary<long, string> corporationRolesAtOther = new Dictionary<long, string>();
        public Dictionary<long, string> corporationTitles = new Dictionary<long, string>();
        public List<int> certificates = new List<int>();

        private Pilot(long _characterID)
        {
            characterID = _characterID;
            pilots[_characterID] = this;
        }

        public Image getImage(int size)
        {
            return ImageManager.getCharacterImage(characterID, size);
        }

/*
SSO scope                      Access mask
characterWalletRead            0x00001 | 0x0200000  | 0x0400000
characterAssetsRead            0x00002 | 0x08000000
characterCalendarRead          0x00004 | 0x0100000
characterCharacterSheet        0x00008
characterContactsRead	       0x00010 | 0x020      | 0x080000
characterFactionalWarfareRead  0x00040
characterIndustryJobsRead      0x00080
characterKillsRead             0x00100
characterMailRead              0x00200 | 0x0400 | 0x0800
characterMarketOrdersRead      0x01000
characterMedalsRead            0x02000
characterNotificationsRead     0x04000 | 0x08000
characterResearchRead          0x10000
characterSkillsRead            0x20000 | 0x40000 | 0x40000000
characterAccountRead           0x2000000
characterContractsRead         0x4000000
characterBookmarksRead         0x10000000
characterChatChannelsRead      0x20000000
characterClonesRead            0x80000000
*/

        public bool loadCharacterSheet()
        {
            APIKey key = null;
            if(keys != null)
            {
                // Look for a key.
                foreach(APIKey k in keys)
                {
                    if (k.hasMask(0x08))
                    {
                        key = k;
                        break;
                    }
                }
            }
            XmlNode result = xmlManager.getPage("char/CharacterSheet.xml.aspx?characterID=" + characterID, key);
            if (result == null)
            {
                return false;
            }
            Corporation corp = null;
            Alliance alliance = null;
            Faction faction = null;
            string corpName = null;
            string aliName = null;
            string factName = null;
            // Get current list of cloneIDs.
            List<long> oldClones = jumpClones.Keys.ToList();
            // Get old clone implants.
            Dictionary<long, List<int>> oldCloneImplants = new Dictionary<long, List<int>>();
            foreach (JumpClone jumpClone in jumpClones.Values)
            {
                oldCloneImplants[jumpClone.jumpCloneID] = new List<int>(jumpClone.implantTypeIDs);
            }
            foreach (XmlNode node in result.ChildNodes)
            {
                switch (node.Name)
                {
                    case "name":
                        characterName = node.InnerText;
                        break;
                    case "homeStationID":
                        homeStationID = Int64.Parse(node.InnerText);
                        break;
                    case "DoB":
                        birthday = DateTime.Parse(node.InnerText);
                        birthday = DateTime.SpecifyKind(birthday, DateTimeKind.Utc);
                        break;
                    case "race":
                        race = node.InnerText;
                        break;
                    case "bloodLineID":
                        bloodLineID = Int32.Parse(node.InnerText);
                        break;
                    case "bloodLine":
                        bloodLine = node.InnerText;
                        break;
                    case "ancestryID":
                        ancestryID = Int32.Parse(node.InnerText);
                        break;
                    case "ancestry":
                        ancestry = node.InnerText;
                        break;
                    case "gender":
                        male = node.InnerText == "Male";
                        break;
                    case "corporationID":
                        corporationID = Int32.Parse(node.InnerText);
                        corp = Corporation.getCorporation(corporationID);
                        break;
                    case "corporationName":
                        corpName = node.InnerText;
                        break;
                    case "allianceID":
                        allianceID = Int32.Parse(node.InnerText);
                        alliance = Alliance.getAlliance(allianceID);
                        break;
                    case "allianceName":
                        aliName = node.InnerText;
                        break;
                    case "factionID":
                        factionID = Int32.Parse(node.InnerText);
                        faction = Faction.getFaction(factionID);
                        break;
                    case "factionName":
                        factName = node.InnerText;
                        break;
                    case "freeSkillPoints":
                        freeSkillPoints = Int32.Parse(node.InnerText);
                        break;
                    case "freeRespecs":
                        freeRespecs = Int32.Parse(node.InnerText);
                        break;
                    case "cloneName":
                        cloneName = node.InnerText;
                        break;
                    case "cloneSkillPoints":
                        cloneSkillPoints = Int32.Parse(node.InnerText);
                        break;
                    case "cloneTypeID":
                        cloneTypeID = Int32.Parse(node.InnerText);
                        break;
                    case "cloneJumpDate":
                        cloneJumpDate = DateTime.Parse(node.InnerText);
                        break;
                    case "lastRespecDate":
                        lastRespecDate = DateTime.Parse(node.InnerText);
                        break;
                    case "lastTimedRespec":
                        lastTimedRespec = DateTime.Parse(node.InnerText);
                        break;
                    case "remoteStationDate":
                        remoteStationDate = DateTime.Parse(node.InnerText);
                        break;
                    case "jumpActivation":
                        remoteStationDate = DateTime.Parse(node.InnerText);
                        break;
                    case "jumpFatigue":
                        remoteStationDate = DateTime.Parse(node.InnerText);
                        break;
                    case "jumpLastUpdate":
                        remoteStationDate = DateTime.Parse(node.InnerText);
                        break;
                    case "balance":
                        balance = Double.Parse(node.InnerText);
                        break;
                    case "attributes":
                        foreach (XmlNode attributeNode in node.ChildNodes)
                        {
                            switch(attributeNode.Name)
                            {
                                case "intelligence":
                                    intelligence = Int32.Parse(attributeNode.InnerText);
                                    break;
                                case "memory":
                                    memory = Int32.Parse(attributeNode.InnerText);
                                    break;
                                case "charisma":
                                    charisma = Int32.Parse(attributeNode.InnerText);
                                    break;
                                case "perception":
                                    perception = Int32.Parse(attributeNode.InnerText);
                                    break;
                                case "willpower":
                                    willpower = Int32.Parse(attributeNode.InnerText);
                                    break;
                            }
                        }
                        break;
                    case "rowset":
                        string[] cloneColumns = { "jumpCloneID", "typeID", "locationID", "cloneName" };
                        List<Dictionary<string, string>> rows;
                        if (xmlUtils.parseRowSet(node, "jumpClones", out rows, cloneColumns))
                        {
                            foreach (Dictionary<string, string> row in rows)
                            {
                                // Get or create the clone.
                                long cloneID = Int64.Parse(row["jumpCloneID"]);
                                if (!jumpClones.ContainsKey(cloneID))
                                {
                                    jumpClones[cloneID] = new JumpClone();
                                    oldClones.Remove(cloneID);
                                }
                                // Update the clone.
                                JumpClone clone = jumpClones[cloneID];
                                clone.jumpCloneID = cloneID;
                                clone.typeID = Int32.Parse(row["typeID"]);
                                clone.cloneName = row["cloneName"];
                                clone.locationID = Int64.Parse(row["locationID"]);
                            }
                        }
                        string[] cloneImplantColumns = { "jumpCloneID", "typeID", "typeName" };
                        if (xmlUtils.parseRowSet(node, "jumpCloneImplants", out rows, cloneImplantColumns))
                        {
                            foreach (Dictionary<string, string> row in rows)
                            {
                                // Get or create the clone.
                                long cloneID = Int64.Parse(row["jumpCloneID"]);
                                if (!jumpClones.ContainsKey(cloneID))
                                {
                                    jumpClones[cloneID] = new JumpClone();
                                }
                                // Get or update the implant type.
                                int typeID = Int32.Parse(row["typeID"]);
                                // Update the clone.
                                JumpClone clone = jumpClones[cloneID];
                                if (!clone.implantTypeIDs.Contains(typeID))
                                {
                                    clone.implantTypeIDs.Add(typeID);
                                }
                                // Remove implant from old list because it's current.
                                if (oldCloneImplants.ContainsKey(cloneID))
                                {
                                    oldCloneImplants[cloneID].Remove(typeID);
                                }
                            }
                        }
                        string[] implantColumns = { "typeID", "typeName" };
                        if (xmlUtils.parseRowSet(node, "implants", out rows, implantColumns))
                        {
                            implants.Clear();
                            foreach (Dictionary<string, string> row in rows)
                            {
                                // Get or update the implant type.
                                int typeID = Int32.Parse(row["typeID"]);
                                if (!implants.Contains(typeID))
                                {
                                    implants.Add(typeID);
                                }
                            }
                        }
                        string[] skillColumns = { "typeID", "skillpoints", "level", "published" };
                        if (xmlUtils.parseRowSet(node, "skills", out rows, skillColumns))
                        {
                            foreach (Dictionary<string, string> row in rows)
                            {
                                // Get the skill type.
                                int typeID = Int32.Parse(row["typeID"]);
                                // Update the skill.
                                if (!skills.ContainsKey(typeID))
                                {
                                    skills[typeID] = new Skill();
                                }
                                Skill skill = skills[typeID];
                                skill.level = Int32.Parse(row["level"]);
                                skill.skillPoints = Int32.Parse(row["skillpoints"]);
                            }
                        }
                        string[] roleColumns = { "roleID", "roleName" };
                        if (xmlUtils.parseRowSet(node, "corporationRoles", out rows, roleColumns))
                        {
                            corporationRoles.Clear();
                            foreach (Dictionary<string, string> row in rows)
                            {
                                // Get or update the implant type.
                                int roleID = Int32.Parse(row["roleID"]);
                                corporationRoles[roleID] = row["roleName"];
                            }
                        }
                        if (xmlUtils.parseRowSet(node, "corporationRolesAtHQ", out rows, roleColumns))
                        {
                            corporationRolesAtHQ.Clear();
                            foreach (Dictionary<string, string> row in rows)
                            {
                                // Get or update the implant type.
                                int roleID = Int32.Parse(row["roleID"]);
                                corporationRolesAtHQ[roleID] = row["roleName"];
                            }
                        }
                        if (xmlUtils.parseRowSet(node, "corporationRolesAtBase", out rows, roleColumns))
                        {
                            corporationRolesAtBase.Clear();
                            foreach (Dictionary<string, string> row in rows)
                            {
                                // Get or update the implant type.
                                int roleID = Int32.Parse(row["roleID"]);
                                corporationRolesAtBase[roleID] = row["roleName"];
                            }
                        }
                        if (xmlUtils.parseRowSet(node, "corporationRolesAtOther", out rows, roleColumns))
                        {
                            corporationRolesAtOther.Clear();
                            foreach (Dictionary<string, string> row in rows)
                            {
                                // Get or update the implant type.
                                int roleID = Int32.Parse(row["roleID"]);
                                corporationRolesAtOther[roleID] = row["roleName"];
                            }
                        }
                        string[] corporationColumns = { "titleID", "titleName" };
                        if (xmlUtils.parseRowSet(node, "corporationTitles", out rows, corporationColumns))
                        {
                            corporationTitles.Clear();
                            foreach (Dictionary<string, string> row in rows)
                            {
                                // Get or update the implant type.
                                int titleID = Int32.Parse(row["titleID"]);
                                corporationTitles[titleID] = row["titleName"];
                            }
                        }
                        string[] certificateColumns = { "certificateID" };
                        if (xmlUtils.parseRowSet(node, "certificates", out rows, certificateColumns))
                        {
                            certificates.Clear();
                            foreach (Dictionary<string, string> row in rows)
                            {
                                // Get or update the implant type.
                                int certID = Int32.Parse(row["certificateID"]);
                                certificates.Add(certID);
                            }
                        }
                        break;
                }
            }
            // Remove old clones.
            foreach (long cloneID in oldClones)
            {
                // Remove an old clone.
                jumpClones.Remove(cloneID);
            }
            // Remove old clone implants.
            foreach(long cloneID in oldCloneImplants.Keys)
            {
                if(jumpClones.ContainsKey(cloneID))
                {
                    foreach(int typeID in oldCloneImplants[cloneID])
                    {
                        jumpClones[cloneID].implantTypeIDs.Remove(typeID);
                    }
                }
            }
            // Update corporation name.
            if (corp != null && corpName != null)
            {
                corp.corporationName = corpName;
            }
            // Update alliance name.
            if (alliance != null && aliName != null)
            {
                alliance.allianceName = aliName;
            }
            // Update faction name.
            if (faction != null && factName != null)
            {
                faction.factionName = factName;
            }
            // Update effective attributes.
            effectiveIntelligence = intelligence;
            effectiveMemory = memory;
            effectivePerception = perception;
            effectiveWillpower = willpower;
            effectiveCharisma = charisma;
            foreach (int implant in implants)
            {
                // 164 Charisma 170 + 175
                // 168 Willpower 171 + 179
                // 167 Perception 172 + 178
                // 166 Memory 173 + 177
                // 165 Intelligence 174 + 176
                List<DgmTypeAttribute> attributes = DgmTypeAttribute.getDgmTypeAttribute(implant);
                foreach (DgmTypeAttribute attr in attributes)
                {
                    if (attr == null)
                    {
                        continue;
                    }
                    if (attr.attributeID == 174 || attr.attributeID == 176)
                    {
                        effectiveIntelligence += (int)attr.valueInt;
                        effectiveIntelligence += (int)attr.valueFloat;
                    }
                    if (attr.attributeID == 173 || attr.attributeID == 177)
                    {
                        effectiveMemory += (int)attr.valueInt;
                        effectiveMemory += (int)attr.valueFloat;
                    }
                    if (attr.attributeID == 172 || attr.attributeID == 178)
                    {
                        effectivePerception += (int)attr.valueInt;
                        effectivePerception += (int)attr.valueFloat;
                    }
                    if (attr.attributeID == 171 || attr.attributeID == 179)
                    {
                        effectiveWillpower += (int)attr.valueInt;
                        effectiveWillpower += (int)attr.valueFloat;
                    }
                    if (attr.attributeID == 170 || attr.attributeID == 175)
                    {
                        effectiveCharisma += (int)attr.valueInt;
                        effectiveCharisma += (int)attr.valueFloat;
                    }
                }
            }
            return true;
        }

    }
}
