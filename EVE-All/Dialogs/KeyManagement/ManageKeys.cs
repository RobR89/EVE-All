using EVE_All_API;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace EVE_All.KeyManagement
{
    public partial class ManageKeys : Form
    {
        private Dictionary<string, long> characterIDmap = new Dictionary<string, long>();

        public ManageKeys()
        {
            InitializeComponent();
        }

        private void ManageKeys_Load(object sender, EventArgs e)
        {
            getKeyLink.Text = UserData.getKeyURL;
            foreach(long keyID in APIKey.getAllKeyIDs())
            {
                APIKey key = APIKey.getKey(keyID);
                addKeyToList(key);
            }
        }

        private void getKeyLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            getKeyLink.LinkVisited = true;
            System.Diagnostics.Process.Start(UserData.getKeyURL);
        }

        private void addKeyToList(APIKey key)
        {
            if(key == null)
            {
                return;
            }
            // Get the characters.
            List<long> chars = new List<long>(key.useCharacters);
            chars.AddRange(key.ignore);
            // Add row.
            int rowID = keyView.Rows.Add(key.keyActive, key.keyName, key.keyID, key.vCode, key.keyType, true, "", true, "", true, "");
            DataGridViewRow row = keyView.Rows[rowID];
            // Get character names and usages.
            int i = 5;
            foreach (long cID in chars)
            {
                Pilot p = Pilot.getPilot(cID);
                characterIDmap[p.characterName] = p.characterID;
                row.Cells[i].Value = key.useCharacters.Contains(p.characterID);
                i++;
                row.Cells[i].Value = p.characterName;
                i++;
                if(i > 9)
                {
                    break;
                }
            }

        }

        private void addKeyBtn_Click(object sender, EventArgs e)
        {
            AddKey add = new AddKey();
            DialogResult result = add.ShowDialog();
            if(result == DialogResult.OK)
            {
                long keyID = Int64.Parse(add.keyID.Text);
                if(APIKey.haveKey(keyID))
                {
                    MessageBox.Show("KeyID already exists.", "Key error", MessageBoxButtons.OK);
                    return;
                }
                APIKey key = APIKey.getKey(keyID, add.vCode.Text);
                // Update key name.
                key.keyName = add.keyName.Text;
                addKeyToList(key);
            }
        }

        private void confirmBtn_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in keyView.Rows)
            {
                long keyID = Int64.Parse(row.Cells["keyID"].Value.ToString());
                string vCode = row.Cells["vCode"].Value.ToString();
                // Get the key.
                APIKey key = APIKey.getKey(keyID, vCode);
                key.keyName = row.Cells["keyName"].Value.ToString();
                key.keyActive = bool.Parse(row.Cells["useKey"].Value.ToString());
                key.useCharacters.Clear();
                key.ignore.Clear();
                for (int i = 1; i <= 3; i++)
                {
                    if(row.Cells["character" + i.ToString()].Value == null)
                    {
                        continue;
                    }
                    string cName = row.Cells["character" + i.ToString()].Value.ToString();
                    if (cName != null && cName.Length > 0)
                    {
                        if (characterIDmap.ContainsKey(cName))
                        {
                            if(bool.Parse(row.Cells["use" + i.ToString()].Value.ToString()) == true)
                            {
                                key.useCharacters.Add(characterIDmap[cName]);
                            }
                            else
                            {
                                key.ignore.Add(characterIDmap[cName]);
                            }
                        }
                    }
                }
            }
        }

    }
}
