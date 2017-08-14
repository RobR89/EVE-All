using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
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
            updateInfo();
        }

        public void updateInfo()
        {
            // Update pilot info.
            pilot.loadCharacterSheet();
            // Get pilot image.
            pilotImage.Image = pilot.getImage(128);
            // Set unchanging info.
            nameLabel.Text = pilot.characterName;
            raceLabel.Text = (pilot.male ? "Male" : "Female") + " - " + pilot.race + " - " + pilot.bloodLine + " - " + pilot.ancestry;
            BirthdayLabel.Text = "Birthday: " + pilot.birthday.ToLocalTime().ToString();
            // Update changable info.
            balanceLabel.Text = "Balance: " + pilot.balance.ToString("N") + " ISK";
            // Update membership info.
            Corporation corp = Corporation.getCorporation(pilot.corporationID);
            corporationLabel.Text = "Corporation: " + corp?.corporationName;
            Alliance alliance = Alliance.getAlliance(pilot.allianceID);
            allianceLabel.Text = "Alliance: " + alliance?.allianceName;
            Faction faction = Faction.getFaction(pilot.factionID);
            factionLabel.Text = "Faction: " + faction?.factionName;
            // Update clone info.
            cloneNameLabel.Text = pilot.cloneName;
            InvType type = InvType.getType(pilot.cloneTypeID);
            cloneTypeLabel.Text = type.name;
            cloneSkillPointsLabel.Text = pilot.cloneSkillPoints.ToString("N");
            // Update attributes.
            intelligenceLabel.Text = "Intelligence: " + pilot.effectiveIntelligence.ToString();
            memoryLabel.Text = "Memory: " + pilot.effectiveMemory.ToString();
            perceptionLabel.Text = "Perception: " + pilot.effectivePerception.ToString();
            willpowerLabel.Text = "Willpower: " + pilot.effectiveWillpower.ToString();
            charismaLabel.Text = "Charisma: " + pilot.effectiveCharisma.ToString();
        }

    }
}
