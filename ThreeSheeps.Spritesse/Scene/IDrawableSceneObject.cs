using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ThreeSheeps.Spritesse.Scene
{
    public interface IDrawableSceneObject : ISceneObject
    {
        float Depth { get; set; }

        bool Visible { get; set; }

        Rectangle BoundingBox { get; }

        void Draw(SpriteBatch spriteBatch, SceneState sceneState, float depth);
    }
}
