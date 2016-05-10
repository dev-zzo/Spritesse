namespace ThreeSheeps.Spritesse.Audio
{
    /// <summary>
    /// Represents a generic sound effect.
    /// </summary>
    public interface ISoundEffect
    {
        /// <summary>
        /// Whether the effect is currently stopped or playing.
        /// </summary>
        bool IsStopped { get; }
        /// <summary>
        /// Start the playback of the sound effect.
        /// </summary>
        void Play();
        /// <summary>
        /// Pause the playback.
        /// </summary>
        void Pause();
        /// <summary>
        /// Resume the playback.
        /// </summary>
        void Resume();
        /// <summary>
        /// Stop the playback.
        /// </summary>
        void Stop();
    }
}
