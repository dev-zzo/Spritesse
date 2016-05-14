using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ThreeSheeps.Spritesse.Physics
{
    /// <summary>
    /// A spatial database for shapes.
    /// </summary>
    internal interface ICollisionDatabase
    {
        /// <summary>
        /// Insert the shape into the database.
        /// </summary>
        /// <param name="shape">Shape reference</param>
        /// <returns>Internal node reference</returns>
        void Insert(PhysicalShape shape);

        /// <summary>
        /// Update the shape's position within the database.
        /// </summary>
        /// <param name="shape">Shape reference</param>
        /// <param name="node">Previous node reference</param>
        /// <returns>New internal node reference</returns>
        void Update(PhysicalShape shape);

        /// <summary>
        /// Remove the shape from the database.
        /// </summary>
        /// <param name="shape">Shape reference</param>
        /// <param name="node">Node reference</param>
        void Remove(PhysicalShape shape);

        /// <summary>
        /// Query the database for shapes that may be within the probe area (AABB).
        /// </summary>
        /// <param name="position">Probe AABB center's position</param>
        /// <param name="halfDimensions">Probe AABB's half-dimensions</param>
        /// <param name="results">List reference to be filled with candidate shapes</param>
        void Query(Vector2 position, Vector2 halfDimensions, IList<PhysicalShape> results);
    }
}
