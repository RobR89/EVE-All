using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EVE_All_API.StaticData;
using EVE_All_API;

namespace EVE_All.Tabs
{
    public partial class MarketBrowserTab : UserControl
    {
        public MarketBrowserTab()
        {
            InitializeComponent();
            Dock = DockStyle.Fill;
            setupTree();
            // Add callback for lazy loading.
            marketTree.BeforeExpand += MarketTree_BeforeExpand;
            marketTree.AfterSelect += MarketTree_AfterSelect;
        }

        private void MarketTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            InvType type = e.Node.Tag as InvType;
            if(type != null)
            {
                typeNameLabel.Text = type.name;
                Image img = ImageManager.getTypeImage(type.typeID, 64);
                typeImage.Image = img;
            }
        }

#region Market groups tree
        private void MarketTree_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            populateTree(e.Node, marketTree.ImageList, e.Node.Tag as InvMarketGroup);
        }

        private void setupTree()
        {
            // Set up the market tree.
            marketTree.BeginUpdate();
            marketTree.Nodes.Clear();
            // Set up the images.
            marketTree.ImageList = new ImageList();
            // Get child groups.
            List<InvMarketGroup> groups = InvMarketGroup.getRootGroups();
            groups = groups.OrderBy(g => g.marketGroupName).ToList();
            // Add the groups.
            populateTreeGroups(marketTree.Nodes, marketTree.ImageList, groups);
            marketTree.EndUpdate();
        }

        private void populateTreeGroups(TreeNodeCollection pNode, ImageList images, List<InvMarketGroup> groups)
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
                Image img = ImageManager.getIconImage(group.iconID);
                if (img != null)
                {
                    images.Images.Add(imgName, img);
                }
                node.ImageKey = imgName;
                node.SelectedImageKey = imgName;
            }
        }

        private void populateTree(TreeNode pNode, ImageList images, InvMarketGroup pGroup)
        {
            // Lazy loading of child nodes to improve performance.
            if(pGroup == null)
            {
                // Already added children.
                return;
            }
            int groupID = pGroup.marketGroupID;
            // Get child groups.
            List<InvMarketGroup> groups = InvMarketGroup.getGroupChildren(groupID);
            groups = groups.OrderBy(g => g.marketGroupName).ToList();
            // Get types for group.
            List<InvType> groupTypes = InvType.getMarketGroupTypes(groupID);
            groupTypes = groupTypes.OrderBy(g => g.name).ToList();
            // Begin update.
            marketTree.BeginUpdate();
            // Clear the subtree.
            pNode.Nodes.Clear();
            // Add the child groups.
            populateTreeGroups(pNode.Nodes, images, groups);
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
                Image img = ImageManager.getTypeImage(type.typeID, 64, true);
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
#endregion

    }
}
