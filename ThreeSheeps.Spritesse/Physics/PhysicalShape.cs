using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ThreeSheeps.Spritesse.Physics
{
    public abstract class PhysicalShape
    {
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

        public abstract Vector2 HalfDimensions { get; }

        public IList<CollisionInformation> CollisionList
        {
            get { return this.collisions; }
        }

        private void UpdatePosition(Vector2 newValue)
        {
            if (this.position == newValue)
                return;
            this.position = newValue;
            if (!this.CanSendCollisions)
                return;
            // TODO
        }

        protected ICollisionResolverService resolver;
        private bool canSendCollisions;
        private bool canReceiveCollisions;
        private Vector2 position;
        private IList<CollisionInformation> collisions;
    }
}
