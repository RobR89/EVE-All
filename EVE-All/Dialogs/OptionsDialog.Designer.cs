namespace EVE_All
{
    partial class OptionsDialog
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
            this.Accept = new System.Windows.Forms.Button();
            this.cachePath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.imagePath = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.sdeZip = new System.Windows.Forms.TextBox();
            this.Canlel = new System.Windows.Forms.Button();
            this.BrowseSDEzip = new System.Windows.Forms.Button();
            this.BrowseImagePath = new System.Windows.Forms.Button();
            this.BrowseCachePath = new System.Windows.Forms.Button();
            this.BrowseTypeZip = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.typeZip = new System.Windows.Forms.TextBox();
            this.BrowseRenderZip = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.renderZip = new System.Windows.Forms.TextBox();
            this.BrowseIconsZip = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.iconsZip = new System.Windows.Forms.TextBox();
            this.mainTab = new System.Windows.Forms.TabControl();
            this.pathsTab = new System.Windows.Forms.TabPage();
            this.staticDataTab = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.mainLayout = new System.Windows.Forms.TableLayoutPanel();
            this.mainTab.SuspendLayout();
            this.pathsTab.SuspendLayout();
            this.staticDataTab.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.mainLayout.SuspendLayout();
            this.SuspendLayout();
            // 
            // Accept
            // 
            this.Accept.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Accept.Location = new System.Drawing.Point(354, 3);
            this.Accept.Name = "Accept";
            this.Accept.Size = new System.Drawing.Size(75, 23);
            this.Accept.TabIndex = 0;
            this.Accept.Text = "Accept";
            this.Accept.UseVisualStyleBackColor = true;
            // 
            // cachePath
            // 
            this.cachePath.Location = new System.Drawing.Point(6, 19);
            this.cachePath.Name = "cachePath";
            this.cachePath.Size = new System.Drawing.Size(324, 20);
            this.cachePath.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Cache path";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Image cache path";
            // 
            // imagePath
            // 
            this.imagePath.Location = new System.Drawing.Point(6, 58);
            this.imagePath.Name = "imagePath";
            this.imagePath.Size = new System.Drawing.Size(324, 20);
            this.imagePath.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "SDE zip file";
            // 
            // sdeZip
            // 
            this.sdeZip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sdeZip.Location = new System.Drawing.Point(3, 16);
            this.sdeZip.Name = "sdeZip";
            this.sdeZip.Size = new System.Drawing.Size(331, 20);
            this.sdeZip.TabIndex = 10;
            this.sdeZip.Tag = "The zip file containing the static data files.";
            // 
            // Canlel
            // 
            this.Canlel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Canlel.Location = new System.Drawing.Point(273, 3);
            this.Canlel.Name = "Canlel";
            this.Canlel.Size = new System.Drawing.Size(75, 23);
            this.Canlel.TabIndex = 13;
            this.Canlel.Text = "Cancel";
            this.Canlel.UseVisualStyleBackColor = true;
            // 
            // BrowseSDEzip
            // 
            this.BrowseSDEzip.Location = new System.Drawing.Point(340, 16);
            this.BrowseSDEzip.Name = "BrowseSDEzip";
            this.BrowseSDEzip.Size = new System.Drawing.Size(75, 23);
            this.BrowseSDEzip.TabIndex = 12;
            this.BrowseSDEzip.Text = "Browse";
            this.BrowseSDEzip.UseVisualStyleBackColor = true;
            this.BrowseSDEzip.Click += new System.EventHandler(this.BrowseSDEzip_Click);
            // 
            // BrowseImagePath
            // 
            this.BrowseImagePath.Location = new System.Drawing.Point(336, 55);
            this.BrowseImagePath.Name = "BrowseImagePath";
            this.BrowseImagePath.Size = new System.Drawing.Size(75, 23);
            this.BrowseImagePath.TabIndex = 6;
            this.BrowseImagePath.Text = "Browse";
            this.BrowseImagePath.UseVisualStyleBackColor = true;
            this.BrowseImagePath.Click += new System.EventHandler(this.BrowseImagePath_Click);
            // 
            // BrowseCachePath
            // 
            this.BrowseCachePath.Location = new System.Drawing.Point(336, 16);
            this.BrowseCachePath.Name = "BrowseCachePath";
            this.BrowseCachePath.Size = new System.Drawing.Size(75, 23);
            this.BrowseCachePath.TabIndex = 3;
            this.BrowseCachePath.Text = "Browse";
            this.BrowseCachePath.UseVisualStyleBackColor = true;
            this.BrowseCachePath.Click += new System.EventHandler(this.BrowseCachePath_Click);
            // 
            // BrowseTypeZip
            // 
            this.BrowseTypeZip.Location = new System.Drawing.Point(340, 100);
            this.BrowseTypeZip.Name = "BrowseTypeZip";
            this.BrowseTypeZip.Size = new System.Drawing.Size(75, 23);
            this.BrowseTypeZip.TabIndex = 16;
            this.BrowseTypeZip.Text = "Browse";
            this.BrowseTypeZip.UseVisualStyleBackColor = true;
            this.BrowseTypeZip.Click += new System.EventHandler(this.BrowseTypeZip_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 84);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(92, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Type Icons zip file";
            // 
            // typeZip
            // 
            this.typeZip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.typeZip.Location = new System.Drawing.Point(3, 100);
            this.typeZip.Name = "typeZip";
            this.typeZip.Size = new System.Drawing.Size(331, 20);
            this.typeZip.TabIndex = 14;
            this.typeZip.Tag = "The zip file containing the Type icon files.";
            // 
            // BrowseRenderZip
            // 
            this.BrowseRenderZip.Location = new System.Drawing.Point(340, 58);
            this.BrowseRenderZip.Name = "BrowseRenderZip";
            this.BrowseRenderZip.Size = new System.Drawing.Size(75, 23);
            this.BrowseRenderZip.TabIndex = 19;
            this.BrowseRenderZip.Text = "Browse";
            this.BrowseRenderZip.UseVisualStyleBackColor = true;
            this.BrowseRenderZip.Click += new System.EventHandler(this.BrowseRenderZip_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 42);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 13);
            this.label6.TabIndex = 18;
            this.label6.Text = "Renders zip file";
            // 
            // renderZip
            // 
            this.renderZip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.renderZip.Location = new System.Drawing.Point(3, 58);
            this.renderZip.Name = "renderZip";
            this.renderZip.Size = new System.Drawing.Size(331, 20);
            this.renderZip.TabIndex = 17;
            this.renderZip.Tag = "The zip file containing the Render image files.";
            // 
            // BrowseIconsZip
            // 
            this.BrowseIconsZip.Location = new System.Drawing.Point(340, 142);
            this.BrowseIconsZip.Name = "BrowseIconsZip";
            this.BrowseIconsZip.Size = new System.Drawing.Size(75, 23);
            this.BrowseIconsZip.TabIndex = 22;
            this.BrowseIconsZip.Text = "Browse";
            this.BrowseIconsZip.UseVisualStyleBackColor = true;
            this.BrowseIconsZip.Click += new System.EventHandler(this.BrowseIconsZip_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 126);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 13);
            this.label7.TabIndex = 21;
            this.label7.Text = "Icons zip file";
            // 
            // iconsZip
            // 
            this.iconsZip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.iconsZip.Location = new System.Drawing.Point(3, 142);
            this.iconsZip.Name = "iconsZip";
            this.iconsZip.Size = new System.Drawing.Size(331, 20);
            this.iconsZip.TabIndex = 20;
            this.iconsZip.Tag = "The zip file containing the general icon files.";
            // 
            // mainTab
            // 
            this.mainTab.Controls.Add(this.pathsTab);
            this.mainTab.Controls.Add(this.staticDataTab);
            this.mainTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTab.Location = new System.Drawing.Point(3, 3);
            this.mainTab.Name = "mainTab";
            this.mainTab.SelectedIndex = 0;
            this.mainTab.Size = new System.Drawing.Size(432, 338);
            this.mainTab.TabIndex = 23;
            // 
            // pathsTab
            // 
            this.pathsTab.Controls.Add(this.label1);
            this.pathsTab.Controls.Add(this.cachePath);
            this.pathsTab.Controls.Add(this.BrowseCachePath);
            this.pathsTab.Controls.Add(this.BrowseImagePath);
            this.pathsTab.Controls.Add(this.imagePath);
            this.pathsTab.Controls.Add(this.label2);
            this.pathsTab.Location = new System.Drawing.Point(4, 22);
            this.pathsTab.Name = "pathsTab";
            this.pathsTab.Padding = new System.Windows.Forms.Padding(3);
            this.pathsTab.Size = new System.Drawing.Size(424, 312);
            this.pathsTab.TabIndex = 0;
            this.pathsTab.Text = "Paths";
            this.pathsTab.UseVisualStyleBackColor = true;
            // 
            // staticDataTab
            // 
            this.staticDataTab.Controls.Add(this.tableLayoutPanel2);
            this.staticDataTab.Location = new System.Drawing.Point(4, 22);
            this.staticDataTab.Name = "staticDataTab";
            this.staticDataTab.Padding = new System.Windows.Forms.Padding(3);
            this.staticDataTab.Size = new System.Drawing.Size(424, 312);
            this.staticDataTab.TabIndex = 1;
            this.staticDataTab.Text = "Static Data";
            this.staticDataTab.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.BrowseSDEzip, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.sdeZip, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.BrowseIconsZip, 1, 7);
            this.tableLayoutPanel2.Controls.Add(this.iconsZip, 0, 7);
            this.tableLayoutPanel2.Controls.Add(this.label7, 0, 6);
            this.tableLayoutPanel2.Controls.Add(this.typeZip, 0, 5);
            this.tableLayoutPanel2.Controls.Add(this.BrowseTypeZip, 1, 5);
            this.tableLayoutPanel2.Controls.Add(this.label5, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.renderZip, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.BrowseRenderZip, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.label6, 0, 2);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 8;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(418, 306);
            this.tableLayoutPanel2.TabIndex = 23;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.Canlel, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.Accept, 2, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 347);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(432, 33);
            this.tableLayoutPanel1.TabIndex = 25;
            // 
            // mainLayout
            // 
            this.mainLayout.ColumnCount = 1;
            this.mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayout.Controls.Add(this.mainTab, 0, 0);
            this.mainLayout.Controls.Add(this.tableLayoutPanel1, 0, 1);
            this.mainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayout.Location = new System.Drawing.Point(0, 0);
            this.mainLayout.Name = "mainLayout";
            this.mainLayout.RowCount = 2;
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.mainLayout.Size = new System.Drawing.Size(438, 383);
            this.mainLayout.TabIndex = 25;
            // 
            // OptionsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(438, 383);
            this.Controls.Add(this.mainLayout);
            this.Name = "OptionsDialog";
            this.Text = "OptionsDialog";
            this.mainTab.ResumeLayout(false);
            this.pathsTab.ResumeLayout(false);
            this.pathsTab.PerformLayout();
            this.staticDataTab.ResumeLayout(false);
            this.staticDataTab.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.mainLayout.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Accept;
        private System.Windows.Forms.TextBox cachePath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox imagePath;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox sdeZip;
        private System.Windows.Forms.Button Canlel;
        private System.Windows.Forms.Button BrowseCachePath;
        private System.Windows.Forms.Button BrowseImagePath;
        private System.Windows.Forms.Button BrowseSDEzip;
        private System.Windows.Forms.Button BrowseTypeZip;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox typeZip;
        private System.Windows.Forms.Button BrowseRenderZip;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox renderZip;
        private System.Windows.Forms.Button BrowseIconsZip;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox iconsZip;
        private System.Windows.Forms.TabControl mainTab;
        private System.Windows.Forms.TabPage pathsTab;
        private System.Windows.Forms.TabPage staticDataTab;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel mainLayout;
    }
}