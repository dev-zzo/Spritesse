using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

//
// The Scene
//
// This is basically where everything non-UI related occurs.
// It is a collection of all gameplay related entities, represented
// by arbitrary objects implementing the ISceneObject interface.
//
// A scene object is an abstraction that allows an arbitrary composition
// of visual, audio, and physical entities to be represented and managed
// as a single game entity. It represents whatever "glue logic" remains
// after you decompose your object into the above parts.
//
// The scene is "observed" via the scene camera component and
// rendered via the scene renderer component.
//

namespace ThreeSheeps.Spritesse.Scene
{
    /// <summary>
    /// Manages the scene.
    /// The scene comprises everything that is related to game play:
    /// * Background and foreground layers
    /// * Dynamic game objects (the player(s), NPCs, items, etc)
    /// </summary>
    public interface ISceneService
    {
        /// <summary>
        /// ContentManager instance to be used to load scene related content.
        /// </summary>
        ContentManager Content { get; }

        /// <summary>
        /// Commands the scene (and any objects added) to load content.
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
