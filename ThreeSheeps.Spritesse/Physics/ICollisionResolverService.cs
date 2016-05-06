using System.Collections.Generic;

namespace ThreeSheeps.Spritesse.Physics
{
    /// <summary>
    /// This is the physical service for the game.
    /// </summary>
    public interface ICollisionResolverService
    {
        /// <summary>
        /// Add a shape to the simulation.
        /// </summary>
        /// <param name="shape"></param>
        void Insert(PhysicalShape shape);

        /// <summary>
        /// Remove the shape from the simulation.
        /// </summary>
        /// <param name="shape"></param>
        void Remove(PhysicalShape shape);

        /// <summary>
        /// Query for collisions between the given shape and the currently inserted ones.
        /// </summary>
        /// <param name="probe">Probe shape</param>
        /// <param name="results">Provides a list to be filled with shapes colliding with the probe</param>
        void Query(PhysicalShape probe, IList<CollisionInformation> results);
    }
}
