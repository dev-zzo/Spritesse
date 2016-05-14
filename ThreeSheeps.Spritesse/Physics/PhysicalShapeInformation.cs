using Microsoft.Xna.Framework;

namespace ThreeSheeps.Spritesse.Physics
{
    /// <summary>
    /// This represents creation information for a shape.
    /// As there may be a lot of parameters, it is represented as a class
    /// instead of trying to pass everything via constructor arguments.
    /// </summary>
    public class PhysicalShapeInformation
    {
        public bool SendCollisions;
        public bool ReceiveCollisions;
        public Vector2 Position;
    }

    public class PhysicalCircleInformation : PhysicalShapeInformation
    {
        public float Radius;
    }

    public class PhysicalBoxInformation : PhysicalShapeInformation
    {
        public Vector2 Dimensions;
    }
}
