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

namespace EVE_All.Tabs
{
    public partial class PilotListTab : UserControl
    {
        private Dictionary<long, TabPage> pilotTabReference = new Dictionary<long, TabPage>();

        public PilotListTab()
        {
            InitializeComponent();

            // Add characters.
            List<AccessToken> tokens = AccessToken.GetAccessTokens();
            foreach (AccessToken token in tokens)
            {
                if(pilotTabReference.ContainsKey(token.CharacterID))
                {
                    continue;
                }
                Pilot pilot = Pilot.GetPilot(token.CharacterID);
                // Create the pilot tab.
                PilotTab pTab = new PilotTab(pilot);
                // Create the tab container.
                TabPage pilotTab = new TabPage(pilot.characterSheet.name);
                pTab.Dock = DockStyle.Fill;
                // Save the reference.
                pilotTabReference[token.CharacterID] = pilotTab;
                // Add tab.
                pilotTab.Controls.Add(pTab);
                pilotsTabs.TabPages.Add(pilotTab);
            }

            // Add callback for new tokens
            AccessToken.AccessTokenAdded += AccessToken_AccessTokenAdded;
        }

        private void AccessToken_AccessTokenAdded(AccessToken token)
        {
            if(InvokeRequired)
            {
                // Insure this is called in a GUI friendly thread.
                Invoke((MethodInvoker)delegate { AccessToken_AccessTokenAdded(token); });
                return;
            }
            if (pilotTabReference.ContainsKey(token.CharacterID))
            {
                return;
            }
            Pilot pilot = Pilot.GetPilot(token.CharacterID);
            // Create the pilot tab.
            PilotTab pTab = new PilotTab(pilot);
            // Create the tab container.
            TabPage pilotTab = new TabPage(pilot.characterSheet.name);
            pTab.Dock = DockStyle.Fill;
            // Save the reference.
            pilotTabReference[token.CharacterID] = pilotTab;
            // Add tab.
            pilotTab.Controls.Add(pTab);
            pilotsTabs.TabPages.Add(pilotTab);
        }
    }
}
