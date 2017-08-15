using EVE_All.KeyManagement;
using EVE_All.Tabs;
using EVE_All_API;
using EVE_All_API.ESI;
using EVE_All_API.StaticData;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;

namespace EVE_All
{
    public partial class EVEAllMain : Form
    {
#region singleInstance
        // this class just wraps some Win32 stuffthat we're going to use
        internal class NativeMethods
        {
            public const int HWND_BROADCAST = 0xffff;
            public static readonly int WM_SHOW_EVE_ALL = RegisterWindowMessage("WM_SHOW_EVE_ALL");
            [DllImport("user32")]
            public static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);
            [DllImport("user32")]
            public static extern int RegisterWindowMessage(string message);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == NativeMethods.WM_SHOW_EVE_ALL)
            {
                if (WindowState == FormWindowState.Minimized)
                {
                    WindowState = FormWindowState.Normal;
                }
                // get our current "TopMost" value (ours will always be false though)
                bool top = TopMost;
                // make our form jump to the top of everything
                TopMost = true;
                // set it back to whatever it was
                TopMost = top;
            }
            base.WndProc(ref m);
        }
        #endregion

        private TabPage loaderTab;
        private TabPage marketTab;
        private Dictionary<long, TabPage> pilotTabs = new Dictionary<long, TabPage>();

        public EVEAllMain()
        {
            // Attempt to guess default language.
            var cul = System.Globalization.CultureInfo.CurrentCulture;
            UserData.language = cul.TwoLetterISOLanguageName;

            // Create default configuration Paths.
            string configPath = findConfigPath();
            UserData.cachePath = configPath + "/cache/";
            UserData.imagePath = configPath + "/image/";

            // Load configuration info.
            UserData.loadConfig(configPath + "/EVE-All.xml");

            //Market.updateMarketGroups();
            //Market.updateMarketValues();
            //Market.updateRegionMarket(999);
            //Market.updateRegionMarket(10000030);
            //List<long> orderIDs = Market.getOrdersForTypeAndRegion(34, 10000030);
            //Market.MarketEntry order = Market.getOrder(orderIDs[0]);
            //Sovereignty.updateStructures();
            //Sovereignty.Structure str = Sovereignty.getStructure(1022679654375);
            //Sovereignty.updateMap();
            //Sovereignty.Map map = Sovereignty.getMap(30000721);

            InitializeComponent();
            while (UserData.sdeZip == null || !File.Exists(UserData.sdeZip))
            {
                OpenFileDialog getZipDialog = new OpenFileDialog();
                getZipDialog.Title = "Requires 'SDE'.zip file";
                getZipDialog.Filter = "ZIP files|*.zip";
                getZipDialog.InitialDirectory = Directory.GetCurrentDirectory();
                DialogResult result = getZipDialog.ShowDialog();
                if (result != DialogResult.OK || !File.Exists(getZipDialog.FileName))
                {
                    MessageBox.Show("Unable to continue without sde file.", "Init error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Close();
                    return;
                }
                UserData.sdeZip = getZipDialog.FileName;
            }

            // Create the loader tab.
            loaderTab = new TabPage("Loader");
            LoaderTab loader = new LoaderTab();
            loader.Dock = DockStyle.Fill;
            loader.loadingComplete += Loader_loadingComplete;
            // Add loader to tabs.
            loaderTab.Controls.Add(loader);
            tabs.TabPages.Add(loaderTab);
        }

        private static string findConfigPath()
        {
            string keepPath = Application.UserAppDataPath;
            string workingPath = keepPath;
            if (!workingPath.Contains("Roaming"))
            {
                return workingPath;
            }
            while (!workingPath.EndsWith("Roaming"))
            {
                keepPath = workingPath;
                workingPath = Directory.GetParent(keepPath).FullName;
            }
            return keepPath;
        }

        private void Loader_loadingComplete(object sender, EventArgs e)
        {
            LoaderTab.LoaderArgs args = (LoaderTab.LoaderArgs)e;
            if (args?.loaderSuccess == true && args?.imageSuccess == true)
            {
                // loading succeded.
                //PathTest();
                PathTest();

                // Remove loader.
                tabs.TabPages.Remove(loaderTab);
                // To-DO: Add new tabs...

                // Create market browser tab
                marketTab = new TabPage("Market");
                marketTab.Dock = DockStyle.Fill;
                marketTab.Controls.Add(new MarketBrowserTab());
                tabs.TabPages.Add(marketTab);

                // Add characters.
                List<long> charIDs = APIKey.getAllCharacterIDs();
                foreach (long charID in charIDs)
                {
                    Pilot pilot = Pilot.getPilot(charID);
                    // Create the pilot tab.
                    PilotTab pTab = new PilotTab(pilot);
                    // Create the tab container.
                    TabPage pilotTab = new TabPage(pilot.characterName);
                    pTab.Dock = DockStyle.Fill;
                    // Add loader to tabs.
                    pilotTab.Controls.Add(pTab);
                    tabs.TabPages.Add(pilotTab);
                }
            }
            else
            {
                // Loading failed.
                Close();
            }
        }

        private void PathTest()
        {
            DateTime s = DateTime.Now;
            SolarSystem start = SolarSystem.getSystem(30002510);
            List<int> path = start.findPath(30000142, true);
            if (path == null)
            {
                System.Diagnostics.Debug.WriteLine("Path not found.");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(path.Count + " jumps.");
                foreach (int systemID in path)
                {
                    //SolarSystem system = SolarSystem.getSystem(systemID);
                    System.Diagnostics.Debug.WriteLine(systemID + ": " + InvNames.getName(systemID));
                }
            }
            DateTime f = DateTime.Now;
            TimeSpan ts = f - s;
            System.Diagnostics.Debug.WriteLine("Path test took: " + ts.TotalMilliseconds + " ms");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void manageAPIKeysToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (ManageKeys manage = new ManageKeys())
            {
                DialogResult result = manage.ShowDialog();
                if(result == DialogResult.OK)
                {
                    // Save the updated configuration to file.
                    UserData.saveConfig();
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (UserData.cachePath != null)
            {
                Market.saveAll(UserData.cachePath + "Market.cache");
            }
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using(OptionsDialog options = new OptionsDialog())
            {
                DialogResult result = options.ShowDialog();
                if(result == DialogResult.OK)
                {
                    options.confirmSettings();
                    // Save the updated configuration to file.
                    UserData.saveConfig();
                }
            }

        }
    }
}
