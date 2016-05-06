using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ThreeSheeps.Spritesse.Physics
{
    /// <summary>
    /// This is a base class for all physical shapes.
    /// </summary>
    public abstract class PhysicalShape
    {
        /// <summary>
        /// This represents creation information for a shape.
        /// As there maybe a lot of parameters, it is represented as a class
        /// instead of trying to pass everything via constructor arguments.
        /// </summary>
        public class CreationInfo
        {
            public bool SendCollisions;
            public bool ReceiveCollisions;
            public Vector2 Position;
        }

        protected PhysicalShape(CreationInfo info)
        {
            this.canSendCollisions = info.SendCollisions;
            this.canReceiveCollisions = info.ReceiveCollisions;
            this.position = info.Position;

            if (this.canReceiveCollisions)
            {
                this.collisions = new List<CollisionInformation>();
            }
        }

        /// <summary>
        /// Whether this shape can be collided with.
        /// </summary>
        public bool CanSendCollisions
        {
            get { return this.canSendCollisions; }
        }

        /// <summary>
        /// Whether this shape can collide with another one.
        /// </summary>
        public bool CanReceiveCollisions
        {
            get { return this.canReceiveCollisions; }
        }

        /// <summary>
        /// Shape position.
        /// </summary>
        public Vector2 Position
        {
            get { return this.position; }
            set { this.UpdatePosition(value); }
        }

        /// <summary>
        /// The half-dimensions of the shape's axis-aligned bounding box.
        /// </summary>
        public abstract Vector2 HalfDimensions { get; }

        /// <summary>
        /// For shapes that can receive collisions, a list containing all occurred collisions.
        /// for other shapes this is null.
        /// </summary>
        public IList<CollisionInformation> CollisionList
        {
            get { return this.collisions; }
        }

        /// <summary>
        /// This is called back by the collision resolver when this shape is inserted.
        /// </summary>
        /// <param name="database">Collision database reference</param>
        internal void OnInserted(ICollisionDatabase database)
        {
            this.database = database;
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
            if (!this.CanSendCollisions)
                return;
            this.database.Update(this);
        }

        private ICollisionDatabase database;
        private bool canSendCollisions;
        private bool canReceiveCollisions;
        private Vector2 position;
        private IList<CollisionInformation> collisions;

        #endregion
    }
}
