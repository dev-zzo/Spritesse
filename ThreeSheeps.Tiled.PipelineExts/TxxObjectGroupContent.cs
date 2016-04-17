using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ThreeSheeps.Tiled
{
    /// <summary>
    /// The object entity, represented by the <object> tag.
    /// </summary>
    public class TxxObjectContent
    {
        /// <summary>
        /// Object ID; unique.
        /// </summary>
        public int Id;
        /// <summary>
        /// Object name.
        /// </summary>
        public string Name;
        /// <summary>
        /// Object type; an arbitrary string.
        /// </summary>
        public string Type;
        /// <summary>
        /// The position of the object's (0,0) point.
        /// </summary>
        public Point Position;
        /// <summary>
        /// Rotation to be applied.
        /// </summary>
        public float Rotation;
        /// <summary>
        /// Tile global Id to use to draw this object.
        /// </summary>
        public int Gid;
        /// <summary>
        /// Whether the object is visible.
        /// </summary>
        public bool Visible;
        /// <summary>
        /// Contains any custom properties set for this node.
        /// </summary>
        public TxxProperties Properties;
    }

    public class TxxBoundedObjectContent : TxxObjectContent
    {
        /// <summary>
        /// Object dimensions.
        /// </summary>
        public Point Dimensions;
    }

    /// <summary>
    /// Rectangular object.
    /// </summary>
    public class TxxRectangleObjectContent : TxxBoundedObjectContent
    {
    }

    /// <summary>
    /// Elliptical object.
    /// </summary>
    public class TxxEllipseObjectContent : TxxBoundedObjectContent
    {
    }

    public class TxxPolyObjectContent : TxxObjectContent
    {
        /// <summary>
        /// Array of object vertices, relative to object position.
        /// </summary>
        public Point[] Points;
    }

    /// <summary>
    /// Polygonal object.
    /// </summary>
    public class TxxPolygonObjectContent : TxxPolyObjectContent
    {
    }

    /// <summary>
    /// Polyline object.
    /// </summary>
    public class TxxPolylineObjectContent : TxxPolyObjectContent
    {
    }

    /// <summary>
    /// Contains TmxObjectContent instances.
    /// </summary>
    public sealed class TxxObjectGroupContent
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
        /// The color used to display the objects in this group.
        /// </summary>
        public Color Color;
        /// <summary>
        /// How transparent is this object group.
        /// </summary>
        public float Opacity;
        /// <summary>
        /// Objects collection.
        /// </summary>
        public readonly List<TxxObjectContent> Objects = new List<TxxObjectContent>();
        /// <summary>
        /// Contains any custom properties set for this node.
        /// </summary>
        public TxxProperties Properties;
    }
}
