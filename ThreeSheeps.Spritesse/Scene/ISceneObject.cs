using Microsoft.Xna.Framework;

namespace ThreeSheeps.Spritesse.Scene
{
    public interface ISceneObject
    {
        Point Position { get; set; }

        void Update(GameTime gameTime, SceneState sceneState);
    }
}
