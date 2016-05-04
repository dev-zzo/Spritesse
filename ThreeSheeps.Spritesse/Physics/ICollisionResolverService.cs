using System.Collections.Generic;

namespace ThreeSheeps.Spritesse.Physics
{
    public interface ICollisionResolverService
    {
        void Insert(PhysicalShape shape);

        void Update(PhysicalShape shape);

        void Remove(PhysicalShape shape);

        void Query(PhysicalShape tester, IList<CollisionInformation> results);
    }
}
