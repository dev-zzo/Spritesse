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

        protected struct CollisionCheckerInfo
        {
            public CollisionCheckerInfo(Type type1, Type type2, CheckCollisionDelegate checker)
            {
                this.Type1 = type1;
                this.Type2 = type2;
                this.Checker = checker;
            }

            public Type Type1;
            public Type Type2;
            public CheckCollisionDelegate Checker;
        }

        protected virtual void BuildCollisionCheckerList(List<CollisionCheckerInfo> checkers)
        {
            checkers.Add(new CollisionCheckerInfo(typeof(PhysicalCircle), typeof(PhysicalCircle),
                new CheckCollisionDelegate(CollisionChecker.CheckCollisionCC)));
            checkers.Add(new CollisionCheckerInfo(typeof(PhysicalAxisAlignedBox), typeof(PhysicalCircle),
                new CheckCollisionDelegate(CollisionChecker.CheckCollisionBC)));
            checkers.Add(new CollisionCheckerInfo(typeof(PhysicalAxisAlignedBox), typeof(PhysicalAxisAlignedBox),
                new CheckCollisionDelegate(CollisionChecker.CheckCollisionBB)));
        }

        private void BuildCollisionCheckers()
        {
            List<CollisionCheckerInfo> checkers = new List<CollisionCheckerInfo>();
            this.BuildCollisionCheckerList(checkers);
            this.collisionCheckers = checkers.ToArray();
        }

        private bool CheckCollision(PhysicalShape shape1, PhysicalShape shape2)
        {
            Type shape1Type = shape1.GetType();
            Type shape2Type = shape2.GetType();

            foreach (CollisionCheckerInfo tp in this.collisionCheckers)
            {
                if (shape1Type == tp.Type1 && shape2Type == tp.Type2)
                {
                    return tp.Checker(shape1, shape2);
                }
                if (shape1Type == tp.Type2 && shape2Type == tp.Type1)
                {
                    return tp.Checker(shape2, shape1);
                }
            }

            throw new ArgumentException("no applicable collision checkers found");
        }

        private readonly ICollisionDatabase database;
        private CollisionCheckerInfo[] collisionCheckers;
        private readonly ISet<PhysicalShape> receivers = new HashSet<PhysicalShape>();
        private readonly List<PhysicalShape> candidates = new List<PhysicalShape>();
    }
}
