using Microsoft.Xna.Framework;

namespace ThreeSheeps.Spritesse.Audio
{
    /// <summary>
    /// Represents a sound effect that can be positioned in space.
    /// </summary>
    public interface IPositionedSoundEffect : ISoundEffect
    {
        /// <summary>
        /// The effect's position in the world.
        /// </summary>
        Vector2 Position { get; set; }
    }
}
