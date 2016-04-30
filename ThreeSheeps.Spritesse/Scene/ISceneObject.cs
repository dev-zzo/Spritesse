using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace ThreeSheeps.Spritesse.Scene
{
    public interface ISceneObject
    {
        Point Position { get; set; }

        void LoadContent(ContentManager manager);

        void Update(GameTime gameTime);
    }
}
