using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ThreeSheeps.Spritesse.Physics
{
    public class CollisionResolverComponent : GameComponent, ICollisionResolverService
    {
        public CollisionResolverComponent(Game game)
            : base(game)
        {
        }

        public void Insert(IPhysical shape)
        {
        }

        public void Remove(IPhysical shape)
        {
        }

        public void Query(IPhysical tester, IList<CollisionInformation> results)
        {
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
