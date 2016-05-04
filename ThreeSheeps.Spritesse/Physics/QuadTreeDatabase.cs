using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ThreeSheeps.Spritesse.Physics
{
    internal sealed class QuadTreeDatabase : ICollisionDatabase
    {

        #region ICollisionDatabase implementation

        public void Insert(IPhysical shape, Vector2 position, Vector2 halfDimensions)
        {
            throw new NotImplementedException();
        }

        public void Update(IPhysical shape, Vector2 position, Vector2 halfDimensions)
        {
            throw new NotImplementedException();
        }

        public void Remove(IPhysical shape)
        {
            throw new NotImplementedException();
        }

        public void Query(Vector2 position, Vector2 halfDimensions, IList<IPhysical> results)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
