using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreeSheeps.Spritesse.Physics
{
    public interface ICollisionResolverService
    {
        void Insert(PhysicalShape shape);

        void Remove(PhysicalShape shape);

        void Query(PhysicalShape tester, IList<CollisionInformation> results);
    }
}
