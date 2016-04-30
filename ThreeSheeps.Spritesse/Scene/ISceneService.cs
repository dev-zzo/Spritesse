using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace ThreeSheeps.Spritesse.Scene
{
    public interface ISceneService
    {
        /// <summary>
        /// ContentManager instance to be used to load scene related content.
        /// </summary>
        ContentManager Content { get; }
        /// <summary>
        /// Commands the scene (and objects added) to load all content.
        /// </summary>
        void LoadContent();
        /// <summary>
        /// Add a scene object.
        /// </summary>
        /// <param name="obj"></param>
        void AddObject(ISceneObject obj);
        /// <summary>
        /// Remove the scene object.
        /// </summary>
        /// <param name="obj"></param>
        void RemoveObject(ISceneObject obj);
        /// <summary>
        /// Wipe everything from the scene.
        /// </summary>
        void Clear();
    }
}
