using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using EVE_All_API.StaticData;
using EVE_All_API;
using EVE_All_API.ESI;
using static EVE_All_API.ESI.Market;

namespace EVE_All.Tabs
{
    public partial class MarketBrowserTab : UserControl
    {
        private System.Timers.Timer labelTimer = new System.Timers.Timer()
        {
            Interval = 1000,
            Enabled = true
        };
        public MarketBrowserTab()
        {
            InitializeComponent();
            Dock = DockStyle.Fill;
            SetupTree();
            // Add callback for lazy loading.
            marketTree.BeforeExpand += MarketTree_BeforeExpand;
            marketTree.AfterSelect += MarketTree_AfterSelect;
            SetupColumns();
            // Populate region combo.
            List<int> regionIDs = Universe.GetRegions();
            List<RegionItem> rItems = new List<RegionItem>();
            regionSelect.Items.Clear();
            rItems.Add(new RegionItem("All (Loaded)", -1));
            rItems.Add(new RegionItem("None", 0));
            foreach (int regionID in regionIDs)
            {
                Universe.Region region = Universe.GetRegion(regionID);
                if (region.region_id < 11000000)
                {
                    rItems.Add(new RegionItem(region.name, region.region_id));
                }
            }
            rItems.Sort();
            regionSelect.Items.AddRange(rItems.ToArray());
            // Watch for updates...
            Market.RegionUpdate += Market_RegionUpdate;
            // Auto update region status label.
            labelTimer.Elapsed += LabelTimer_Elapsed;
        }

        private void LabelTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            ShowUpdatingRegions();
        }

        private void Market_RegionUpdate(int regionID)
        {
            if (InvokeRequired)
            {
                // Insure this is called in a GUI friendly thread.
                Invoke((MethodInvoker)delegate { Market_RegionUpdate(regionID); });
                return;
            }
            if (updatingRegions.Contains(regionID))
            {
                // Remove region from updating status.
                updatingRegions.RemoveAll(r => r == regionID);
            }
            // Get region.
            RegionItem item = regionSelect.SelectedItem as RegionItem;
            if (item?.regionID == regionID)
            {
                // Our current region updated... Refresh the list.
                PopulateOrders();
            }
        }

        protected class RegionItem : IComparable
        {
            public RegionItem(string _name, int id)
            {
                name = _name;
                regionID = id;
            }
            public string name;
            public int regionID;
            public override string ToString()
            {
                return name;
            }

            public int CompareTo(object obj)
            {
                RegionItem item = obj as RegionItem;
                if(regionID <= 0 || item.regionID <= 0)
                {
                    return regionID.CompareTo(item.regionID);
                }
                return name.CompareTo(item.name);
            }
        }

        private void RegionSelect_SelectedValueChanged(object sender, EventArgs e)
        {
            UpdateSelectedRegion();
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            UpdateSelectedRegion();
        }

        private void UpdateSelectedRegion()
        {
            RegionItem item = regionSelect.SelectedItem as RegionItem;
            if (item != null)
            {
                if (item.regionID > 0)
                {
                    if (!updatingRegions.Contains(item.regionID))
                    {
                        updatingRegions.Add(item.regionID);
                    }
                    MarketRegionPage regionPage = Market.GetRegionPage(item.regionID, true);
                    if (regionPage != null)
                    {
                        regionPage.ScheduleRefresh();
                    }
                }
                PopulateOrders();
            }
        }
        private void SetupColumns()
        {
            // Set up seller columns
            marketSellersList.Columns.Add("Quantity", 75);
            marketSellersList.Columns.Add("Price", 75);
            marketSellersList.Columns.Add("Location", 300);
            marketSellersList.Columns.Add("Expires In", 120);
            // Set up buyers columns
            marketBuyersList.Columns.Add("Quantity", 75);
            marketBuyersList.Columns.Add("Price", 75);
            marketBuyersList.Columns.Add("Location", 300);
            marketBuyersList.Columns.Add("Range", 75);
            marketBuyersList.Columns.Add("Min Volume", 75);
            marketBuyersList.Columns.Add("Expires In", 120);
        }

#region GUI updates
        private List<int> updatingRegions = new List<int>();
        private void ShowUpdatingRegions()
        {
            if (updatingRegions.Count > 0)
            {
                string regions = "";
                string delim = "";
                int num = 0;
                foreach (int regionID in updatingRegions)
                {
                    // Show at most 6 entries.
                    num++;
                    if (num > 6)
                    {
                        regions += ", ...";
                        break;
                    }
                    Universe.Region region = Universe.GetRegion(regionID);
                    if (region != null)
                    {
                        regions += delim + region.name;
                        MarketRegionPage regionPage = Market.GetRegionPage(regionID, false);
                        if (regionPage != null)
                        {
                            TimeSpan wait = regionPage.expire.Subtract(DateTime.Now);
                            if (wait.TotalMilliseconds < 1)
                            {
                                wait = new TimeSpan(1);
                            }
                            regions += "(" + wait.TotalSeconds.ToString("N1") + ")";
                        }
                        delim = ", ";
                    }
                }
                regionUpdateStatus.Text = "Updating region(s): " + regions;
            }
            else
            {
                regionUpdateStatus.Text = "";
            }
        }

        private void PopulateOrders()
        {
            if (InvokeRequired)
            {
                // Insure this is called in a GUI friendly thread.
                Invoke((MethodInvoker)delegate { PopulateOrders(); });
                return;
            }
            // Begin update.
            marketBuyersList.BeginUpdate();
            marketSellersList.BeginUpdate();
            // Clear list.
            marketBuyersList.Items.Clear();
            marketSellersList.Items.Clear();
            // Update region status.
            ShowUpdatingRegions();
            // Get type.
            InvType type = marketTree.SelectedNode?.Tag as InvType;
            // Get region.
            RegionItem item = regionSelect.SelectedItem as RegionItem;
            if (item != null && type != null && item.regionID != 0)
            {
                PopulateOrders(type.typeID, item.regionID);
            }
            // Complete update.
            marketBuyersList.EndUpdate();
            marketSellersList.EndUpdate();
        }
        private void PopulateOrders(int typeID, int regionID)
        {
            List<Market.MarketOrder> orders;
            if (regionID > 0)
            {
                orders = Market.GetOrdersForTypeAndRegion(typeID, regionID);
            }
            else if(regionID == 0)
            {
                // No region selected, leave empty.
                return;
            }
            else
            {
                // All regions selected
                orders = Market.GetOrdersForType(typeID);
            }
            string[] selCols = new string[4];
            string[] buyCols = new string[6];
            foreach (Market.MarketOrder order in orders)
            {
                TimeSpan expiresIn = new TimeSpan(order.duration,0,0,0) - (DateTime.Now - order.issued);
                string expireStr = expiresIn.Days.ToString() + "D " + expiresIn.Hours.ToString() + "H " + expiresIn.Minutes.ToString() + "M " + expiresIn.Seconds.ToString() + "S";
                string locationStr = InvNames.GetName((int)order.location_id);
                if(locationStr == null)
                {
                    // Get structure name.
                    // TO-DO:
                    locationStr = "Structure: " + order.location_id.ToString();
                }
                if (order.is_buy_order)
                {
                    buyCols[0] = order.volume_remain.ToString();
                    buyCols[1] = order.price.ToString("N2");
                    buyCols[2] = locationStr;
                    buyCols[3] = order.range;
                    buyCols[4] = order.min_volume.ToString();
                    buyCols[5] = expireStr;
                    ListViewItem item = new ListViewItem(buyCols);
                    marketBuyersList.Items.Add(item);
                }
                else
                {
                    selCols[0] = order.volume_remain.ToString();
                    selCols[1] = order.price.ToString("N2");
                    selCols[2] = locationStr;
                    selCols[3] = expireStr;
                    ListViewItem item = new ListViewItem(selCols);
                    marketSellersList.Items.Add(item);
                }

            }
        }
#endregion GUI updates

        #region Market groups tree
        private void MarketTree_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            PopulateTree(e.Node, marketTree.ImageList, e.Node.Tag as InvMarketGroup);
        }

        private void MarketTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            InvType type = e.Node.Tag as InvType;
            if (type != null)
            {
                typeNameLabel.Text = type.name;
                Image img = ImageManager.GetTypeImage(type.typeID, 64);
                typeImage.Image = img;
            }
            PopulateOrders();
        }

        private void SetupTree()
        {
            // Set up the market tree.
            marketTree.BeginUpdate();
            marketTree.Nodes.Clear();
            // Set up the images.
            marketTree.ImageList = new ImageList();
            // Get child groups.
            List<InvMarketGroup> groups = InvMarketGroup.GetRootGroups();
            groups = groups.OrderBy(g => g.marketGroupName).ToList();
            // Add the groups.
            PopulateTreeGroups(marketTree.Nodes, marketTree.ImageList, groups);
            marketTree.EndUpdate();
        }

        private void PopulateTreeGroups(TreeNodeCollection pNode, ImageList images, List<InvMarketGroup> groups)
        {
            foreach (InvMarketGroup group in groups)
            {
                if(group.marketGroupID == 350001)
                {
                    // Skip Infantry gear group.  (Obsolete)
                    continue;
                }
                // Create the new group node.
                TreeNode node = pNode.Add(group.marketGroupName);
                node.Tag = group;
                // Add temp holder.
                TreeNode gNode = node.Nodes.Add("Groups:");
                // Add node image.
                string imgName = "icon:" + group.iconID.ToString();
                Image img = ImageManager.GetIconImage(group.iconID);
                if (img != null)
                {
                    images.Images.Add(imgName, img);
                }
                node.ImageKey = imgName;
                node.SelectedImageKey = imgName;
            }
        }

        private void PopulateTree(TreeNode pNode, ImageList images, InvMarketGroup pGroup)
        {
            // Lazy loading of child nodes to improve performance.
            if(pGroup == null)
            {
                // Already added children.
                return;
            }
            int groupID = pGroup.marketGroupID;
            // Get child groups.
            List<InvMarketGroup> groups = InvMarketGroup.GetGroupChildren(groupID);
            groups = groups.OrderBy(g => g.marketGroupName).ToList();
            // Get types for group.
            List<InvType> groupTypes = InvType.GetMarketGroupTypes(groupID);
            groupTypes = groupTypes.OrderBy(g => g.name).ToList();
            // Begin update.
            marketTree.BeginUpdate();
            // Clear the subtree.
            pNode.Nodes.Clear();
            // Add the child groups.
            PopulateTreeGroups(pNode.Nodes, images, groups);
            // Add types to group.
            foreach (InvType type in groupTypes)
            {
                if (!type.published)
                {
                    continue;
                }
                TreeNode tNode = pNode.Nodes.Add(type.name);
                tNode.Tag = type;
                // Add image.
                string imgName = "type:" + type.typeID.ToString();
                Image img = ImageManager.GetTypeImage(type.typeID, 64, true);
                if (img != null)
                {
                    images.Images.Add(imgName, img);
                }
                tNode.ImageKey = imgName;
                tNode.SelectedImageKey = imgName;
            }
            // Clear the tag so we don't have to reload the subtree again.
            pNode.Tag = null;
            marketTree.EndUpdate();
        }
        #endregion Market groups tree

    }
}
