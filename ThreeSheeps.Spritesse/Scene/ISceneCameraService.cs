﻿using Microsoft.Xna.Framework;

namespace ThreeSheeps.Spritesse.Scene
{
    /// <summary>
    /// Represents an abstract camera.
    /// </summary>
    public interface ISceneCameraService
    {
        /// <summary>
        /// Position of the camera = center of the view rectangle.
        /// </summary>
        Point Position { get; set; }

        /// <summary>
        /// View rectangle = area visible on the screen.
        /// </summary>
        Rectangle ViewRectangle { get; }

        /// <summary>
        /// Make the camera follow the given scene object.
        /// </summary>
        /// <param name="obj"></param>
        void AttachToObject(ISceneObject obj);
    }
}
