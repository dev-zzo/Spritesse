using Microsoft.Xna.Framework;

namespace ThreeSheeps.Spritesse.Scene
{
    public interface ISceneService
    {
        /// <summary>
        /// Wipe everything from the scene.
        /// </summary>
        void Clear();
        /// <summary>
        /// Load location content.
        /// </summary>
        /// <param name="name"></param>
        void LoadLocation(string name);
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
    }
}
