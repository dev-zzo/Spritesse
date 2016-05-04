using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ThreeSheeps.Spritesse.Physics
{
    public abstract class PhysicalShape : IPhysical
    {
        public class CreationInfo
        {
            public bool SendCollisions;
            public bool ReceiveCollisions;
            public Vector2 Position;
        }

        protected PhysicalShape(ICollisionResolverService resolver, CreationInfo info)
        {
            this.resolver = resolver;
            this.canSendCollisions = info.SendCollisions;
            this.canReceiveCollisions = info.ReceiveCollisions;
            this.position = info.Position;
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

        private ICollisionResolverService resolver;
        private bool canSendCollisions;
        private bool canReceiveCollisions;
        private Vector2 position;
        private IList<CollisionInformation> collisions;
    }
}
