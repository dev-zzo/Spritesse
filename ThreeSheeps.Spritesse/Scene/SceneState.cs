using Microsoft.Xna.Framework;

namespace ThreeSheeps.Spritesse.Scene
{
    /// <summary>
    /// Encapsulates essential scene state for updates.
    /// </summary>
    public class SceneState
    {
        /// <summary>
        /// Revision number of data below; incremented with each change.
        /// </summary>
        public uint Revision { get; set; }

        /// <summary>
        /// Area visible on the screen, for visibility tests.
        /// </summary>
        public Rectangle CameraRectangle { get; set; }
    }
}
