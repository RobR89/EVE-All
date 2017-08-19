using EVE_All_API.StaticData;
using Newtonsoft.Json;
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
            lock (pilots)
            {
                if (pilots.ContainsKey(_characterID))
                {
                    return pilots[_characterID];
                }
                else
                {
                    return new Pilot(_characterID);
                }
            }
        }

        public enum PilotEvent { CharacterSheetUpdate, AttributesUpdate, ImageLoaded };
        public delegate void PilotHandler(PilotEvent e);
        public event PilotHandler esiUpdate;
        private void issueUpdate(PilotEvent e)
        {
            esiUpdate?.Invoke(e);
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
        public Image pilotImage = null;
        public readonly CharacterSheetPage characterSheet = new CharacterSheetPage();
        public readonly CharacterAttributesPage characterAttributes = new CharacterAttributesPage();

        public class CharacterSheetPage
        {
            // From characterSheet
            public string name;
            public long corporation_ID;
            public DateTime birthday;
            public string gender;
            public int race_id;
            public int bloodLine_id;
            public int ancestry_id;
            public string description;
            public double security_status;
            public DateTime expire;
        }
        public class CharacterAttributesPage
        {
            // From character/$/attributes/
            public int intelligence;
            public int memory;
            public int charisma;
            public int perception;
            public int willpower;
            public int bonus_remaps;
            public DateTime last_remap_date;
            public DateTime accrued_remap_cooldown_date;
            public DateTime expire;
        }

        public long allianceID;
        public long factionID;
        public long homeStationID;
        public int freeSkillPoints;
        public string cloneName;
        public int cloneSkillPoints;
        public int cloneTypeID;
        public DateTime cloneJumpDate = new DateTime(0);
        public DateTime remoteStationDate = new DateTime(0);
        public Dictionary<long, JumpClone> jumpClones = new Dictionary<long, JumpClone>();
        public DateTime jumpActivation = new DateTime(0);
        public DateTime jumpFatigue = new DateTime(0);
        public DateTime jumpLastUpdate = new DateTime(0);
        public double balance;
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
            lock (pilots)
            {
                pilots[_characterID] = this;
            }
            AccessToken token = AccessToken.getFirstTokenForCharacter(characterID);
            characterSheet.name = token?.CharacterName;
        }

        public void loadImage(int size)
        {
            pilotImage = ImageManager.getCharacterImage(characterID, size);
            issueUpdate(PilotEvent.ImageLoaded);
        }

        public void loadCharacterSheet()
        {
            if(DateTime.Now < characterSheet.expire)
            {
                // Page not expired.
                return;
            }
            string url = "characters/" + characterID.ToString() + "/";
            JSON.ESIResponse resp = JSON.getESIPage(url, null, null);
            if (resp?.code == System.Net.HttpStatusCode.OK)
            {
                JsonConvert.PopulateObject(resp.content, characterSheet);
                characterSheet.expire = resp.expires;
                issueUpdate(PilotEvent.CharacterSheetUpdate);
            }
        }

        public void loadAttributes()
        {
            if (DateTime.Now < characterAttributes.expire)
            {
                // Page not expired.
                return;
            }
            AccessToken token = AccessToken.getTokenForCharacterWithScope(characterID, "esi-skills.read_skills.v1");
            string url = "characters/" + characterID.ToString() + "/attributes/";
            JSON.ESIResponse resp = JSON.getESIPage(url, null, token);
            if (resp?.code == System.Net.HttpStatusCode.OK)
            {
                JsonConvert.PopulateObject(resp.content, characterAttributes);
                characterAttributes.expire = resp.expires;
                issueUpdate(PilotEvent.AttributesUpdate);
            }
        }
    }
}
