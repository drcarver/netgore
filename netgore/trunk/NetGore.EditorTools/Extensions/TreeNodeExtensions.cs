using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace NetGore.EditorTools
{
    /// <summary>
    /// Extensions for the TreeNode.
    /// </summary>
    public static class TreeNodeExtensions
    {
        /// <summary>
        /// Gets the 0-based depth of the given TreeNode.
        /// </summary>
        /// <param name="n">The TreeNode to get the depth of.</param>
        /// <returns>The depth of the <paramref name="n"/>, where 0 means this is the root n and it has
        /// no parent nodes.</returns>
        public static int GetDepth(this TreeNode node)
        {
            int depth = 0;
            while (node.Parent != null)
            {
                depth++;
                node = node.Parent;
            }

            return depth;
        }

        /// <summary>
        /// Gets an IEnumerable of all the parent TreeNodes for the given <paramref name="n"/>.
        /// </summary>
        /// <param name="n">The TreeNode to find the parents for.</param>
        /// <returns>An IEnumerable of all the parent TreeNodes for the given <paramref name="n"/>.</returns>
        public static IEnumerable<TreeNode> GetParents(this TreeNode node)
        {
            TreeNode current = node;
            while (current.Parent != null)
            {
                yield return current.Parent;
                current = current.Parent;
            }
        }

        /// <summary>
        /// Gets an IEnumerable of all the TreeNodes that share the same Parent and are on the same depth
        /// as the given <paramref name="n"/>. That is, all TreeNodes that this <paramref name="n"/>
        /// is immediately grouped with.
        /// </summary>
        /// <param name="n">The TreeNode to fid the sister nodes for.</param>
        /// <returns>The sister nodes for the given <paramref name="n"/>, but not the <paramref name="n"/>
        /// itself.</returns>
        public static IEnumerable<TreeNode> GetSisterNodes(this TreeNode node)
        {
            IEnumerable<TreeNode> sisterNodes;

            if (node.Parent != null)
                sisterNodes = node.Parent.Nodes.Cast<TreeNode>();
            else
                sisterNodes = node.TreeView.Nodes.Cast<TreeNode>();

            Debug.Assert(sisterNodes.Contains(node));

            return sisterNodes.Where(x => x != node);
        }

        /// <summary>
        /// Swaps this TreeNode with another TreeNode.
        /// </summary>
        /// <param name="n">This TreeNode.</param>
        /// <param name="other">The TreeNode to swap with.</param>
        /// <param name="swapChildren">If true, the child nodes are also swapped.</param>
        public static void SwapNode(this TreeNode node, TreeNode other, bool swapChildren)
        {
            TreeNode a = node;
            TreeNode b = other;

            TreeNodeCollection aParentNodes = a.Parent != null ? a.Parent.Nodes : a.TreeView.Nodes;
            TreeNodeCollection bParentNodes = b.Parent != null ? b.Parent.Nodes : b.TreeView.Nodes;

            var aChildNodes = a.Nodes.ToArray<TreeNode>();
            var bChildNodes = b.Nodes.ToArray<TreeNode>();

            a.Nodes.Clear();
            b.Nodes.Clear();

            a.Remove();
            b.Remove();

            // Swap child nodes
            if (swapChildren)
            {
                a.Nodes.AddRange(bChildNodes);
                b.Nodes.AddRange(aChildNodes);
            }
            else
            {
                a.Nodes.AddRange(aChildNodes);
                b.Nodes.AddRange(bChildNodes);
            }

            // Swap parents
            aParentNodes.Add(b);
            bParentNodes.Add(a);
        }
    }
}