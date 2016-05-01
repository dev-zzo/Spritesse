using ThreeSheeps.Spritesse.Graphics;

namespace ThreeSheeps.Spritesse.Scene
{
    /// <summary>
    /// Implement this interface if your scene object has any renderables.
    /// </summary>
    public interface IRenderableSceneObject
    {
        /// <summary>
        /// This will be called by the scene to register all renderables for drawing.
        /// </summary>
        /// <param name="service">Scene renderer service to use</param>
        void RegisterRenderables(ISceneRendererService service);
    }
}
