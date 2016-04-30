using Microsoft.Xna.Framework;

namespace ThreeSheeps.Spritesse.Graphics
{
    /// <summary>
    /// Represents a visible object.
    /// </summary>
    public interface IRenderable
    {
        /// <summary>
        /// Whether the object should be considered for rendering.
        /// </summary>
        bool Visible { get; set; }

        /// <summary>
        /// Bounding box for visibility testing.
        /// </summary>
        Rectangle BoundingBox { get; }

        /// <summary>
        /// Allows the object to draw itself.
        /// </summary>
        /// <param name="context"></param>
        void Draw(SceneRenderContext context);
    }
}
