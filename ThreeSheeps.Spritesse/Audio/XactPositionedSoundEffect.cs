using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace ThreeSheeps.Spritesse.Audio
{
    internal class XactPositionedSoundEffect : XactSoundEffect, IPositionedSoundEffect
    {
        internal XactPositionedSoundEffect(Cue cue, AudioEmitter emitter)
            : base(cue)
        {
            this.emitter = emitter;
        }

        public Vector2 Position
        {
            get { return new Vector2(this.emitter.Position.X, this.emitter.Position.Y) * (1.0f / SCALE); }
            set { this.emitter.Position = new Vector3(value.X * SCALE, value.Y * SCALE, 0.0f); }
        }

        internal AudioEmitter Emitter { get { return this.emitter; } }

        internal void Update(AudioListener listener)
        {
            if (!this.CueInstance.IsPlaying)
                return;

            this.CueInstance.Apply3D(listener, this.emitter);
        }

        private const float SCALE = 1.0f;
        private AudioEmitter emitter;
    }
}
