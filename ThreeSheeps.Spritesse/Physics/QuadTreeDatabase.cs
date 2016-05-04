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
            throw new NotImplementedException();
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
    }
}
