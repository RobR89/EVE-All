namespace EVE_All.Tabs
{
    partial class MarketBrowserTab
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.marketTree = new System.Windows.Forms.TreeView();
            this.marketSplitter = new System.Windows.Forms.SplitContainer();
            this.marketItemPanel = new System.Windows.Forms.TableLayoutPanel();
            this.marketInfoPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.typeImage = new System.Windows.Forms.PictureBox();
            this.typeNameLabel = new System.Windows.Forms.Label();
            this.marketOrdersSplitter = new System.Windows.Forms.SplitContainer();
            this.marketSellersList = new System.Windows.Forms.ListView();
            this.marketBuyersList = new System.Windows.Forms.ListView();
            this.regionLabel = new System.Windows.Forms.Label();
            this.regionSelect = new System.Windows.Forms.ComboBox();
            this.RefreshButton = new System.Windows.Forms.Button();
            this.RegionPanel = new System.Windows.Forms.TableLayoutPanel();
            this.regionUpdateStatus = new System.Windows.Forms.Label();
            this.SellOrdersLayout = new System.Windows.Forms.TableLayoutPanel();
            this.sellOrdersLabel = new System.Windows.Forms.Label();
            this.buyOrdersLayout = new System.Windows.Forms.TableLayoutPanel();
            this.buyOrdersLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.marketSplitter)).BeginInit();
            this.marketSplitter.Panel1.SuspendLayout();
            this.marketSplitter.Panel2.SuspendLayout();
            this.marketSplitter.SuspendLayout();
            this.marketItemPanel.SuspendLayout();
            this.marketInfoPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.typeImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.marketOrdersSplitter)).BeginInit();
            this.marketOrdersSplitter.Panel1.SuspendLayout();
            this.marketOrdersSplitter.Panel2.SuspendLayout();
            this.marketOrdersSplitter.SuspendLayout();
            this.RegionPanel.SuspendLayout();
            this.SellOrdersLayout.SuspendLayout();
            this.buyOrdersLayout.SuspendLayout();
            this.SuspendLayout();
            // 
            // marketTree
            // 
            this.marketTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.marketTree.Location = new System.Drawing.Point(0, 0);
            this.marketTree.MinimumSize = new System.Drawing.Size(80, 100);
            this.marketTree.Name = "marketTree";
            this.marketTree.Size = new System.Drawing.Size(200, 456);
            this.marketTree.TabIndex = 0;
            // 
            // marketSplitter
            // 
            this.marketSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.marketSplitter.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.marketSplitter.Location = new System.Drawing.Point(0, 0);
            this.marketSplitter.Name = "marketSplitter";
            // 
            // marketSplitter.Panel1
            // 
            this.marketSplitter.Panel1.Controls.Add(this.marketTree);
            // 
            // marketSplitter.Panel2
            // 
            this.marketSplitter.Panel2.Controls.Add(this.marketItemPanel);
            this.marketSplitter.Size = new System.Drawing.Size(754, 456);
            this.marketSplitter.SplitterDistance = 200;
            this.marketSplitter.TabIndex = 1;
            // 
            // marketItemPanel
            // 
            this.marketItemPanel.ColumnCount = 1;
            this.marketItemPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.marketItemPanel.Controls.Add(this.marketInfoPanel, 0, 1);
            this.marketItemPanel.Controls.Add(this.marketOrdersSplitter, 0, 2);
            this.marketItemPanel.Controls.Add(this.RegionPanel, 0, 0);
            this.marketItemPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.marketItemPanel.Location = new System.Drawing.Point(0, 0);
            this.marketItemPanel.Name = "marketItemPanel";
            this.marketItemPanel.RowCount = 3;
            this.marketItemPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.marketItemPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.marketItemPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.marketItemPanel.Size = new System.Drawing.Size(550, 456);
            this.marketItemPanel.TabIndex = 1;
            // 
            // marketInfoPanel
            // 
            this.marketInfoPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.marketInfoPanel.AutoSize = true;
            this.marketInfoPanel.Controls.Add(this.typeImage);
            this.marketInfoPanel.Controls.Add(this.typeNameLabel);
            this.marketInfoPanel.Location = new System.Drawing.Point(3, 38);
            this.marketInfoPanel.Name = "marketInfoPanel";
            this.marketInfoPanel.Size = new System.Drawing.Size(544, 70);
            this.marketInfoPanel.TabIndex = 0;
            // 
            // typeImage
            // 
            this.typeImage.Location = new System.Drawing.Point(3, 3);
            this.typeImage.MaximumSize = new System.Drawing.Size(64, 64);
            this.typeImage.MinimumSize = new System.Drawing.Size(64, 64);
            this.typeImage.Name = "typeImage";
            this.typeImage.Size = new System.Drawing.Size(64, 64);
            this.typeImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.typeImage.TabIndex = 0;
            this.typeImage.TabStop = false;
            // 
            // typeNameLabel
            // 
            this.typeNameLabel.AutoSize = true;
            this.typeNameLabel.Location = new System.Drawing.Point(73, 0);
            this.typeNameLabel.Name = "typeNameLabel";
            this.typeNameLabel.Size = new System.Drawing.Size(0, 13);
            this.typeNameLabel.TabIndex = 1;
            // 
            // marketOrdersSplitter
            // 
            this.marketOrdersSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.marketOrdersSplitter.Location = new System.Drawing.Point(3, 114);
            this.marketOrdersSplitter.Name = "marketOrdersSplitter";
            this.marketOrdersSplitter.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // marketOrdersSplitter.Panel1
            // 
            this.marketOrdersSplitter.Panel1.Controls.Add(this.SellOrdersLayout);
            // 
            // marketOrdersSplitter.Panel2
            // 
            this.marketOrdersSplitter.Panel2.Controls.Add(this.buyOrdersLayout);
            this.marketOrdersSplitter.Size = new System.Drawing.Size(544, 344);
            this.marketOrdersSplitter.SplitterDistance = 159;
            this.marketOrdersSplitter.TabIndex = 1;
            // 
            // marketSellersList
            // 
            this.marketSellersList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.marketSellersList.FullRowSelect = true;
            this.marketSellersList.GridLines = true;
            this.marketSellersList.Location = new System.Drawing.Point(3, 16);
            this.marketSellersList.Name = "marketSellersList";
            this.marketSellersList.Size = new System.Drawing.Size(538, 159);
            this.marketSellersList.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.marketSellersList.TabIndex = 0;
            this.marketSellersList.UseCompatibleStateImageBehavior = false;
            this.marketSellersList.View = System.Windows.Forms.View.Details;
            // 
            // marketBuyersList
            // 
            this.marketBuyersList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.marketBuyersList.FullRowSelect = true;
            this.marketBuyersList.GridLines = true;
            this.marketBuyersList.Location = new System.Drawing.Point(3, 16);
            this.marketBuyersList.Name = "marketBuyersList";
            this.marketBuyersList.Size = new System.Drawing.Size(538, 162);
            this.marketBuyersList.Sorting = System.Windows.Forms.SortOrder.Descending;
            this.marketBuyersList.TabIndex = 0;
            this.marketBuyersList.UseCompatibleStateImageBehavior = false;
            this.marketBuyersList.View = System.Windows.Forms.View.Details;
            // 
            // regionLabel
            // 
            this.regionLabel.AutoSize = true;
            this.regionLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.regionLabel.Location = new System.Drawing.Point(3, 0);
            this.regionLabel.Name = "regionLabel";
            this.regionLabel.Size = new System.Drawing.Size(41, 29);
            this.regionLabel.TabIndex = 0;
            this.regionLabel.Text = "Region";
            this.regionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // regionSelect
            // 
            this.regionSelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.regionSelect.FormattingEnabled = true;
            this.regionSelect.Location = new System.Drawing.Point(50, 3);
            this.regionSelect.Name = "regionSelect";
            this.regionSelect.Size = new System.Drawing.Size(156, 21);
            this.regionSelect.TabIndex = 1;
            this.regionSelect.SelectedValueChanged += new System.EventHandler(this.RegionSelect_SelectedValueChanged);
            // 
            // RefreshButton
            // 
            this.RefreshButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.RefreshButton.Location = new System.Drawing.Point(466, 3);
            this.RefreshButton.Name = "RefreshButton";
            this.RefreshButton.Size = new System.Drawing.Size(75, 23);
            this.RefreshButton.TabIndex = 2;
            this.RefreshButton.Text = "Refresh";
            this.RefreshButton.UseVisualStyleBackColor = true;
            this.RefreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // RegionPanel
            // 
            this.RegionPanel.AutoSize = true;
            this.RegionPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.RegionPanel.ColumnCount = 4;
            this.RegionPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.RegionPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.RegionPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.RegionPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.RegionPanel.Controls.Add(this.regionLabel, 0, 0);
            this.RegionPanel.Controls.Add(this.RefreshButton, 3, 0);
            this.RegionPanel.Controls.Add(this.regionSelect, 1, 0);
            this.RegionPanel.Controls.Add(this.regionUpdateStatus, 2, 0);
            this.RegionPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RegionPanel.Location = new System.Drawing.Point(3, 3);
            this.RegionPanel.Name = "RegionPanel";
            this.RegionPanel.RowCount = 1;
            this.RegionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.RegionPanel.Size = new System.Drawing.Size(544, 29);
            this.RegionPanel.TabIndex = 3;
            // 
            // regionUpdateStatus
            // 
            this.regionUpdateStatus.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.regionUpdateStatus.AutoSize = true;
            this.regionUpdateStatus.Location = new System.Drawing.Point(212, 8);
            this.regionUpdateStatus.Name = "regionUpdateStatus";
            this.regionUpdateStatus.Size = new System.Drawing.Size(0, 13);
            this.regionUpdateStatus.TabIndex = 3;
            // 
            // SellOrdersLayout
            // 
            this.SellOrdersLayout.ColumnCount = 1;
            this.SellOrdersLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.SellOrdersLayout.Controls.Add(this.marketSellersList, 0, 1);
            this.SellOrdersLayout.Controls.Add(this.sellOrdersLabel, 0, 0);
            this.SellOrdersLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SellOrdersLayout.Location = new System.Drawing.Point(0, 0);
            this.SellOrdersLayout.Name = "SellOrdersLayout";
            this.SellOrdersLayout.RowCount = 2;
            this.SellOrdersLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.SellOrdersLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.SellOrdersLayout.Size = new System.Drawing.Size(544, 159);
            this.SellOrdersLayout.TabIndex = 0;
            // 
            // sellOrdersLabel
            // 
            this.sellOrdersLabel.AutoSize = true;
            this.sellOrdersLabel.Location = new System.Drawing.Point(3, 0);
            this.sellOrdersLabel.Name = "sellOrdersLabel";
            this.sellOrdersLabel.Size = new System.Drawing.Size(56, 13);
            this.sellOrdersLabel.TabIndex = 1;
            this.sellOrdersLabel.Text = "Sell orders";
            // 
            // buyOrdersLayout
            // 
            this.buyOrdersLayout.ColumnCount = 1;
            this.buyOrdersLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.buyOrdersLayout.Controls.Add(this.buyOrdersLabel, 0, 0);
            this.buyOrdersLayout.Controls.Add(this.marketBuyersList, 0, 1);
            this.buyOrdersLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buyOrdersLayout.Location = new System.Drawing.Point(0, 0);
            this.buyOrdersLayout.Name = "buyOrdersLayout";
            this.buyOrdersLayout.RowCount = 2;
            this.buyOrdersLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.buyOrdersLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.buyOrdersLayout.Size = new System.Drawing.Size(544, 181);
            this.buyOrdersLayout.TabIndex = 0;
            // 
            // buyOrdersLabel
            // 
            this.buyOrdersLabel.AutoSize = true;
            this.buyOrdersLabel.Location = new System.Drawing.Point(3, 0);
            this.buyOrdersLabel.Name = "buyOrdersLabel";
            this.buyOrdersLabel.Size = new System.Drawing.Size(59, 13);
            this.buyOrdersLabel.TabIndex = 0;
            this.buyOrdersLabel.Text = "Buy Orders";
            // 
            // MarketBrowserTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.marketSplitter);
            this.Name = "MarketBrowserTab";
            this.Size = new System.Drawing.Size(754, 456);
            this.marketSplitter.Panel1.ResumeLayout(false);
            this.marketSplitter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.marketSplitter)).EndInit();
            this.marketSplitter.ResumeLayout(false);
            this.marketItemPanel.ResumeLayout(false);
            this.marketItemPanel.PerformLayout();
            this.marketInfoPanel.ResumeLayout(false);
            this.marketInfoPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.typeImage)).EndInit();
            this.marketOrdersSplitter.Panel1.ResumeLayout(false);
            this.marketOrdersSplitter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.marketOrdersSplitter)).EndInit();
            this.marketOrdersSplitter.ResumeLayout(false);
            this.RegionPanel.ResumeLayout(false);
            this.RegionPanel.PerformLayout();
            this.SellOrdersLayout.ResumeLayout(false);
            this.SellOrdersLayout.PerformLayout();
            this.buyOrdersLayout.ResumeLayout(false);
            this.buyOrdersLayout.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView marketTree;
        private System.Windows.Forms.SplitContainer marketSplitter;
        private System.Windows.Forms.FlowLayoutPanel marketInfoPanel;
        private System.Windows.Forms.PictureBox typeImage;
        private System.Windows.Forms.Label typeNameLabel;
        private System.Windows.Forms.TableLayoutPanel marketItemPanel;
        private System.Windows.Forms.SplitContainer marketOrdersSplitter;
        private System.Windows.Forms.ListView marketSellersList;
        private System.Windows.Forms.ListView marketBuyersList;
        private System.Windows.Forms.Label regionLabel;
        private System.Windows.Forms.ComboBox regionSelect;
        private System.Windows.Forms.Button RefreshButton;
        private System.Windows.Forms.TableLayoutPanel RegionPanel;
        private System.Windows.Forms.Label regionUpdateStatus;
        private System.Windows.Forms.TableLayoutPanel SellOrdersLayout;
        private System.Windows.Forms.Label sellOrdersLabel;
        private System.Windows.Forms.TableLayoutPanel buyOrdersLayout;
        private System.Windows.Forms.Label buyOrdersLabel;
    }
}
