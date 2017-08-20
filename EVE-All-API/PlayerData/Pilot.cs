using EVE_All_API.ESI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace EVE_All_API
{
    public class Pilot
    {
        private static Dictionary<long, Pilot> pilots = new Dictionary<long, Pilot>();
        public static Pilot GetPilot(long _characterID)
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
        public event PilotHandler EsiUpdate;
        private void IssueUpdate(PilotEvent e)
        {
            EsiUpdate?.Invoke(e);
        }

#region Depreciated
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
#endregion

        public readonly long characterID;
        public Image pilotImage = null;
        public readonly CharacterSheetPage characterSheet = new CharacterSheetPage();
        public readonly CharacterAttributesPage characterAttributes = new CharacterAttributesPage();

        private Pilot(long _characterID)
        {
            characterID = _characterID;
            lock (pilots)
            {
                pilots[_characterID] = this;
            }
            // Attempt to find a name.
            AccessToken token = AccessToken.GetFirstTokenForCharacter(characterID);
            characterSheet.name = token?.CharacterName;

            // Set up ESI pages.
            // Character Sheet
            characterSheet.url = "characters/" + characterID.ToString() + "/";
            characterSheet.autoUpdateAction = new Action<Task>( _ => LoadCharacterSheet());
            // Attributes.
            characterAttributes.url = "characters/" + characterID.ToString() + "/attributes/";
            characterAttributes.autoUpdateAction = new Action<Task>(_ => LoadAttributes());
        }

        public class CharacterSheetPage : ESIPage
        {
            // From characterSheet/$character_ID
            public string name;
            public long corporation_ID;
            public DateTime birthday;
            public string gender;
            public int race_id;
            public int bloodLine_id;
            public int ancestry_id;
            public string description;
            public double security_status;
        }

        public class CharacterAttributesPage : ESIPage
        {
            public  CharacterAttributesPage()
            {
                scope = "esi-skills.read_skills.v1";
            }
            // From character/$character_ID/attributes/
            public int intelligence;
            public int memory;
            public int charisma;
            public int perception;
            public int willpower;
            public int bonus_remaps;
            public DateTime last_remap_date;
            public DateTime accrued_remap_cooldown_date;
        }

        public void LoadImage(int size)
        {
            pilotImage = ImageManager.getCharacterImage(characterID, size);
            IssueUpdate(PilotEvent.ImageLoaded);
        }

        public void LoadCharacterSheet()
        {
            JSON.JSONResponse resp = characterSheet.GetPage();
            if (resp?.httpCode == System.Net.HttpStatusCode.OK)
            {
                IssueUpdate(PilotEvent.CharacterSheetUpdate);
            }
        }

        public void LoadAttributes()
        {
            JSON.JSONResponse resp = characterAttributes.GetPage(characterID);
            if (resp?.httpCode == System.Net.HttpStatusCode.OK)
            {
                IssueUpdate(PilotEvent.AttributesUpdate);
            }
        }

    }
}
