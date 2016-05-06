using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ThreeSheeps.Spritesse.Physics
{
    /// <summary>
    /// This implements the quad-tree spatial database.
    /// </summary>
    public sealed class QuadTreeDatabase : ICollisionDatabase
    {
        public QuadTreeDatabase(Vector2 initialRootPosition, float initialRootSize)
        {
            this.root = this.AllocateNode(null, initialRootPosition, initialRootSize * 0.5f);
        }

        #region ICollisionDatabase implementation

        public object Insert(PhysicalShape shape)
        {
            // Ensure the object can fit into the current root.
            this.ExpandRoot(shape.Position, shape.HalfDimensions);
            // Insert it somewhere.
            return this.Insert(shape, this.root);
        }

        public object Update(PhysicalShape shape, object node)
        {
            TreeNode actualNode = node as TreeNode;
            // Check whether the object is still within the current node
            if (ContainedIn(shape.Position, shape.HalfDimensions, actualNode.Center, actualNode.HalfSize))
            {
                return actualNode;
            }

            // If not, remove and insert it.
            actualNode.Remove(shape);
            object newNode = this.Insert(shape);
            this.Cleanup(actualNode);
            return newNode;
        }

        public void Remove(PhysicalShape shape, object node)
        {
            TreeNode actualNode = node as TreeNode;
            actualNode.Remove(shape);
            this.Cleanup(actualNode);
        }

        public void Query(Vector2 position, Vector2 halfDimensions, IList<PhysicalShape> results)
        {
            Query(position, halfDimensions, results, this.root);
        }

        #endregion

        // This one has an implementation issue.
        // * It would be much more awesome if this array would be a value type...
        // * Avoid these being garbage collected by keeping a free list.
        private class TreeNode : List<PhysicalShape>
        {
            public TreeNode(TreeNode parent, Vector2 center, float halfSize)
            {
                this.Parent = parent;
                this.Center = center;
                this.HalfSize = halfSize;
            }

            public bool IsEmptyLeaf { get { return this.Count == 0 && this.Children == null; } }

            public TreeNode Parent;
            public Vector2 Center;
            public float HalfSize;
            public TreeNode[] Children;
        }

        /// <summary>
        /// Ensure the given shape fits into the current tree root;
        /// expand the root to guarantee that.
        /// </summary>
        /// <param name="shapePosition">Position of the shape's center</param>
        /// <param name="shapeHalfDimensions">Shape's half dimensions</param>
        private void ExpandRoot(Vector2 shapePosition, Vector2 shapeHalfDimensions)
        {
            TreeNode root = this.root;

            while (!ContainedIn(shapePosition, shapeHalfDimensions, root.Center, root.HalfSize))
            {
                // Assume the shape is at the new root's center;
                // Calculate the offset and subtract it.
                int index = ChildIndexFromPositionDifference(shapePosition, root.Center);
                Vector2 newRootCenter = root.Center - ChildOffsetFromIndex(index) * root.HalfSize;
                // Build children nodes for the new root
                TreeNode newRoot = this.AllocateNode(null, newRootCenter, root.HalfSize * 2.0f);
                newRoot.Children = this.AllocateNodeChildren(newRoot, newRoot.Center, newRoot.HalfSize);
                // Dump the unneeded one
                TreeNode unused = newRoot.Children[index];
                newRoot.Children[index] = root;
                root.Parent = newRoot;
                this.FreeNode(unused);

                root = newRoot;
            }

            this.root = root;
        }

        /// <summary>
        /// Insert the shape into the given node or a child of it.
        /// </summary>
        /// <param name="shape"></param>
        /// <param name="node"></param>
        private TreeNode Insert(PhysicalShape shape, TreeNode node)
        {
            // This could be implemented recursively as well.
            // Currently, there are no obvious gains in favor of recursion, though.
            for (; ; )
            {
                // Invariant: the object fits into the current node.

                if (node.Children == null)
                {
                    // This is a leaf node.
                    // Try accumulating shapes first.
                    if (node.Count < MAX_SHAPES_PER_NODE)
                    {
                        break;
                    }
                    node.Children = this.AllocateNodeChildren(node, node.Center, node.HalfSize);
                    // Redistribute shapes from this node.
                    // Either the shape will be inserted into the current node, or into one child.
                    int shapeCount = node.Count;
                    while (shapeCount > 0)
                    {
                        PhysicalShape oldShape = node[0];
                        node.RemoveAt(0);
                        Insert(oldShape, node);
                        shapeCount--;
                    }
                }

                // This is an inner node.
                // Check the children; if not possible, add here.
                int index = ChildIndexFromPositionDifference(node.Center, shape.Position);
                TreeNode child = node.Children[index];
                if (!ContainedIn(shape.Position, shape.HalfDimensions, child.Center, child.HalfSize))
                {
                    break;
                }
                // Select the new node and continue
                node = child;
            }

            node.Add(shape);
            return node;
        }

        private static void Query(Vector2 position, Vector2 halfDimensions, IList<PhysicalShape> results, TreeNode node)
        {
            foreach (PhysicalShape shape in node)
                results.Add(shape);
            if (node.Children != null)
            {
                for (int index = 0; index < 4; ++index)
                {
                    TreeNode child = node.Children[index];
                    // Check whether the query area overlaps with the child's square
                    Vector2 difference = child.Center - position;
                    difference = new Vector2(Math.Abs(difference.X), Math.Abs(difference.Y));
                    if (difference.X - node.HalfSize <= halfDimensions.X && difference.Y - node.HalfSize <= halfDimensions.Y)
                    {
                        Query(position, halfDimensions, results, child);
                    }
                }
            }
        }

        #region Memory management

        private TreeNode[] AllocateNodeChildren(TreeNode parent, Vector2 nodeCenter, float nodeHalfSize)
        {
            TreeNode[] children = new TreeNode[4];
            float childSize = nodeHalfSize * 0.5f;
            for (int index = 0; index < 4; ++index)
            {
                children[index] = this.AllocateNode(
                    parent,
                    ChildCenterFromIndex(index, nodeCenter, nodeHalfSize),
                    childSize);
            }
            return children;
        }

        private void FreeNodeChildren(TreeNode[] children)
        {
        }

        private TreeNode AllocateNode(TreeNode parent, Vector2 center, float halfSize)
        {
            // TODO: check the free list
            return new TreeNode(parent, center, halfSize);
        }

        private void FreeNode(TreeNode node)
        {
            // TODO: release to the free list
        }

        private void Cleanup(TreeNode node)
        {
            bool cleanChildren = true;
            if (node.Children != null)
            {
                for (int index = 0; index < 4; ++index)
                {
                    TreeNode child = node.Children[index];
                    cleanChildren &= child.IsEmptyLeaf;
                }
                if (cleanChildren)
                {
                    this.FreeNodeChildren(node.Children);
                    node.Children = null;
                }
            }
            if (node.IsEmptyLeaf && node.Parent != null)
            {
                this.Cleanup(node.Parent);
            }
        }

        #endregion

        /// <summary>
        /// Check whether an object is contained within the container square.
        /// </summary>
        /// <param name="objectCenter"></param>
        /// <param name="objectHalfDimensions"></param>
        /// <param name="containerCenter"></param>
        /// <param name="containerHalfSize"></param>
        /// <returns></returns>
        private static bool ContainedIn(Vector2 objectCenter, Vector2 objectHalfDimensions, Vector2 containerCenter, float containerHalfSize)
        {
            Vector2 offset = objectCenter - containerCenter;
            Vector2 objectExtent = new Vector2(Math.Abs(offset.X), Math.Abs(offset.Y)) + objectHalfDimensions;
            return objectExtent.X <= containerHalfSize && objectExtent.Y <= containerHalfSize;
        }

        private static int ChildIndexFromPositionDifference(Vector2 nodeCenter, Vector2 objectCenter)
        {
            return (nodeCenter.X >= objectCenter.X ? 1 : 0) | (nodeCenter.Y >= objectCenter.Y ? 2 : 0);
        }

        private static Vector2 ChildOffsetFromIndex(int index)
        {
            return offsets[index];
        }

        private static Vector2 ChildCenterFromIndex(int index, Vector2 parentCenter, float parentHalfSize)
        {
            return parentCenter + offsets[index] * parentHalfSize;
        }

        private const int MAX_SHAPES_PER_NODE = 4;

        private static Vector2[] offsets =
            {
                new Vector2(-0.5f, -0.5f),
                new Vector2(+0.5f, -0.5f),
                new Vector2(-0.5f, +0.5f),
                new Vector2(+0.5f, +0.5f),
            };

        private TreeNode root;
    }
}
