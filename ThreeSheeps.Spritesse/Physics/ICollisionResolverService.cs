using System.Collections.Generic;

namespace ThreeSheeps.Spritesse.Physics
{
    /// <summary>
    /// This is the physical service for the game.
    /// </summary>
    public interface ICollisionResolverService
    {
        /// <summary>
        /// Create a shape; determine which shape to create from the type.
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        IPhysicalShape Create(PhysicalShapeInformation info);

        /// <summary>
        /// Create a circle shape, specifically.
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        IPhysicalShape Create(PhysicalCircleInformation info);

        /// <summary>
        /// Create a box shape, specifically.
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        IPhysicalShape Create(PhysicalBoxInformation info);

        /// <summary>
        /// Delete the shape.
        /// </summary>
        /// <param name="shape"></param>
        void Recycle(IPhysicalShape shape);

        /// <summary>
        /// Clear all shapes.
        /// </summary>
        void Clear();

        /// <summary>
        /// Query for collisions between the given shape and the currently inserted ones.
        /// </summary>
        /// <param name="probe">Probe shape</param>
        /// <param name="results">Provides a list to be filled with shapes colliding with the probe</param>
        void Query(IPhysicalShape probe, IList<CollisionInformation> results);
    }
}
