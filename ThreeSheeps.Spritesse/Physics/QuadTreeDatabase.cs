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
            this.ExpandRoot(shape.Position, shape.HalfDimensions);
            this.Insert(shape, this.root, this.rootCenter, this.rootHalfSize);
        }

        public void Update(PhysicalShape shape)
        {
            throw new NotImplementedException();
        }

        public void Remove(PhysicalShape shape)
        {
            throw new NotImplementedException();
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
            // Keep a reference to the parent node?
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
            Vector2 rootCenter = this.rootCenter;
            float rootHalfSize = this.rootHalfSize;

            while (!ContainedIn(shapePosition, shapeHalfDimensions, rootCenter, rootHalfSize))
            {
                TreeNode[] children = CreateNewChildren();
                int index = ChildIndexFromPositionDifference(rootCenter, shapePosition);
                TreeNode newRoot = children[index];
                children[index] = root;
                newRoot.Children = children;
                root = newRoot;
                rootCenter -= ChildOffsetFromIndex(index) * rootHalfSize;
                rootHalfSize *= 2.0f;
            }

            if (rootHalfSize != this.rootHalfSize)
            {
                this.root = root;
                this.rootCenter = rootCenter;
                this.rootHalfSize = rootHalfSize;
            }
        }

        /// <summary>
        /// Insert the shape into the given node or a child of it.
        /// </summary>
        /// <param name="shape"></param>
        /// <param name="node"></param>
        /// <param name="nodeCenter"></param>
        /// <param name="nodeHalfSize"></param>
        private void Insert(PhysicalShape shape, TreeNode node, Vector2 nodeCenter, float nodeHalfSize)
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
                    node.Children = CreateNewChildren();
                    // Redistribute shapes from this node
                    int shapeCount = node.Count;
                    while (shapeCount > 0)
                    {
                        PhysicalShape oldShape = node[0];
                        node.RemoveAt(0);
                        Insert(oldShape, node, nodeCenter, nodeHalfSize);
                        shapeCount--;
                    }
                }

                // This is an inner node.
                // Check the children; if not possible, add here.
                int index = ChildIndexFromPositionDifference(nodeCenter, shape.Position);
                nodeHalfSize *= 0.5f;
                nodeCenter += ChildOffsetFromIndex(index) * nodeHalfSize;
                if (!ContainedIn(shape.Position, shape.HalfDimensions, nodeCenter, nodeHalfSize))
                {
                    node.Add(shape);
                    break;
                }
                // Select the new node and continue
                node = node.Children[index];
            }
        }

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

        private static TreeNode[] CreateNewChildren()
        {
            TreeNode[] children = new TreeNode[4];
            for (int i = 0; i < 4; ++i)
                children[i] = new TreeNode();
            return children;
        }

        private static int ChildIndexFromPositionDifference(Vector2 nodeCenter, Vector2 objectCenter)
        {
            return (nodeCenter.X >= objectCenter.X ? 1 : 0) | (nodeCenter.Y >= objectCenter.Y ? 2 : 0);
        }

        private static Vector2 ChildOffsetFromIndex(int index)
        {
            return offsets[index];
        }

        private const int MAX_SHAPES_PER_NODE = 4;

        private static Vector2[] offsets =
            {
                new Vector2(-1.0f, -1.0f),
                new Vector2(-1.0f, +1.0f),
                new Vector2(+1.0f, -1.0f),
                new Vector2(+1.0f, +1.0f),
            };

        private TreeNode root;
        private Vector2 rootCenter;
        private float rootHalfSize;
    }
}
