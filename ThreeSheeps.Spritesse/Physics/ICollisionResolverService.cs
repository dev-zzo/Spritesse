using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreeSheeps.Spritesse.Physics
{
    public interface ICollisionResolverService
    {
        void Insert(IPhysical shape);

        void Remove(IPhysical shape);

        void Query(IPhysical tester, IList<CollisionInformation> results);
    }
}
