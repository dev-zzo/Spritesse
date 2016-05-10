using Microsoft.Xna.Framework.Audio;

namespace ThreeSheeps.Spritesse.Audio
{
    internal class XactSoundEffect : ISoundEffect
    {
        internal XactSoundEffect(Cue cue)
        {
            this.cue = cue;
        }

        internal Cue CueInstance { get { return this.cue; } }

        public bool IsStopped { get { return this.cue.IsStopped; } }

        public void Play()
        {
            this.cue.Play();
        }

        public void Pause()
        {
            this.cue.Pause();
        }

        public void Resume()
        {
            this.cue.Resume();
        }

        public void Stop()
        {
            this.cue.Stop(AudioStopOptions.AsAuthored);
        }

        private Cue cue;
    }
}
