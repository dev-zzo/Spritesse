using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ThreeSheeps.Spritesse.Physics
{
    public interface IPhysicalShape
    {
        /// <summary>
        /// Whether this shape can be collided with.
        /// </summary>
        bool CanSendCollisions { get; }

        /// <summary>
        /// Whether this shape can collide with another one.
        /// </summary>
        bool CanReceiveCollisions { get; }

        /// <summary>
        /// Shape position.
        /// </summary>
        Vector2 Position { get; set; }

        /// <summary>
        /// The dimensions of the shape's axis-aligned bounding box.
        /// </summary>
        Vector2 Dimensions { get; }

        /// <summary>
        /// For shapes that can receive collisions, a list containing all occurred collisions.
        /// for other shapes this is null.
        /// </summary>
        IList<CollisionInformation> CollisionList { get; }
    }
}
