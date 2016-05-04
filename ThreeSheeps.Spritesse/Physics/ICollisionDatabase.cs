﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ThreeSheeps.Spritesse.Physics
{
    public interface ICollisionDatabase
    {
        void Insert(PhysicalShape shape, Vector2 position, Vector2 halfDimensions);

        void Update(PhysicalShape shape, Vector2 position, Vector2 halfDimensions);

        void Remove(PhysicalShape shape);

        void Query(Vector2 position, Vector2 halfDimensions, IList<PhysicalShape> results);
    }
}
