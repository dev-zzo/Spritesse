using Microsoft.Xna.Framework.Graphics;

namespace ThreeSheeps.Spritesse.Graphics
{
    /// <summary>
    /// Provides means to render a scene.
    /// </summary>
    public interface ISceneRendererService
    {
        /// <summary>
        /// The viewable area used to render scene into.
        /// </summary>
        Viewport Viewport { get; }

        /// <summary>
        /// Adds a background layer object; these are drawn first. Typically, this is a tile map.
        /// </summary>
        /// <param name="obj"></param>
        void AddBackgroundObject(IRenderable obj);

        /// <summary>
        /// Adds a foreground layer object; these are drawn last. Typically, this is a tile map.
        /// </summary>
        /// <param name="obj"></param>
        void AddForegroundObject(IRenderable obj);

        /// <summary>
        /// Registers a renderable object for drawing in an objects phase.
        /// </summary>
        /// <param name="obj"></param>
        void RegisterObject(IRenderable obj);

        /// <summary>
        /// Unregisters a renderable object.
        /// </summary>
        /// <param name="obj"></param>
        void UnregisterObject(IRenderable obj);

        /// <summary>
        /// Clears every registered object.
        /// </summary>
        void Clear();
    }
}
