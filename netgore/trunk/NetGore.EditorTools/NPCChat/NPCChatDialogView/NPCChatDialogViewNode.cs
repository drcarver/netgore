using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using NetGore.NPCChat;

namespace NetGore.EditorTools.NPCChat
{
    /// <summary>
    /// A TreeNode for the <see cref="NPCChatDialogView"/>.
    /// </summary>
    public class NPCChatDialogViewNode : TreeNode
    {
        NPCChatDialogViewNodeItemType _chatItemType;

        /// <summary>
        /// Initializes a new instance of the <see cref="NPCChatDialogViewNode"/> class.
        /// </summary>
        /// <param name="treeView">The tree view.</param>
        /// <param name="handledItem">The handled item.</param>
        internal NPCChatDialogViewNode(TreeView treeView, object handledItem)
        {
            Tag = handledItem;
            UpdateChatItemType();
            treeView.Nodes.Add(this);
            TreeViewCasted.NotifyNodeCreated(this);
            Update(true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NPCChatDialogViewNode"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="handledItem">The handled item.</param>
        internal NPCChatDialogViewNode(TreeNode parent, object handledItem)
        {
            Tag = handledItem;
            UpdateChatItemType();
            parent.Nodes.Add(this);
            TreeViewCasted.NotifyNodeCreated(this);
            UpdateText();
        }

        /// <summary>
        /// Gets the NPC chat item that this <see cref="NPCChatDialogViewNode"/> handles.
        /// </summary>
        public object ChatItem
        {
            get { return Tag; }
            internal set
            {
                if (Tag == value)
                    return;

                Tag = value;
                UpdateChatItemType();

                UpdateText();
            }
        }

        /// <summary>
        /// Gets the ChatItem as a <see cref="EditorNPCChatDialogItem"/>. If this is a ChatItemType is
        /// <see cref="NPCChatDialogViewNodeItemType.Redirect"/>, this will
        /// return the <see cref="EditorNPCChatDialogItem"/> that the n redirects to.
        /// </summary>
        /// <exception cref="MethodAccessException">Unsupported ChatItemType for this operation.</exception>
        public EditorNPCChatDialogItem ChatItemAsDialogItem
        {
            get
            {
                if (ChatItemType == NPCChatDialogViewNodeItemType.DialogItem)
                    return (EditorNPCChatDialogItem)ChatItem;
                else if (ChatItemType == NPCChatDialogViewNodeItemType.Redirect)
                    return ChatItemAsRedirect.ChatItemAsDialogItem;
                else
                    throw new MethodAccessException("Invalid ChatItemType for this method.");
            }
        }

        /// <summary>
        /// Gets the ChatItem as the NPCChatDialogViewNode containing the EditorNPCChatDialogItem that this
        /// NPCChatDialogViewNode redirects to.
        /// </summary>
        /// <exception cref="MethodAccessException">Unsupported ChatItemType for this operation.</exception>
        public NPCChatDialogViewNode ChatItemAsRedirect
        {
            get
            {
                if (ChatItemType == NPCChatDialogViewNodeItemType.Redirect)
                    return (NPCChatDialogViewNode)ChatItem;
                else
                    throw new MethodAccessException("Invalid ChatItemType for this method.");
            }
        }

        /// <summary>
        /// Gets the ChatItem as a EditorNPCChatResponse.
        /// </summary>
        /// <exception cref="MethodAccessException">Unsupported ChatItemType for this operation.</exception>
        public EditorNPCChatResponse ChatItemAsResponse
        {
            get
            {
                if (ChatItemType == NPCChatDialogViewNodeItemType.Response)
                    return (EditorNPCChatResponse)ChatItem;
                else
                    throw new MethodAccessException("Invalid ChatItemType for this method.");
            }
        }

        /// <summary>
        /// Gets the type of item stored in the ChatItem.
        /// </summary>
        public NPCChatDialogViewNodeItemType ChatItemType
        {
            get { return _chatItemType; }
        }

        /// <summary>
        /// Gets if the <see cref="NPCChatDialogViewNode"/> is dead (has been removed from the
        /// <see cref="TreeView"/> and is not coming back).
        /// </summary>
        public bool IsDead
        {
            get { return TreeView == null; }
        }

        /// <summary>
        /// Gets the Parent TreeNode casted to a NPCChatDialogViewNode.
        /// </summary>
        internal NPCChatDialogViewNode ParentCasted
        {
            get { return Parent as NPCChatDialogViewNode; }
        }

        /// <summary>
        /// Gets the TreeView casted to a NPCChatDialogView.
        /// </summary>
        NPCChatDialogView TreeViewCasted
        {
            get { return (NPCChatDialogView)TreeView; }
        }

        /// <summary>
        /// Creates a NPCChatDialogViewNode as a child of this NPCChatDialogViewNode.
        /// </summary>
        /// <param name="dialogItem">The NPCChatDialogItemBase the child n will handle.</param>
        /// <returns>A NPCChatDialogViewNode as a child of this NPCChatDialogViewNode.</returns>
        NPCChatDialogViewNode CreateNode(NPCChatDialogItemBase dialogItem)
        {
            // Check if the n already exists
            var children = Nodes.OfType<NPCChatDialogViewNode>();
            NPCChatDialogViewNode retNode =
                children.FirstOrDefault(
                    x =>
                    (x.ChatItemType == NPCChatDialogViewNodeItemType.DialogItem ||
                     x.ChatItemType == NPCChatDialogViewNodeItemType.Redirect) && x.ChatItemAsDialogItem == dialogItem);

            // Create the new n if needed
            if (retNode == null)
            {
                // Check if it has to be a redirect n
                NPCChatDialogViewNode existingDialogNode = TreeViewCasted.GetNodeForDialogItem(dialogItem);
                if (existingDialogNode != null)
                    retNode = new NPCChatDialogViewNode(this, existingDialogNode);
                else
                    retNode = new NPCChatDialogViewNode(this, dialogItem);
            }

            return retNode;
        }

        /// <summary>
        /// Creates a NPCChatDialogViewNode as a child of this NPCChatDialogViewNode.
        /// </summary>
        /// <param name="response">The NPCChatResponseBase the child n will handle.</param>
        /// <returns>A NPCChatDialogViewNode as a child of this NPCChatDialogViewNode.</returns>
        NPCChatDialogViewNode CreateNode(NPCChatResponseBase response)
        {
            // Check if the n already exists
            var children = Nodes.OfType<NPCChatDialogViewNode>();
            NPCChatDialogViewNode retNode =
                children.FirstOrDefault(
                    x => x.ChatItemType == NPCChatDialogViewNodeItemType.Response && x.ChatItemAsResponse == response);

            if (retNode == null)
                retNode = new NPCChatDialogViewNode(this, response);

            return retNode;
        }

        /// <summary>
        /// Gets the text for a NPCChatDialogItemBase.
        /// </summary>
        /// <param name="item">The NPCChatDialogItemBase.</param>
        /// <returns>The text for a NPCChatDialogItemBase.</returns>
        static string GetText(NPCChatDialogItemBase item)
        {
            string text = item.Title;
            if (string.IsNullOrEmpty(text))
            {
                text = item.Text;
                if (text.Length > 100)
                    text = text.Substring(0, 100);
            }

            return text;
        }

        /// <summary>
        /// Gets if this NPCChatDialogViewNode is for the NPCChatDialogItemBase with the specified index.
        /// </summary>
        /// <param name="dialogItemIndex">The index of the NPCChatDialogItemBase.</param>
        /// <param name="includeRedirects">If true, this method will return true if the NPCChatDialogViewNode is
        /// for a redirection to the NPCChatDialogItemBase with the specified <paramref name="dialogItemIndex"/>;
        /// otherwise, only the NPCChatDialogViewNode that handles the NPCChatDialogItemBase with the
        /// specified <paramref name="dialogItemIndex"/> directly will return true.</param>
        /// <returns>True if this NPCChatDialogViewNode handles the NPCChatDialogItemBase with the specified
        /// <paramref name="dialogItemIndex"/>; otherwise false.</returns>
        public bool IsForDialogItem(ushort dialogItemIndex, bool includeRedirects)
        {
            switch (ChatItemType)
            {
                case NPCChatDialogViewNodeItemType.DialogItem:
                    if (ChatItemAsDialogItem.Index == dialogItemIndex)
                        return true;
                    break;

                case NPCChatDialogViewNodeItemType.Redirect:
                    if (includeRedirects && ChatItemAsDialogItem.Index == dialogItemIndex)
                        return true;
                    break;
            }

            return false;
        }

        /// <summary>
        /// Removes the current tree n from the tree view control.
        /// </summary>
        public new void Remove()
        {
            TreeViewCasted.NotifyNodeDestroyed(this);
            base.Remove();
        }

        /// <summary>
        /// Updates this NPCChatDialogViewNode.
        /// </summary>
        /// <param name="recursive">If true, all nodes under this NPCChatDialogViewNode are updated, too.</param>
        public void Update(bool recursive)
        {
            bool checkToSwapRedirectNodes = false;
            var childNodes = Nodes.OfType<NPCChatDialogViewNode>();
            IEnumerable<NPCChatDialogViewNode> nodesToRemove = null;

            switch (ChatItemType)
            {
                case NPCChatDialogViewNodeItemType.DialogItem:
                    // For a dialog item, add the responses
                    EditorNPCChatDialogItem asDialogItem = ChatItemAsDialogItem;
                    var validNodes = new List<NPCChatDialogViewNode>(asDialogItem.Responses.Count());
                    foreach (NPCChatResponseBase response in asDialogItem.Responses)
                    {
                        validNodes.Add(CreateNode(response));
                    }

                    // Mark dead nodes
                    if (validNodes.Count != Nodes.Count)
                        nodesToRemove = childNodes.Except(validNodes);

                    break;

                case NPCChatDialogViewNodeItemType.Redirect:
                    // For a redirect, there are no child nodes

                    // Mark dead nodes
                    if (Nodes.Count > 0)
                        nodesToRemove = childNodes;

                    break;

                case NPCChatDialogViewNodeItemType.Response:
                    // For a response, add the dialog item, using a redirect if needed
                    NPCChatDialogItemBase dialogItem = TreeViewCasted.NPCChatDialog.GetDialogItem(ChatItemAsResponse.Page);
                    NPCChatDialogViewNode validNode = null;
                    if (dialogItem != null)
                    {
                        validNode = CreateNode(dialogItem);
                        if (validNode.ChatItemType == NPCChatDialogViewNodeItemType.Redirect)
                            checkToSwapRedirectNodes = true;
                    }

                    // Mark dead nodes
                    if (Nodes.Count > (dialogItem != null ? 1 : 0))
                        nodesToRemove = childNodes.Where(x => x != validNode);

                    break;

                default:
                    throw new Exception("Invalid ChatItemType.");
            }

            // Remove the marked nodes to be removed
            if (nodesToRemove != null)
            {
                foreach (NPCChatDialogViewNode node in nodesToRemove)
                {
                    node.Remove();
                }
            }

            // Update the text of this n
            UpdateText();

            // Check to recursively update all the children nodes
            if (recursive)
            {
                foreach (NPCChatDialogViewNode child in childNodes)
                {
                    child.Update(true);
                }
            }

            // Check if to swap nodes so the redirect is as deep in the tree as possible
            // This must be done here at the end, otherwise we will disrupt the recursive update process
            // and some nodes will end up not appearing in the tree
            if (checkToSwapRedirectNodes)
            {
                foreach (
                    NPCChatDialogViewNode child in childNodes.Where(x => x.ChatItemType == NPCChatDialogViewNodeItemType.Redirect)
                    )
                {
                    NPCChatDialogViewNode existing = TreeViewCasted.GetNodeForDialogItem(child.ChatItemAsDialogItem);
                    int existingDepth = existing.GetDepth();
                    int childDepth = child.GetDepth();

                    if (existingDepth > childDepth)
                        existing.SwapNode(child, existing.Nodes.Count <= 0);
                }
            }
        }

        /// <summary>
        /// Updates the ChatItemAsDialogItem property.
        /// </summary>
        void UpdateChatItemType()
        {
            if (Tag is NPCChatDialogItemBase)
                _chatItemType = NPCChatDialogViewNodeItemType.DialogItem;
            else if (Tag is NPCChatResponseBase)
                _chatItemType = NPCChatDialogViewNodeItemType.Response;
            else if (Tag is TreeNode)
                _chatItemType = NPCChatDialogViewNodeItemType.Redirect;
            else
                throw new Exception("Invalid ChatItem type.");
        }

        /// <summary>
        /// Updates the text for this NPCChatDialogViewNode.
        /// </summary>
        internal void UpdateText()
        {
            EditorNPCChatDialogItem asDialogItem;
            EditorNPCChatResponse asResponse;

            string text;
            Color foreColor;

            switch (ChatItemType)
            {
                case NPCChatDialogViewNodeItemType.Redirect:
                    asDialogItem = ChatItemAsDialogItem;
                    text = "[GOTO " + asDialogItem.Index + ": " + GetText(asDialogItem) + "]";
                    foreColor = TreeViewCasted.NodeForeColorGoTo;
                    break;

                case NPCChatDialogViewNodeItemType.DialogItem:
                    asDialogItem = ChatItemAsDialogItem;
                    text = asDialogItem.Index + ": " + GetText(asDialogItem);
                    foreColor = asDialogItem.IsBranch ? TreeViewCasted.NodeForeColorBranch : TreeViewCasted.NodeForeColorNormal;
                    break;

                case NPCChatDialogViewNodeItemType.Response:
                    asResponse = ChatItemAsResponse;
                    text = "[" + asResponse.Value + ": " + asResponse.Text + "]";
                    foreColor = TreeViewCasted.NodeForeColorResponse;
                    break;

                default:
                    throw new Exception("Invalid ChatItemType.");
            }

            Text = text;
            ForeColor = foreColor;
        }
    }
}