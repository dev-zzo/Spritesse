using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using ThreeSheeps.Spritesse.Content;
using ThreeSheeps.Spritesse.Graphics;

namespace ThreeSheeps.Spritesse.Scene.Objects
{
    /// <summary>
    /// Encapsulates and represents a game location.
    /// This includes visual, physical, and audio data.
    /// </summary>
    public class LocationSceneObject : ISceneObject
    {
        public LocationSceneObject(string assetName)
        {
            this.assetName = assetName;
        }

        #region ISceneObject implementation

        public Point Position { get; set; }

        public void LoadContent(ContentManager manager, GameServiceContainer services)
        {
            ISceneRendererService renderer = services.GetService<ISceneRendererService>();
            this.location = manager.Load<Location>(this.assetName);
            foreach (TileMap layerContent in this.location.BackgroundLayers)
            {
                RenderableTileMap layer = new RenderableTileMap(layerContent);
                renderer.AddBackgroundObject(layer);
            }
            foreach (TileMap layerContent in this.location.ForegroundLayers)
            {
                RenderableTileMap layer = new RenderableTileMap(layerContent);
                renderer.AddForegroundObject(layer);
            }
        }

        public void Update(GameTime gameTime)
        {
        }

        #endregion

        private string assetName;
        private Location location;
    }
}
