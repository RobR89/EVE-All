using System.Threading.Tasks;
using System.Windows.Forms;
using EVE_All_API;
using EVE_All_API.StaticData;

namespace EVE_All.Tabs
{
    public partial class PilotTab : UserControl
    {
        private Pilot pilot;

        public PilotTab(Pilot nPilot)
        {
            pilot = nPilot;
            // Initialize controls.
            InitializeComponent();
            Dock = DockStyle.Fill;
            // Update pilot info.
            pilot.EsiUpdate += Pilot_EsiUpdate;
            RequestUpdate();
        }

        private void Pilot_EsiUpdate(Pilot p, Pilot.PilotEvent e)
        {
            if(InvokeRequired)
            {
                // Insure this is called in a GUI friendly thread.
                Invoke((MethodInvoker)delegate{ Pilot_EsiUpdate(p, e); });
                return;
            }
            switch (e)
            {
                case Pilot.PilotEvent.CharacterSheetUpdate:
                    UpdateCharacterSheet();
                    break;
                case Pilot.PilotEvent.AttributesUpdate:
                    UpdateAttributes();
                    break;
                case Pilot.PilotEvent.ImageLoaded:
                    // Get pilot image.
                    pilotImage.Image = pilot.pilotImage;
                    break;
            }
        }

        public void RequestUpdate()
        {
            // Update pilot info.
            pilot.characterSheet.ScheduleRefresh();
            pilot.characterAttributes.ScheduleRefresh();
            new Task(() => pilot.LoadImage(128)).Start();

            //balanceLabel.Text = "Balance: " + pilot.balance.ToString("N") + " ISK";
            //// Update membership info.
            //Corporation corp = Corporation.getCorporation(pilot.corporationID);
            //corporationLabel.Text = "Corporation: " + corp?.corporationName;
            //Alliance alliance = Alliance.getAlliance(pilot.allianceID);
            //allianceLabel.Text = "Alliance: " + alliance?.allianceName;
            //Faction faction = Faction.getFaction(pilot.factionID);
            //factionLabel.Text = "Faction: " + faction?.factionName;
            //// Update clone info.
            //cloneNameLabel.Text = pilot.cloneName;
            //InvType type = InvType.getType(pilot.cloneTypeID);
            //cloneTypeLabel.Text = type.name;
            //cloneSkillPointsLabel.Text = pilot.cloneSkillPoints.ToString("N");
        }

        private void UpdateCharacterSheet()
        {
            nameLabel.Text = pilot.characterSheet.name;
            // Populate the race, bloodline, and ancestry.
            ChrRace race = ChrRace.GetRace(pilot.characterSheet.race_id);
            ChrBloodline bloodLine = ChrBloodline.GetBloodline(pilot.characterSheet.bloodLine_id);
            ChrAncestry ancestry = ChrAncestry.GetAncestry(pilot.characterSheet.ancestry_id);
            raceLabel.Text = pilot.characterSheet.gender + " - " + race?.raceName + " - " + bloodLine?.bloodlineName + " - " + ancestry?.ancestryName;
            // Update the birthday.
            BirthdayLabel.Text = "Birthday: " + pilot.characterSheet.birthday.ToString();
            // Update security status.
            securityLabel.Text = "SecurityStatus: " + pilot.characterSheet.security_status.ToString();
        }

        private void UpdateAttributes()
        {
            // Update attributes.
            intelligenceLabel.Text = "Intelligence: " + pilot.characterAttributes.intelligence.ToString();
            memoryLabel.Text = "Memory: " + pilot.characterAttributes.memory.ToString();
            perceptionLabel.Text = "Perception: " + pilot.characterAttributes.perception.ToString();
            willpowerLabel.Text = "Willpower: " + pilot.characterAttributes.willpower.ToString();
            charismaLabel.Text = "Charisma: " + pilot.characterAttributes.charisma.ToString();
            lastRemapLabel.Text = "Last remap: " + pilot.characterAttributes.last_remap_date.ToString();
            nextRemapLabel.Text = "Next remap: " + pilot.characterAttributes.accrued_remap_cooldown_date.ToString();
            BonusRemapLabel.Text = "Bonus remap: " + pilot.characterAttributes.bonus_remaps.ToString();
        }

    }
}
