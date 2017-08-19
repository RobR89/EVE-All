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
        private TabPage pilotListTab;

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

            InitializeComponent();

            // Check for existance of SDE files. Open dialog if they cannot be found.
            while (UserData.sdeZip == null || !File.Exists(UserData.sdeZip))
            {
                // TO-DO: add dialog informing user of where they can download the files from with a clickable link.
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
                // Remove loader.
                tabs.TabPages.Remove(loaderTab);

                // Create pilot list tab
                pilotListTab = new TabPage("Characters");
                pilotListTab.Dock = DockStyle.Fill;
                pilotListTab.Controls.Add(new PilotListTab());
                tabs.TabPages.Add(pilotListTab);

                // Create market browser tab
                marketTab = new TabPage("Market");
                marketTab.Dock = DockStyle.Fill;
                marketTab.Controls.Add(new MarketBrowserTab());
                tabs.TabPages.Add(marketTab);

            }
            else
            {
                // Loading failed.
                // TO-DO: display a reason...
                Close();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (UserData.cachePath != null)
            {
                Market.saveAll(UserData.cachePath + "Market.cache");
            }
            UserData.saveConfig();
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

        private void loginToCharacterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SSO.startRequest();
        }
    }
}
