using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ThreeSheeps.Spritesse.Physics
{
    public interface IPhysical
    {
        bool CanSendCollisions { get; }

        bool CanReceiveCollisions { get; }

        Vector2 Position { get; set; }

        Vector2 HalfDimensions { get; }

        IList<CollisionInformation> CollisionList { get; }
    }
}
