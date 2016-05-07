using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace ThreeSheeps.Spritesse.Physics
{
    /// <summary>
    /// This is an axis-aligned box shape.
    /// </summary>
    [DebuggerDisplay("AAB at ({Position.X}, {Position.Y}), size ({HalfDimensions.X}, {HalfDimensions.Y})")]
    public sealed class PhysicalAxisAlignedBox : PhysicalShape
    {
        public new sealed class CreationInfo : PhysicalShape.CreationInfo
        {
            public Vector2 Dimensions;
        }

        public PhysicalAxisAlignedBox(CreationInfo info)
            : base(info)
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
