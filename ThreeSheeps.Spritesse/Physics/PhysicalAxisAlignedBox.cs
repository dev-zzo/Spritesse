using Microsoft.Xna.Framework;

namespace ThreeSheeps.Spritesse.Physics
{
    public sealed class PhysicalAxisAlignedBox : PhysicalShape
    {
        public new sealed class CreationInfo : PhysicalShape.CreationInfo
        {
            public Vector2 Dimensions;
        }

        public PhysicalAxisAlignedBox(ICollisionResolverService resolver, CreationInfo info)
            : base(resolver, info)
        {
            this.halfDims = info.Dimensions * 0.5f;
        }

        public override Vector2 HalfDimensions
        {
            get { return this.halfDims; }
        }

        private Vector2 halfDims;
    }
}
