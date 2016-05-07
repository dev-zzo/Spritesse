﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using ThreeSheeps.Spritesse.Content;
using ThreeSheeps.Spritesse.Graphics;
using ThreeSheeps.Spritesse.Physics;

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
            ICollisionResolverService collider = services.GetService<ICollisionResolverService>();
            foreach (PhysicalShape.CreationInfo info in this.location.StaticShapes)
            {
                if (info is PhysicalCircle.CreationInfo)
                {
                    PhysicalShape obj = new PhysicalCircle(info as PhysicalCircle.CreationInfo);
                    collider.Insert(obj);
                }
                else if (info is PhysicalAxisAlignedBox.CreationInfo)
                {
                    PhysicalShape obj = new PhysicalAxisAlignedBox(info as PhysicalAxisAlignedBox.CreationInfo);
                    collider.Insert(obj);
                }
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
