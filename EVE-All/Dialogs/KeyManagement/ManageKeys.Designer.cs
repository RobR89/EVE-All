namespace EVE_All.KeyManagement
{
    partial class ManageKeys
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.getKeyLink = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.addKeyBtn = new System.Windows.Forms.Button();
            this.confirmBtn = new System.Windows.Forms.Button();
            this.keyView = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.useKey = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.keyName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.keyID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.keyType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.use1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.character1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.use2 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.character2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.use3 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.character3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.keyView)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // getKeyLink
            // 
            this.getKeyLink.AutoSize = true;
            this.getKeyLink.Location = new System.Drawing.Point(3, 22);
            this.getKeyLink.Name = "getKeyLink";
            this.getKeyLink.Size = new System.Drawing.Size(13, 13);
            this.getKeyLink.TabIndex = 0;
            this.getKeyLink.TabStop = true;
            this.getKeyLink.Text = "a";
            this.getKeyLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.getKeyLink_LinkClicked);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(284, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "To get an API key for your account visit the folowing page.";
            // 
            // addKeyBtn
            // 
            this.addKeyBtn.Location = new System.Drawing.Point(6, 42);
            this.addKeyBtn.Name = "addKeyBtn";
            this.addKeyBtn.Size = new System.Drawing.Size(75, 23);
            this.addKeyBtn.TabIndex = 3;
            this.addKeyBtn.Text = "Add Key";
            this.addKeyBtn.UseVisualStyleBackColor = true;
            this.addKeyBtn.Click += new System.EventHandler(this.addKeyBtn_Click);
            // 
            // confirmBtn
            // 
            this.confirmBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.confirmBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.confirmBtn.Location = new System.Drawing.Point(745, 42);
            this.confirmBtn.Name = "confirmBtn";
            this.confirmBtn.Size = new System.Drawing.Size(75, 23);
            this.confirmBtn.TabIndex = 4;
            this.confirmBtn.Text = "Confirm";
            this.confirmBtn.UseVisualStyleBackColor = true;
            this.confirmBtn.Click += new System.EventHandler(this.confirmBtn_Click);
            // 
            // keyView
            // 
            this.keyView.AllowUserToAddRows = false;
            this.keyView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.keyView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.keyView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.useKey,
            this.keyName,
            this.keyID,
            this.vCode,
            this.keyType,
            this.use1,
            this.character1,
            this.use2,
            this.character2,
            this.use3,
            this.character3});
            this.keyView.Location = new System.Drawing.Point(0, 75);
            this.keyView.MultiSelect = false;
            this.keyView.Name = "keyView";
            this.keyView.Size = new System.Drawing.Size(831, 146);
            this.keyView.TabIndex = 5;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.addKeyBtn);
            this.panel1.Controls.Add(this.getKeyLink);
            this.panel1.Controls.Add(this.confirmBtn);
            this.panel1.Location = new System.Drawing.Point(0, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(831, 68);
            this.panel1.TabIndex = 6;
            // 
            // useKey
            // 
            this.useKey.HeaderText = "";
            this.useKey.Name = "useKey";
            this.useKey.Width = 20;
            // 
            // keyName
            // 
            this.keyName.HeaderText = "Key Name";
            this.keyName.Name = "keyName";
            // 
            // keyID
            // 
            this.keyID.HeaderText = "KeyID";
            this.keyID.Name = "keyID";
            this.keyID.ReadOnly = true;
            // 
            // vCode
            // 
            this.vCode.HeaderText = "vCode";
            this.vCode.Name = "vCode";
            // 
            // keyType
            // 
            this.keyType.HeaderText = "Type";
            this.keyType.Name = "keyType";
            this.keyType.ReadOnly = true;
            // 
            // use1
            // 
            this.use1.HeaderText = "";
            this.use1.Name = "use1";
            this.use1.Width = 20;
            // 
            // character1
            // 
            this.character1.HeaderText = "Character";
            this.character1.Name = "character1";
            this.character1.ReadOnly = true;
            // 
            // use2
            // 
            this.use2.HeaderText = "";
            this.use2.Name = "use2";
            this.use2.Width = 20;
            // 
            // character2
            // 
            this.character2.HeaderText = "Character";
            this.character2.Name = "character2";
            this.character2.ReadOnly = true;
            // 
            // use3
            // 
            this.use3.HeaderText = "";
            this.use3.Name = "use3";
            this.use3.Width = 20;
            // 
            // character3
            // 
            this.character3.HeaderText = "Character";
            this.character3.Name = "character3";
            this.character3.ReadOnly = true;
            // 
            // ManageKeys
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(832, 221);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.keyView);
            this.Name = "ManageKeys";
            this.Text = "ManageKeys";
            this.Load += new System.EventHandler(this.ManageKeys_Load);
            ((System.ComponentModel.ISupportInitialize)(this.keyView)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.LinkLabel getKeyLink;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button addKeyBtn;
        private System.Windows.Forms.Button confirmBtn;
        private System.Windows.Forms.DataGridView keyView;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridViewTextBoxColumn keyName;
        private System.Windows.Forms.DataGridViewTextBoxColumn character3;
        private System.Windows.Forms.DataGridViewCheckBoxColumn use3;
        private System.Windows.Forms.DataGridViewTextBoxColumn character2;
        private System.Windows.Forms.DataGridViewCheckBoxColumn use2;
        private System.Windows.Forms.DataGridViewTextBoxColumn character1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn use1;
        private System.Windows.Forms.DataGridViewTextBoxColumn keyType;
        private System.Windows.Forms.DataGridViewTextBoxColumn vCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn keyID;
        private System.Windows.Forms.DataGridViewCheckBoxColumn useKey;
    }
}