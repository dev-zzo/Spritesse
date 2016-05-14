using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ThreeSheeps.Spritesse.Physics
{
    public sealed class CollisionResolverComponent : GameComponent, ICollisionResolverService
    {
        internal CollisionResolverComponent(Game game, ICollisionDatabase database)
            : base(game)
        {
            this.database = database;
            game.Services.AddService(typeof(ICollisionResolverService), this);
        }

        public CollisionResolverComponent(Game game)
            : this(game, new QuadTreeDatabase(new Vector2(128.0f, 128.0f), 256.0f))
        {
        }

        public IPhysicalShape Create(PhysicalShapeInformation info)
        {
            PhysicalCircleInformation circleInfo = info as PhysicalCircleInformation;
            PhysicalBoxInformation boxInfo = info as PhysicalBoxInformation;
            if (circleInfo != null)
            {
                return this.Create(circleInfo);
            }
            if (boxInfo != null)
            {
                return this.Create(boxInfo);
            }
            throw new ArgumentException("info");
        }

        public IPhysicalShape Create(PhysicalCircleInformation info)
        {
            PhysicalCircle shape = new PhysicalCircle(this.database, info);
            this.Insert(shape);
            return shape;
        }

        public IPhysicalShape Create(PhysicalBoxInformation info)
        {
            PhysicalAxisAlignedBox shape = new PhysicalAxisAlignedBox(this.database, info);
            this.Insert(shape);
            return shape;
        }

        public void Recycle(IPhysicalShape shape)
        {
            this.Remove((PhysicalShape)shape);
        }

        public void Clear()
        {
            // TODO
        }

        public void Query(IPhysicalShape probe, IList<CollisionInformation> results)
        {
            PhysicalShape shape = (PhysicalShape)probe;
            this.database.Query(probe.Position, shape.HalfDimensions, this.candidates);

            foreach (PhysicalShape sender in this.candidates)
            {
                // Work around self-collision
                if (sender == probe)
                    continue;

                if (this.CheckCollision(sender, shape))
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
            foreach (PhysicalShape receiver in this.receivers)
            {
                receiver.CollisionList.Clear();
                this.Query(receiver, receiver.CollisionList);
            }
        }

        private void Insert(PhysicalShape shape)
        {
            if (shape.CanSendCollisions)
            {
                this.database.Insert(shape);
            }
            if (shape.CanReceiveCollisions)
            {
                this.receivers.Add(shape);
            }
        }

        private void Remove(PhysicalShape shape)
        {
            if (shape.CanSendCollisions)
            {
                this.database.Remove(shape);
            }
            if (shape.CanReceiveCollisions)
            {
                this.receivers.Remove(shape);
            }
        }

        private bool CheckCollision(PhysicalShape shape1, PhysicalShape shape2)
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
