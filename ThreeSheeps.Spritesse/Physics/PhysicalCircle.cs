using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace ThreeSheeps.Spritesse.Physics
{
    /// <summary>
    /// This is a circle shape.
    /// </summary>
    [DebuggerDisplay("Circle at ({Position.X}, {Position.Y}), radius {Radius}")]
    public sealed class PhysicalCircle : PhysicalShape
    {
        public new sealed class CreationInfo : PhysicalShape.CreationInfo
        {
            public float Radius;
        }

        public PhysicalCircle(CreationInfo info)
            : base(info)
        {
            this.radius = info.Radius;
        }

        public override Vector2 HalfDimensions
        {
            get { return new Vector2(this.radius, this.radius); }
        }

        /// <summary>
        /// Circle's radius.
        /// </summary>
        public float Radius
        {
            get { return this.radius; }
        }

        private float radius;
    }
}
