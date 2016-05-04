using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ThreeSheeps.Spritesse.Physics
{
    public class CollisionResolverComponent : GameComponent, ICollisionResolverService
    {
        public CollisionResolverComponent(Game game, ICollisionDatabase database)
            : base(game)
        {
            this.database = database;
        }

        public CollisionResolverComponent(Game game)
            : this(game, new QuadTreeDatabase())
        {
        }

        public void Insert(PhysicalShape shape)
        {
            if (shape.CanSendCollisions)
            {
                this.Database.Insert(shape);
            }
            if (shape.CanReceiveCollisions)
            {
                this.Receivers.Add(shape);
            }
        }

        public void Update(PhysicalShape shape)
        {
            if (shape.CanSendCollisions)
            {
                this.Database.Update(shape);
            }
        }

        public void Remove(PhysicalShape shape)
        {
            if (shape.CanSendCollisions)
            {
                this.Database.Remove(shape);
            }
            if (shape.CanReceiveCollisions)
            {
                this.Receivers.Remove(shape);
            }
        }

        public void Query(PhysicalShape tester, IList<CollisionInformation> results)
        {
            this.Database.Query(tester.Position, tester.HalfDimensions, this.candidates);

            foreach (PhysicalShape sender in this.candidates)
            {
                if (this.CheckCollision(sender, tester))
                {
                    CollisionInformation info = new CollisionInformation();
                    info.Sender = sender;
                    results.Add(info);
                }
            }

            this.candidates.Clear();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (PhysicalShape receiver in this.Receivers)
            {
                this.Query(receiver, receiver.CollisionList);
            }
        }

        protected ICollisionDatabase Database { get { return this.database; } }

        protected ISet<PhysicalShape> Receivers { get { return this.receivers; } }

        protected virtual bool CheckCollision(PhysicalShape shape1, PhysicalShape shape2)
        {
            // TODO
            return false;
        }

        private readonly ICollisionDatabase database;
        private readonly ISet<PhysicalShape> receivers = new HashSet<PhysicalShape>();
        private readonly List<PhysicalShape> candidates = new List<PhysicalShape>();
    }
}
