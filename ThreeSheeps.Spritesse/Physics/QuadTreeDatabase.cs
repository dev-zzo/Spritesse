using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ThreeSheeps.Spritesse.Physics
{
    internal sealed class QuadTreeDatabase : ICollisionDatabase
    {
        #region ICollisionDatabase implementation

        public void Insert(PhysicalShape shape)
        {
            // Ensure the object can fit into the current root.
            this.ExpandRoot(shape.Position, shape.HalfDimensions);
            // Insert it somewhere.
            this.Insert(shape, this.root);
        }

        public void Update(PhysicalShape shape)
        {
            // Find the node.
            TreeNode node = FindNode(shape, this.root);
            if (node == null)
            {
                // Should probably throw an exception.
                return;
            }
            // Check whether the object is still within the current node
            if (ContainedIn(shape.Position, shape.HalfDimensions, node.Center, node.HalfSize))
            {
                return;
            }

            // If not, remove and insert it.
            Remove(shape, node);
            this.Insert(shape);
        }

        public void Remove(PhysicalShape shape)
        {
            // Find the node.
            TreeNode node = FindNode(shape, this.root);
            if (node == null)
            {
                // Should probably throw an exception.
                return;
            }
            Remove(shape, node);
        }

        public void Query(Vector2 position, Vector2 halfDimensions, IList<PhysicalShape> results)
        {
            throw new NotImplementedException();
        }

        #endregion

        // This one has an implementation issue.
        // * It would be much more awesome if this array would be a value type...
        // * Avoid these being garbage collected by keeping a free list.
        private class TreeNode : List<PhysicalShape>
        {
            public TreeNode(Vector2 center, float halfSize)
            {
                this.Center = center;
                this.HalfSize = halfSize;
            }

            // Keep a reference to the parent node?
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
                TreeNode newRoot = this.AllocateNode(newRootCenter, root.HalfSize * 2.0f);
                newRoot.Children = this.AllocateNodeChildren(newRoot.Center, newRoot.HalfSize);
                // Dump the unneeded one
                TreeNode unused = newRoot.Children[index];
                newRoot.Children[index] = root;
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
        private void Insert(PhysicalShape shape, TreeNode node)
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
                        node.Add(shape);
                        break;
                    }
                    node.Children = this.AllocateNodeChildren(node.Center, node.HalfSize);
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
                    node.Add(shape);
                    break;
                }
                // Select the new node and continue
                node = child;
            }
        }

        private static TreeNode FindNode(PhysicalShape shape, TreeNode node)
        {
            for (; ; )
            {
                if (node.Contains(shape))
                    return node;
                if (node.Children == null)
                    return null;
                int index = ChildIndexFromPositionDifference(node.Center, shape.Position);
                node = node.Children[index];
            }
        }

        private static void Remove(PhysicalShape shape, TreeNode node)
        {
            node.Remove(shape);
            // TODO: Check for empty nodes that could be freed.
        }

        #region Memory management

        private TreeNode[] AllocateNodeChildren(Vector2 nodeCenter, float nodeHalfSize)
        {
            TreeNode[] children = new TreeNode[4];
            float childSize = nodeHalfSize * 0.5f;
            for (int index = 0; index < 4; ++index)
            {
                children[index] = this.AllocateNode(ChildCenterFromIndex(index, nodeCenter, nodeHalfSize), childSize);
            }
            return children;
        }

        private TreeNode AllocateNode(Vector2 center, float halfSize)
        {
            // TODO: check the free list
            return new TreeNode(center, halfSize);
        }

        private void FreeNode(TreeNode node)
        {
            // TODO: release to the free list
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
