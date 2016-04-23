using Microsoft.Xna.Framework;

namespace ThreeSheeps.Tiled
{
    /// <summary>
    /// Base class for layered objects.
    /// </summary>
    public class TxxLayerContent
    {
        /// <summary>
        /// Object group name.
        /// </summary>
        public string Name;
        /// <summary>
        /// Rendering offset for this object group, in pixels.
        /// </summary>
        public Point RenderingOffset;
        /// <summary>
        // Whether the objects in this group are visible.
        /// </summary>
        public bool Visible;
        /// <summary>
        /// How transparent is this object group.
        /// </summary>
        public float Opacity;
        /// <summary>
        /// Contains any custom properties set for this node.
        /// </summary>
        public TxxProperties Properties;
    }
}
