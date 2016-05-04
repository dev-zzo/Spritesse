using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ThreeSheeps.Spritesse.Physics
{
    public interface ICollisionDatabase
    {
        void Insert(IPhysical shape, Vector2 position, Vector2 halfDimensions);

        void Update(IPhysical shape, Vector2 position, Vector2 halfDimensions);

        void Remove(IPhysical shape);

        void Query(Vector2 position, Vector2 halfDimensions, IList<IPhysical> results);
    }
}
