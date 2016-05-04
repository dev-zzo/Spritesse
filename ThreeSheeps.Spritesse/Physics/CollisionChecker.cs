﻿using Microsoft.Xna.Framework;
using System;

namespace ThreeSheeps.Spritesse.Physics
{
    public delegate bool CheckCollisionDelegate(PhysicalShape shape1, PhysicalShape shape2);

    internal static class CollisionChecker
    {
        /// <summary>
        /// Test for collisions.
        /// </summary>
        /// <param name="_shape1">An instance of PhysicalCircle</param>
        /// <param name="_shape2">An instance of PhysicalCircle</param>
        /// <returns>True if the two collide, false otherwise.</returns>
        public static bool CheckCollisionCC(PhysicalShape _shape1, PhysicalShape _shape2)
        {
            PhysicalCircle shape1 = _shape1 as PhysicalCircle;
            PhysicalCircle shape2 = _shape2 as PhysicalCircle;
            float distanceSquared = (shape1.Position - shape2.Position).LengthSquared();
            float contactDistance = shape1.Radius + shape2.Radius;
            return distanceSquared <= contactDistance * contactDistance;
        }

        /// <summary>
        /// Test for collisions.
        /// </summary>
        /// <param name="_shape1">An instance of PhysicalAxisAlignedBox</param>
        /// <param name="_shape2">An instance of PhysicalAxisAlignedBox</param>
        /// <returns>True if the two collide, false otherwise.</returns>
        public static bool CheckCollisionBB(PhysicalShape _shape1, PhysicalShape _shape2)
        {
            PhysicalAxisAlignedBox shape1 = _shape1 as PhysicalAxisAlignedBox;
            PhysicalAxisAlignedBox shape2 = _shape2 as PhysicalAxisAlignedBox;
            Vector2 shape1Min = shape1.Position - shape1.HalfDimensions;
            Vector2 shape1Max = shape1.Position + shape1.HalfDimensions;
            Vector2 shape2Min = shape2.Position - shape2.HalfDimensions;
            Vector2 shape2Max = shape2.Position + shape2.HalfDimensions;
            return !((shape1Max.Y < shape2Min.Y) 
                || (shape1Min.Y > shape2Max.Y) 
                || (shape1Min.X > shape2Max.X) 
                || (shape1Max.X < shape2Min.X));
        }

        /// <summary>
        /// Test for collisions.
        /// </summary>
        /// <param name="_shape1">An instance of PhysicalAxisAlignedBox</param>
        /// <param name="_shape2">An instance of PhysicalCircle</param>
        /// <returns>True if the two collide, false otherwise.</returns>
        public static bool CheckCollisionBC(PhysicalShape _shape1, PhysicalShape _shape2)
        {
            PhysicalAxisAlignedBox shape1 = _shape1 as PhysicalAxisAlignedBox;
            PhysicalCircle shape2 = _shape2 as PhysicalCircle;
            Vector2 boxHalfDimensions = shape1.HalfDimensions;
            // See http://stackoverflow.com/a/402010/1654774
            // A vector in Q1 denoting the circle position
            float distanceX = Math.Abs(shape2.Position.X - shape1.Position.X);
            float distanceY = Math.Abs(shape2.Position.Y - shape1.Position.Y);
            // Check if the circle is completely outside.
            if (distanceX > boxHalfDimensions.X + shape2.Radius) return false;
            if (distanceY > boxHalfDimensions.Y + shape2.Radius) return false;
            // Check if the circle's center is inside.
            if (distanceX <= boxHalfDimensions.X) return true;
            if (distanceY <= boxHalfDimensions.Y) return true;
            // The hard case.
            distanceX -= boxHalfDimensions.X;
            distanceY -= boxHalfDimensions.Y;
            return distanceX * distanceX + distanceY * distanceY <= shape2.Radius * shape2.Radius;
        }
    }
}
