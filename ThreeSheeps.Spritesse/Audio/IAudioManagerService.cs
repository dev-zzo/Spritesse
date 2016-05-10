namespace ThreeSheeps.Spritesse.Audio
{
    /// <summary>
    /// Provides audible objects and services for the game.
    /// </summary>
    public interface IAudioManagerService
    {
        /// <summary>
        /// Adjust background music volume.
        /// </summary>
        float BgmVolume { get; set; }
        /// <summary>
        /// Adjust in-game sound effects volume.
        /// </summary>
        float SfxVolume { get; set; }
        /// <summary>
        /// Adjust UI sound effects volume.
        /// </summary>
        float UIVolume { get; set; }

        /// <summary>
        /// Set and start playing a background music track.
        /// </summary>
        /// <param name="cueName"></param>
        void StartBgm(string cueName);
        /// <summary>
        /// Pause the playback of the BGM.
        /// </summary>
        void PauseBgm();
        /// <summary>
        /// Resume the playback of the BGM.
        /// </summary>
        void ResumeBgm();
        /// <summary>
        /// Stop the playback of the BGM.
        /// </summary>
        void StopBgm();

        /// <summary>
        /// Create an in-game sound effect instance.
        /// </summary>
        /// <param name="cueName">Sound cue name to instantiate</param>
        /// <returns>Sound effect instance</returns>
        ISoundEffect CreateSfx(string cueName);
        /// <summary>
        /// Create an in-game sound effect instance with player-relative positioning.
        /// </summary>
        /// <param name="cueName">Sound cue name to instantiate</param>
        /// <returns>Sound effect instance</returns>
        IPositionedSoundEffect CreatePositionedSfx(string cueName);
        /// <summary>
        /// Recycle the sound effect instance.
        /// </summary>
        /// <param name="sfx">Instance to be recycled</param>
        void Recycle(ISoundEffect sfx);

        /// <summary>
        /// Pause the playback of all in-game sound effects.
        /// </summary>
        void PauseAllSfx();

        /// <summary>
        /// Resume the playback of all in-game sound effects.
        /// </summary>
        void ResumeAllSfx();

        /// <summary>
        /// Play an UI sound effect.
        /// </summary>
        /// <param name="cueName">Sound cue to play</param>
        void PlayUICue(string cueName);
    }
}
