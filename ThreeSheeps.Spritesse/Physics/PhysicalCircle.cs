using Microsoft.Xna.Framework;

namespace ThreeSheeps.Spritesse.Physics
{
    public sealed class PhysicalCircle : PhysicalShape
    {
        public new sealed class CreationInfo : PhysicalShape.CreationInfo
        {
            public float Radius;
        }

        public PhysicalCircle(ICollisionResolverService resolver, CreationInfo info)
            : base(resolver, info)
        {
            this.radius = info.Radius;
        }

        public override Vector2 HalfDimensions
        {
            get { return new Vector2(this.radius, this.radius); }
        }

        private float radius;
    }
}
