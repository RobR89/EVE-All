using EVE_All_API;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace EVE_All
{
    public partial class OptionsDialog : Form
    {
        public OptionsDialog()
        {
            InitializeComponent();
            cachePath.Text = UserData.cachePath;
            imagePath.Text = UserData.imagePath;
            sdeZip.Text = UserData.sdeZip;
            typeZip.Text = UserData.typeIconZip;
            renderZip.Text = UserData.renderZip;
            iconsZip.Text = UserData.iconsZip;
            SSO_ClientID.Text = UserData.sso_ClientID;
            SSO_ResponseURI.Text = UserData.sso_RedirectURI;
            SSO_SecurityKey.Text = UserData.sso_SecurityKey;
            SSO_Scopes.Text = UserData.sso_Scopes;

            List<TextBox> boxes = new List<TextBox>(){ sdeZip, typeZip, renderZip, iconsZip };
            foreach(TextBox textBox in boxes)
            {
                if(!(textBox.Tag is String))
                {
                    continue;
                }
                ToolTip tip = new ToolTip();
                tip.IsBalloon = true;
                tip.AutomaticDelay = 5000;
                tip.AutoPopDelay = 50000;
                tip.InitialDelay = 100;
                tip.ReshowDelay = 500;
                tip.ShowAlways = true;
                tip.SetToolTip(textBox, textBox.Tag as String);
            }
        }

        public void confirmSettings()
        {
            UserData.cachePath = cachePath.Text;
            UserData.imagePath = imagePath.Text;
            UserData.sdeZip = sdeZip.Text;
            UserData.typeIconZip = typeZip.Text;
            UserData.renderZip = renderZip.Text;
            UserData.iconsZip = iconsZip.Text;
            UserData.sso_ClientID = SSO_ClientID.Text;
            UserData.sso_RedirectURI = SSO_ResponseURI.Text;
            UserData.sso_SecurityKey = SSO_SecurityKey.Text;
            UserData.sso_Scopes = SSO_Scopes.Text;
        }

        private void BrowseCachePath_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fb = new FolderBrowserDialog())
            {
                DialogResult result = fb.ShowDialog();
                if(result == DialogResult.OK)
                {
                    cachePath.Text = fb.SelectedPath;
                }
            }
        }

        private void BrowseImagePath_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fb = new FolderBrowserDialog())
            {
                DialogResult result = fb.ShowDialog();
                if (result == DialogResult.OK)
                {
                    imagePath.Text = fb.SelectedPath;
                }
            }
        }

        private void BrowseSDEzip_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog fb = new OpenFileDialog())
            {
                fb.Title = "SDE.zip file";
                fb.Filter = "ZIP files|*.zip";
                DialogResult result = fb.ShowDialog();
                if (result == DialogResult.OK)
                {
                    sdeZip.Text = fb.FileName;
                }
            }
        }

        private void BrowseTypeZip_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog fb = new OpenFileDialog())
            {
                fb.Title = "Types.zip file";
                fb.Filter = "ZIP files|*.zip";
                DialogResult result = fb.ShowDialog();
                if (result == DialogResult.OK)
                {
                    typeZip.Text = fb.FileName;
                }
            }
        }

        private void BrowseRenderZip_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog fb = new OpenFileDialog())
            {
                fb.Title = "Render.zip file";
                fb.Filter = "ZIP files|*.zip";
                DialogResult result = fb.ShowDialog();
                if (result == DialogResult.OK)
                {
                    renderZip.Text = fb.FileName;
                }
            }
        }

        private void BrowseIconsZip_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog fb = new OpenFileDialog())
            {
                fb.Title = "Icons.zip file";
                fb.Filter = "ZIP files|*.zip";
                DialogResult result = fb.ShowDialog();
                if (result == DialogResult.OK)
                {
                    iconsZip.Text = fb.FileName;
                }
            }
        }

    }
}
