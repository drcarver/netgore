using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NetGore.Graphics;
using NetGore.IO;
using Point=System.Drawing.Point;

namespace NetGore.EditorTools
{
    /// <summary>
    /// TreeView used to display Grhs
    /// </summary>
    public class GrhTreeView : TreeView, IComparer
    {
        /// <summary>
        /// Timer to update the animated Grhs in the grh tree
        /// </summary>
        readonly Timer _animTimer = new Timer();

        readonly ContextMenu _contextMenu = new ContextMenu();
        readonly CreateWallEntityHandler _createWall;
        readonly Vector2 _gameScreenSize;
        readonly MapGrhWalls _mapGrhWalls;

        ContentManager _contentManager;
        EditGrhForm _editGrhDataForm;

        /// <summary>
        /// Occurs after a new Grh n is selected
        /// </summary>
        public event GrhTreeViewEvent GrhAfterSelect;

        /// <summary>
        /// Occurs before a new Grh n is selected
        /// </summary>
        public event GrhTreeViewCancelEvent GrhBeforeSelect;

        /// <summary>
        /// Occurs when a Grh n is clicked
        /// </summary>
        public event GrhTreeNodeMouseClickEvent GrhMouseClick;

        /// <summary>
        /// Occurs when a Grh n is double-clicked
        /// </summary>
        public event GrhTreeNodeMouseClickEvent GrhMouseDoubleClick;

        /// <summary>
        /// Initializes a new instance of the <see cref="GrhTreeView"/> class.
        /// </summary>
        /// <param name="gameScreenSize">The size of the game screen.</param>
        /// <param name="createWall">Delegate used to create a <see cref="WallEntityBase"/>.</param>
        /// <param name="mapGrhWalls">The <see cref="MapGrhWalls"/> instance to use.</param>
        public GrhTreeView(Vector2 gameScreenSize, CreateWallEntityHandler createWall, MapGrhWalls mapGrhWalls)
        {
            if (createWall == null)
                throw new ArgumentNullException("createWall");
            if (mapGrhWalls == null)
                throw new ArgumentNullException("mapGrhWalls");

            _gameScreenSize = gameScreenSize;
            _createWall = createWall;
            _mapGrhWalls = mapGrhWalls;
            GrhInfo.OnRemove += GrhInfo_OnRemove;

            // ReSharper disable DoNotCallOverridableMethodsInConstructor

            AllowDrop = true;

            // Remove all nodes
            Nodes.Clear();

            // Create the animate timer
            _animTimer.Interval = 150;
            _animTimer.Tick += UpdateAnimations;
            _animTimer.Start();

            // Set the sort method
            TreeViewNodeSorter = this;

            // Create the ImageList containing the Grhs as an image
            ImageList = GrhImageList.ImageList;

            // Event hooks
            GrhMouseDoubleClick += GrhTreeView_GrhMouseDoubleClick;

            // Set up the context menu for the GrhTreeView
            _contextMenu.MenuItems.Add(new MenuItem("Edit", MenuClickEdit));
            _contextMenu.MenuItems.Add(new MenuItem("New Grh", MenuClickNewGrh));
            _contextMenu.MenuItems.Add(new MenuItem("Duplicate", MenuClickDuplicate));
            _contextMenu.MenuItems.Add(new MenuItem("Automatic Update", MenuClickAutomaticUpdate));
            ContextMenu = _contextMenu;

            // ReSharper restore DoNotCallOverridableMethodsInConstructor
        }

        /// <summary>
        /// Gets or sets the size of the Grh preview images
        /// </summary>
        [Description("Size of the Grh images in pixels")]
        public Size ImageSize
        {
            get { return ImageList.ImageSize; }
            set { ImageList.ImageSize = value; }
        }

        /// <summary>
        /// Gets if the form for editing a <see cref="GrhData"/> is currently visible.
        /// </summary>
        public bool IsEditingGrhData
        {
            get { return _editGrhDataForm != null; }
        }

        /// <summary>
        /// Gets if this <see cref="GrhTreeView"/> needs to draw.
        /// </summary>
        public bool NeedsToDraw
        {
            get { return _editGrhDataForm != null; }
        }

        /// <summary>
        /// Adds a Grh to the tree or updates it if it already exists.
        /// </summary>
        /// <param name="gd"><see cref="GrhData"/> to add or update.</param>
        void AddGrhToTree(GrhData gd)
        {
            GrhTreeViewNode.Create(this, gd);
        }

        /// <summary>
        /// Attempts to begin the editing of a <see cref="GrhData"/>.
        /// </summary>
        /// <param name="node">The <see cref="TreeNode"/> containing the <see cref="GrhData"/> to edit.</param>
        /// <returns>True if the editing started successfully; otherwise false.</returns>
        public bool BeginEditGrhData(TreeNode node)
        {
            return BeginEditGrhData(node, GetGrhData(node), false);
        }

        /// <summary>
        /// Attempts to begin the editing of a <see cref="GrhData"/>.
        /// </summary>
        /// <param name="gd">The <see cref="GrhData"/> to edit.</param>
        /// <returns>True if the editing started successfully; otherwise false.</returns>
        public bool BeginEditGrhData(GrhData gd)
        {
            return BeginEditGrhData(gd, false);
        }

        /// <summary>
        /// Attempts to begin the editing of a <see cref="GrhData"/>.
        /// </summary>
        /// <param name="gd">The <see cref="GrhData"/> to edit.</param>
        /// <param name="deleteOnCancel">If true, the <paramref name="gd"/> will be deleted if the edit form is
        /// closed by pressing "Cancel".</param>
        /// <returns>True if the editing started successfully; otherwise false.</returns>
        bool BeginEditGrhData(GrhData gd, bool deleteOnCancel)
        {
            return BeginEditGrhData(FindGrhDataNode(gd), gd, deleteOnCancel);
        }

        /// <summary>
        /// Attempts to begin the editing of a <see cref="GrhData"/>.
        /// </summary>
        /// <param name="node">The <see cref="TreeNode"/> containing the <see cref="GrhData"/> to edit.</param>
        /// <param name="gd">The <see cref="GrhData"/> to edit.</param>
        /// <param name="deleteOnCancel">If true, the <paramref name="gd"/> will be deleted if the edit form is
        /// closed by pressing "Cancel".</param>
        /// <returns>True if the editing started successfully; otherwise false.</returns>
        bool BeginEditGrhData(TreeNode node, GrhData gd, bool deleteOnCancel)
        {
            if ((_editGrhDataForm != null && !_editGrhDataForm.IsDisposed) || node == null || gd == null)
                return false;

            _editGrhDataForm = new EditGrhForm(gd, _mapGrhWalls, _createWall, _gameScreenSize);
            _editGrhDataForm.FormClosed += delegate
                                           {
                                               if (_editGrhDataForm == null)
                                                   return;

                                               if (deleteOnCancel && _editGrhDataForm.WasCanceled)
                                               {
                                                   // Delete the GrhData
                                                   GrhInfo.Delete(gd);
                                               }
                                               else
                                               {
                                                   // Update the GrhData
                                                   UpdateGrhData(gd);
                                               }

                                               _editGrhDataForm = null;
                                           };

            _editGrhDataForm.Show();

            return true;
        }

        void CheckForMissingTextures()
        {
            // We must create the hash collection since its constructor has the updating goodies, and we want
            // to make sure that is called
            TextureHashCollection hashCollection = new TextureHashCollection();

            // Get the GrhDatas with missing textures
            var missing = GrhInfo.FindMissingTextures();
            if (missing.Count() == 0)
                return;

            // Display a form showing which textures need to be fixed
            // The GrhTreeView will be disabled until the MissingTexturesForm is closed
            Enabled = false;
            MissingTexturesForm frm = new MissingTexturesForm(hashCollection, missing, _contentManager);
            frm.FormClosed += delegate
                              {
                                  RebuildTree();
                                  Enabled = true;
                              };
            frm.Show();
        }

        /// <summary>
        /// Deletes a node from the tree, along with any node under it.
        /// </summary>
        /// <param name="root">Root node to delete.</param>
        static void DeleteNode(GrhTreeViewNode root)
        {
            GrhInfo.Delete(root.GrhData);
        }

        /// <summary>
        /// Deletes a node from the tree, along with any node under it.
        /// </summary>
        /// <param name="root">Root node to delete.</param>
        static void DeleteNode(GrhTreeViewFolderNode root)
        {
            var toDelete = root.GetChildGrhDataNodes(true).ToArray();
            foreach (var node in toDelete)
            {
                GrhInfo.Delete(node.GrhData);
            }
        }

        static void DeleteNode(TreeNode node)
        {
            if (node is GrhTreeViewNode)
                DeleteNode((GrhTreeViewNode)node);
            else if (node is GrhTreeViewFolderNode)
                DeleteNode((GrhTreeViewFolderNode)node);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
                GrhImageList.Save();
        }

        /// <summary>
        /// Draws the <see cref="GrhTreeView"/>.
        /// </summary>
        /// <param name="sb">The <see cref="SpriteBatch"/> to draw to.</param>
        public void Draw(SpriteBatch sb)
        {
            if (_editGrhDataForm != null)
                _editGrhDataForm.Draw(sb);
        }

        void DuplicateGrhDataNode(GrhTreeViewNode node, string oldCategoryStart, string newCategoryStart)
        {
            var gd = node.GrhData;

            // Replace the start of the categorization to the new category
            var newCategory = newCategoryStart;
            if (gd.Categorization.Category.ToString().Length > oldCategoryStart.Length)
                newCategory += gd.Categorization.Category.ToString().Substring(oldCategoryStart.Length);

            // Grab the new title
            var newTitle = GrhInfo.GetUniqueTitle(newCategory, gd.Categorization.Title);

            // Duplicate
            var newGrhData = gd.Duplicate(new SpriteCategorization(newCategory, newTitle));

            // Add the new one to the tree
            UpdateGrhData(newGrhData);
        }

        void DuplicateNode(TreeNode node, string oldCategoryStart, string newCategoryStart)
        {
            if (node is GrhTreeViewNode)
                DuplicateGrhDataNode((GrhTreeViewNode)node, oldCategoryStart, newCategoryStart);
            else if (node is GrhTreeViewFolderNode)
            {
                var grhDataNodes = ((GrhTreeViewFolderNode)node).GetChildGrhDataNodes(true).ToImmutable();

                foreach (var child in grhDataNodes)
                {
                    DuplicateGrhDataNode(child, oldCategoryStart, newCategoryStart);
                }
            }
        }

        /// <summary>
        /// Creates a duplicate of the tree nodes and structure from the root, giving it a unique name. All GrhDatas
        /// under the node to be duplicated will be duplicated and placed in a new category with a new GrhIndex,
        /// but with the same name and structure.
        /// </summary>
        /// <param name="root">Root <see cref="TreeNode"/> to duplicate from.</param>
        public void DuplicateNodes(TreeNode root)
        {
            SpriteCategory category;
            SpriteCategory newCategory;

            if (root is GrhTreeViewNode)
            {
                category = ((GrhTreeViewNode)root).GrhData.Categorization.Category;
                newCategory = category;
            }
            else
            {
                category = ((GrhTreeViewFolderNode)root).FullCategory;
                newCategory = GrhInfo.GetUniqueCategory(category);
            }

            DuplicateNode(root, category.ToString(), newCategory.ToString());
        }

        public GrhTreeViewFolderNode FindFolder(string category)
        {
            var delimiters = new string[] { SpriteCategorization.Delimiter };
            var categoryParts = category.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

            GrhTreeViewFolderNode current = null;
            var currentColl = Nodes;

            for (int i = 0; i < categoryParts.Length; i++)
            {
                string subCategory = categoryParts[i];
                current = currentColl.OfType<GrhTreeViewFolderNode>().Where(x => x.SubCategory == subCategory).FirstOrDefault();

                if (current == null)
                    return null;

                currentColl = current.Nodes;
            }

            return current;
        }

        public GrhTreeViewNode FindGrhDataNode(GrhData grhData)
        {
            var folder = FindFolder(grhData.Categorization.Category.ToString());
            if (folder != null)
            {
                var existingNode = folder.Nodes.OfType<GrhTreeViewNode>().Where(x => x.GrhData == grhData).FirstOrDefault();
                if (existingNode != null)
                    return existingNode;
            }

            return null;
        }

        /// <summary>
        /// Gets the category to use from the given <paramref name="node"/>.
        /// </summary>
        /// <param name="node">The <see cref="TreeNode"/> to get the category from.</param>
        /// <returns>The category to use from the given <paramref name="node"/>.</returns>
        public static SpriteCategory GetCategoryFromTreeNode(TreeNode node)
        {
            // Check for a valid node
            if (node != null)
            {
                if (node is GrhTreeViewNode)
                    return ((GrhTreeViewNode)node).GrhData.Categorization.Category;
                else if (node is GrhTreeViewFolderNode)
                    return ((GrhTreeViewFolderNode)node).FullCategory;
            }

            return "Uncategorized";
        }

        /// <summary>
        /// Finds the <see cref="GrhData"/> for a given <see cref="TreeNode"/>.
        /// </summary>
        /// <param name="node"><see cref="TreeNode"/> to get the <see cref="GrhData"/> from.</param>
        /// <returns><see cref="GrhData"/> for the <see cref="TreeNode"/>, or null if invalid.</returns>
        public static GrhData GetGrhData(TreeNode node)
        {
            var casted = node as GrhTreeViewNode;
            if (casted != null)
                return casted.GrhData;

            return null;
        }

        static IEnumerable<GrhTreeViewNode> GetVisibleAnimatedGrhTreeViewNodes(IEnumerable root)
        {
            foreach (var node in root.OfType<TreeNode>())
            {
                if (!node.IsVisible)
                    continue;

                var asGrhTreeViewNode = node as GrhTreeViewNode;
                if (asGrhTreeViewNode != null && asGrhTreeViewNode.NeedsToUpdateImage)
                    yield return asGrhTreeViewNode;
                else if (node.Nodes.Count > 0)
                {
                    foreach (var child in GetVisibleAnimatedGrhTreeViewNodes(node.Nodes))
                    {
                        yield return child;
                    }
                }
            }
        }

        void GrhInfo_OnRemove(GrhData sender)
        {
            var node = FindGrhDataNode(sender);
            if (node != null)
                node.Update();
        }

        void GrhTreeView_GrhMouseDoubleClick(object sender, GrhTreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                BeginEditGrhData(e.Node);
        }

        void HighlightFolder(TreeNode root, TreeNode folder)
        {
            if (folder != null && (root == folder || (root.Parent == folder && IsGrhDataNode(root))))
            {
                // Highlight the folder, the Grhs in the folder, but not the folders in the folder
                root.ForeColor = SystemColors.HighlightText;
                root.BackColor = SystemColors.Highlight;
            }
            else
            {
                // Do not highlight anything else / remove old highlighting
                root.ForeColor = ForeColor;
                root.BackColor = BackColor;
            }

            // Recurse through the rest of the nodes
            foreach (TreeNode child in root.Nodes)
            {
                HighlightFolder(child, folder);
            }
        }

        /// <summary>
        /// Initializes the <see cref="GrhTreeView"/>.
        /// </summary>
        public void Initialize(ContentManager cm)
        {
            _contentManager = cm;

            // Check for missing textures
            CheckForMissingTextures();

            // Iterate through all the GrhDatas
            foreach (GrhData grhData in GrhInfo.GrhDatas)
            {
                AddGrhToTree(grhData);
            }

            // Perform the initial sort
            Sort();
        }

        /// <summary>
        /// Checks if a <see cref="TreeNode"/> contains a <see cref="GrhData"/>, or is a folder.
        /// </summary>
        /// <param name="node">The <see cref="TreeNode"/> to check.</param>
        /// <returns>True if a Grh, false if a folder.</returns>
        static bool IsGrhDataNode(TreeNode node)
        {
            return node is GrhTreeViewNode;
        }

        void MenuClickAutomaticUpdate(object sender, EventArgs e)
        {
            // HACK: I shouldn't be grabbing the ContentManager like this... but how else should I go about getting it? o.O
            ContentManager cm = GrhInfo.GrhDatas.First(x => x.ContentManager != null).ContentManager;
            if (cm == null)
                throw new Exception("Failed to find a ContentManager to use.");

            var newGDs = AutomaticGrhDataUpdater.UpdateAll(cm, ContentPaths.Dev.Grhs);
            int newCount = newGDs.Count();

            if (newCount > 0)
            {
                UpdateGrhDatas(newGDs);
                MessageBox.Show(newCount + " new GrhDatas have been automatically added.");
            }
            else
                MessageBox.Show("No new GrhDatas automatically added - everything is already up to date.");
        }

        void MenuClickDuplicate(object sender, EventArgs e)
        {
            TreeNode node = SelectedNode;

            if (node == null)
                return;

            // Confirm the duplicate request
            int count = NodeCount(node);
            string text;
            if (count <= 0)
            {
                Debug.Fail(string.Format("Somehow, we have a count of `{0}` nodes...", count));
                return;
            }

            if (count == 1)
                text = "Are you sure you wish to duplicate this node?";
            else
                text = string.Format("Are you sure you wish to duplicate these {0} nodes?", NodeCount(node));
            if (MessageBox.Show(text, "Duplicate nodes?", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            DuplicateNodes(node);
        }

        void MenuClickEdit(object sender, EventArgs e)
        {
            TreeNode node = SelectedNode;

            if (node == null)
                return;

            GrhData gd = GetGrhData(node);

            if (gd != null && node.Nodes.Count == 0)
            {
                // The TreeNode is a GrhData
                BeginEditGrhData(node);
            }
            else if (gd == null)
            {
                // The TreeNode is a folder
                node.BeginEdit();
            }
        }

        void MenuClickNewGrh(object sender, EventArgs e)
        {
            if (_contentManager == null)
                return;

            // Create the new GrhData
            var category = GetCategoryFromTreeNode(SelectedNode);
            GrhData gd = GrhInfo.CreateGrhData(_contentManager, category);
            UpdateGrhData(gd);

            // Begin edit
            BeginEditGrhData(gd, true);
        }

        /// <summary>
        /// Counts the number of nodes under the root n, plus the root itself
        /// </summary>
        /// <param name="root">Root n to count from</param>
        /// <returns>Number of nodes under the root n, plus the root itself</returns>
        public static int NodeCount(TreeNode root)
        {
            // No root? No count
            if (root == null)
                return 0;

            // 1 because we are counting ourself
            int count = 1;

            // Recursively count the children
            foreach (TreeNode child in root.Nodes)
            {
                count += NodeCount(child);
            }

            return count;
        }

        protected override void OnAfterLabelEdit(NodeLabelEditEventArgs e)
        {
            base.OnAfterLabelEdit(e);

            if (e.Node != null && e.Label != null)
            {
                e.CancelEdit = true;
                e.Node.Text = e.Label;
                UpdateGrhsToTree(e.Node);
                Sort();
            }
        }

        protected override void OnAfterSelect(TreeViewEventArgs e)
        {
            base.OnAfterSelect(e);

            if (GrhAfterSelect == null || e.Node == null || !IsGrhDataNode(e.Node))
                return;

            GrhData gd = GetGrhData(e.Node);
            if (gd != null)
                GrhAfterSelect(this, new GrhTreeViewEventArgs(gd, e));
        }

        protected override void OnBeforeSelect(TreeViewCancelEventArgs e)
        {
            base.OnBeforeSelect(e);

            if (GrhBeforeSelect == null || e.Node == null || !IsGrhDataNode(e.Node))
                return;

            GrhData gd = GetGrhData(e.Node);
            if (gd != null)
                GrhBeforeSelect(this, new GrhTreeViewCancelEventArgs(gd, e));
        }

        protected override void OnDragDrop(DragEventArgs e)
        {
            base.OnDragDrop(e);

            foreach (TreeNode child in Nodes)
            {
                HighlightFolder(child, null);
            }

            if (!e.Data.GetDataPresent("System.Windows.Forms.TreeNode", false))
                return;

            Point pt = PointToClient(new Point(e.X, e.Y));
            TreeNode destNode = GetNodeAt(pt);
            TreeNode newNode = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");
            TreeNode addedNode;

            // Don't allow dropping onto itself
            if (newNode == destNode)
                return;

            // If the destination node is a GrhData node, move the destination to the folder
            if (IsGrhDataNode(destNode))
                destNode = destNode.Parent;

            // Check for a valid destination
            if (destNode == null)
                return;

            if (IsGrhDataNode(newNode))
            {
                // Move a GrhData node
                addedNode = (TreeNode)newNode.Clone();
                destNode.Nodes.Add(addedNode);
                destNode.Expand();
            }
            else
            {
                // Move a folder node

                // Do not allow a node to be moved into its own child
                TreeNode tmp = destNode;
                while (tmp.Parent != null)
                {
                    if (tmp.Parent == newNode)
                        return;
                    tmp = tmp.Parent;
                }

                addedNode = (TreeNode)newNode.Clone();
                destNode.Nodes.Add(addedNode);
            }

            // If a node was added, we will want to update
            destNode.Expand();
            newNode.Remove();
            UpdateGrhsToTree(addedNode);
            SelectedNode = addedNode;
        }

        protected override void OnDragEnter(DragEventArgs drgevent)
        {
            drgevent.Effect = DragDropEffects.Move;

            base.OnDragEnter(drgevent);
        }

        protected override void OnDragOver(DragEventArgs e)
        {
            base.OnDragOver(e);

            TreeNode nodeOver = GetNodeAt(PointToClient(new Point(e.X, e.Y)));
            if (nodeOver != null)
            {
                // Find the folder the n will drop into
                TreeNode folderNode = nodeOver;
                if (!IsGrhDataNode(folderNode))
                    folderNode = folderNode.Parent;

                // Perform the highlighting
                foreach (TreeNode child in Nodes)
                {
                    HighlightFolder(child, folderNode);
                }
            }
        }

        protected override void OnItemDrag(ItemDragEventArgs e)
        {
            base.OnItemDrag(e);

            if (e.Button == MouseButtons.Left)
            {
                SelectedNode = (TreeNode)e.Item;
                DoDragDrop(e.Item, DragDropEffects.Move);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.KeyCode == Keys.Delete)
            {
                const string txt = "Are you sure you wish to delete this GrhData?";
                if (MessageBox.Show(txt, "Delete GrhData?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    DeleteNode(SelectedNode);
            }
        }

        protected override void OnNodeMouseClick(TreeNodeMouseClickEventArgs e)
        {
            base.OnNodeMouseClick(e);

            // Change the SelectedNode to the clicked node (normally, right-click doesn't change the selected)
            if (e.Node != null)
                SelectedNode = e.Node;

            // If there is no GrhMouseClick event, or this was a folder, there is nothing left to do
            if (GrhMouseClick == null || e.Node == null || !IsGrhDataNode(e.Node))
                return;

            // Get the GrhData for the n clicked, raising the GrhMouseClick event if valid
            GrhData gd = GetGrhData(e.Node);
            if (gd != null)
                GrhMouseClick(this, new GrhTreeNodeMouseClickEventArgs(gd, e));
        }

        protected override void OnNodeMouseDoubleClick(TreeNodeMouseClickEventArgs e)
        {
            base.OnNodeMouseDoubleClick(e);

            // If there is no GrhMouseDoubleClick event, or this was a folder, there is nothing left to do
            if (GrhMouseDoubleClick == null || e.Node == null || !IsGrhDataNode(e.Node))
                return;

            // Get the GrhData for the n double-clicked, raising the GrhMouseDoubleClick event if valid
            GrhData gd = GetGrhData(e.Node);
            if (gd != null)
                GrhMouseDoubleClick(this, new GrhTreeNodeMouseClickEventArgs(gd, e));
        }

        /// <summary>
        /// Completely rebuilds the <see cref="GrhTreeView"/>.
        /// </summary>
        public void RebuildTree()
        {
            // Clear all nodes
            Nodes.Clear();

            // Re-add every GrhData
            foreach (GrhData grh in GrhInfo.GrhDatas)
            {
                AddGrhToTree(grh);
            }

            Sort();
        }

        /// <summary>
        /// Updates the visible animated <see cref="GrhTreeViewNode"/>s.
        /// </summary>
        void UpdateAnimations(object sender, EventArgs e)
        {
            foreach (var toUpdate in GetVisibleAnimatedGrhTreeViewNodes(Nodes))
            {
                toUpdate.UpdateIconImage();
            }
        }

        /// <summary>
        /// Updates a <see cref="GrhData"/>'s information in the tree.
        /// </summary>
        /// <param name="grhData"><see cref="GrhData"/> to update.</param>
        public void UpdateGrhData(GrhData grhData)
        {
            GrhTreeViewNode.Create(this, grhData);
        }

        /// <summary>
        /// Updates a GrhData's information in the tree
        /// </summary>
        /// <param name="grhIndex">Index of the GrhData to update</param>
        public void UpdateGrhData(GrhIndex grhIndex)
        {
            UpdateGrhData(GrhInfo.GetData(grhIndex));
        }

        public void UpdateGrhDatas(IEnumerable<GrhData> grhDatas)
        {
            foreach (GrhData grhData in grhDatas)
            {
                UpdateGrhData(grhData);
            }
        }

        /// <summary>
        /// Updates the GrhData categorizing to match the displayed tree. Used to make the GrhData categorization
        /// be set based on the tree.
        /// </summary>
        /// <param name="root">Root TreeNode to start updating at</param>
        static void UpdateGrhsToTree(TreeNode root)
        {
            if (!IsGrhDataNode(root))
            {
                // Node is a branch, so it should be a folder
                foreach (TreeNode node in root.Nodes)
                {
                    UpdateGrhsToTree(node);
                }
            }
            else
            {
                var grhDataNode = root as GrhTreeViewNode;
                if (grhDataNode == null)
                    return;

                grhDataNode.SyncGrhData();
            }
        }

        #region IComparer Members

        /// <summary>
        /// Compares two <see cref="TreeNode"/>s.
        /// </summary>
        /// <param name="a">First object.</param>
        /// <param name="b">Second object.</param>
        /// <returns>-1 if a is first, 1 if b is first, or 0 for no preference.</returns>
        int IComparer.Compare(object a, object b)
        {
            TreeNode x = (TreeNode)a;
            TreeNode y = (TreeNode)b;

            // Folders take priority
            if (!IsGrhDataNode(x) && IsGrhDataNode(y))
                return -1;

            if (!IsGrhDataNode(y) && IsGrhDataNode(x))
                return 1;

            // Text sort
            return x.Text.CompareTo(y.Text);
        }

        #endregion
    }
}