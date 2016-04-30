using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreeSheeps.Spritesse.Graphics
{
    public interface ISceneRendererService
    {
        /// <summary>
        /// Adds a background layer object; these are drawn first. Typically, this is a tile map.
        /// </summary>
        /// <param name="obj"></param>
        void AddBackgroundObject(IRenderable obj);
        /// <summary>
        /// Adds a foreground layer object; these are drawn last. Typically, this is a tile map.
        /// </summary>
        /// <param name="obj"></param>
        void AddForegroundObject(IRenderable obj);
        /// <summary>
        /// Registers a renderable object for drawing in an objects phase.
        /// </summary>
        /// <param name="obj"></param>
        void RegisterObject(IRenderable obj);
        /// <summary>
        /// Unregisters a renderable object.
        /// </summary>
        /// <param name="obj"></param>
        void UnregisterObject(IRenderable obj);
        /// <summary>
        /// Clears every registered object.
        /// </summary>
        void Clear();
    }
}
