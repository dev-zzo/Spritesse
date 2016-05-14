using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace ThreeSheeps.Spritesse.Physics
{
    /// <summary>
    /// This is an axis-aligned box shape.
    /// </summary>
    [DebuggerDisplay("AAB at ({Position.X}, {Position.Y}), size ({HalfDimensions.X}, {HalfDimensions.Y})")]
    internal sealed class PhysicalAxisAlignedBox : PhysicalShape
    {
        public PhysicalAxisAlignedBox(ICollisionDatabase database, PhysicalBoxInformation info)
            : base(database, info)
        {
            this.halfDimensions = info.Dimensions * 0.5f;
        }

        public override Vector2 HalfDimensions
        {
            get { return this.halfDimensions; }
        }

        private Vector2 halfDimensions;
    }
}
