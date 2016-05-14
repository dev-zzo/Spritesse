using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ThreeSheeps.Spritesse.Physics
{
    /// <summary>
    /// This is a base class for all physical shapes.
    /// </summary>
    internal abstract class PhysicalShape : IPhysicalShape
    {
        protected PhysicalShape(ICollisionDatabase database, PhysicalShapeInformation info)
        {
            this.database = database;
            this.canSendCollisions = info.SendCollisions;
            this.canReceiveCollisions = info.ReceiveCollisions;
            this.position = info.Position;

            if (this.canReceiveCollisions)
            {
                this.collisions = new List<CollisionInformation>();
            }
        }

        #region IPhysicalShape implementation

        public bool CanSendCollisions
        {
            get { return this.canSendCollisions; }
        }

        public bool CanReceiveCollisions
        {
            get { return this.canReceiveCollisions; }
        }

        public Vector2 Position
        {
            get { return this.position; }
            set { this.UpdatePosition(value); }
        }

        public Vector2 Dimensions { get { return this.HalfDimensions * 2.0f; } }

        public abstract Vector2 HalfDimensions { get; }

        public IList<CollisionInformation> CollisionList
        {
            get { return this.collisions; }
        }

        #endregion

        /// <summary>
        /// Used internally to manage opaque database information
        /// </summary>
        internal object DatabaseInfo { get; set; }

        /// <summary>
        /// Called whenever anything important for the spatial DB changes.
        /// </summary>
        protected void UpdateDatabase()
        {
            if (!this.CanSendCollisions)
                return;
            this.database.Update(this);
        }

        #region Implementation details

        /// <summary>
        /// Handles all the dirty work for this.Position setter.
        /// </summary>
        /// <param name="newValue">New position value.</param>
        private void UpdatePosition(Vector2 newValue)
        {
            if (this.position == newValue)
                return;
            this.position = newValue;
            this.UpdateDatabase();
        }

        private ICollisionDatabase database;
        private bool canSendCollisions;
        private bool canReceiveCollisions;
        private Vector2 position;
        private IList<CollisionInformation> collisions;

        #endregion
    }
}
