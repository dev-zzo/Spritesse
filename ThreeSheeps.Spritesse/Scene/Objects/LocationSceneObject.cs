using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using ThreeSheeps.Spritesse.Content;
using ThreeSheeps.Spritesse.Graphics;

namespace ThreeSheeps.Spritesse.Scene.Objects
{
    public class LocationSceneObject : ISceneObject, IRenderableSceneObject
    {
        public LocationSceneObject(string assetName)
        {
            this.assetName = assetName;
        }

        public Point Position { get; set; }

        public void LoadContent(ContentManager manager)
        {
            this.location = manager.Load<Location>(this.assetName);
            foreach (TileMap layerContent in this.location.BackgroundLayers)
            {
                RenderableTileMap layer = new RenderableTileMap(layerContent);
                this.bgLayers.Add(layer);
            }
            foreach (TileMap layerContent in this.location.ForegroundLayers)
            {
                RenderableTileMap layer = new RenderableTileMap(layerContent);
                this.fgLayers.Add(layer);
            }
        }

        public void RegisterRenderables(ISceneRendererService service)
        {
            foreach (IRenderable obj in this.bgLayers)
            {
                service.AddBackgroundObject(obj);
            }
            foreach (IRenderable obj in this.fgLayers)
            {
                service.AddForegroundObject(obj);
            }
            // Not needed any more.
            this.bgLayers.Clear();
            this.fgLayers.Clear();
        }

        public void Update(GameTime gameTime)
        {
        }

        private string assetName;
        private Location location;
        private readonly List<IRenderable> bgLayers = new List<IRenderable>();
        private readonly List<IRenderable> fgLayers = new List<IRenderable>();
    }
}
