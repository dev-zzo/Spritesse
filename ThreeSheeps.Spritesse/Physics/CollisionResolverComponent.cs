using System;
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
                shape.OnInserted(this.Database);
            }
            if (shape.CanReceiveCollisions)
            {
                this.Receivers.Add(shape);
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

        public void Query(PhysicalShape probe, IList<CollisionInformation> results)
        {
            this.Database.Query(probe.Position, probe.HalfDimensions, this.candidates);

            foreach (PhysicalShape sender in this.candidates)
            {
                // Work around self-collision
                if (sender == probe)
                    continue;

                if (this.CheckCollision(sender, probe))
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
            Type shape1Type = shape1.GetType();
            Type shape2Type = shape2.GetType();

            if (shape1Type == typeof(PhysicalCircle))
            {
                if (shape2Type == typeof(PhysicalCircle))
                {
                    return CollisionCheckHelper.CheckCollision(shape1 as PhysicalCircle, shape2 as PhysicalCircle);
                }
                if (shape2Type == typeof(PhysicalAxisAlignedBox))
                {
                    return CollisionCheckHelper.CheckCollision(shape2 as PhysicalAxisAlignedBox, shape1 as PhysicalCircle);
                }
            }
            else if (shape1Type == typeof(PhysicalAxisAlignedBox))
            {
                if (shape2Type == typeof(PhysicalCircle))
                {
                    return CollisionCheckHelper.CheckCollision(shape1 as PhysicalAxisAlignedBox, shape2 as PhysicalCircle);
                }
                if (shape2Type == typeof(PhysicalAxisAlignedBox))
                {
                    return CollisionCheckHelper.CheckCollision(shape1 as PhysicalAxisAlignedBox, shape2 as PhysicalAxisAlignedBox);
                }
            }

            throw new ArgumentException("no applicable collision checkers found");
        }

        private readonly ICollisionDatabase database;
        private readonly ISet<PhysicalShape> receivers = new HashSet<PhysicalShape>();
        private readonly List<PhysicalShape> candidates = new List<PhysicalShape>();
    }
}
