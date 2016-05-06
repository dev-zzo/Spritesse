using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace ThreeSheeps.Spritesse.Scene
{
    /// <summary>
    /// This defines a scene object -- an entity "performing" on a scene.
    /// </summary>
    public interface ISceneObject
    {
        /// <summary>
        /// Object position, in pixels.
        /// </summary>
        Point Position { get; set; }

        /// <summary>
        /// Will be called by the scene component and is the place 
        /// to load relevant content.
        /// </summary>
        /// <param name="manager">Content manager instance to use for loading</param>
        /// <param name="services">Services container to use</param>
        void LoadContent(ContentManager manager, GameServiceContainer services);

        /// <summary>
        /// Will be called by the scene component and is the place
        /// to update anything that needs updating on each tick.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        void Update(GameTime gameTime);
    }
}
