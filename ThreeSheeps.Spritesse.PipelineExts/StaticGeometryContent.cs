using Microsoft.Xna.Framework;

namespace ThreeSheeps.Spritesse.PipelineExts
{
    public class StaticGeometryContent
    {
        public Vector2 Position;
    }

    public class StaticRectangleContent : StaticGeometryContent
    {
        public Vector2 Dimensions;
    }

    public class StaticCircleContent : StaticGeometryContent
    {
        public float Radius;
    }
}
