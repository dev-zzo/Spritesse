using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace ThreeSheeps.Spritesse.Audio
{
    public sealed class AudioManagerComponent : GameComponent, IAudioManagerService
    {
        public AudioManagerComponent(Game game)
            : base(game)
        {
            game.Services.AddService(typeof(IAudioManagerService), this);
            this.bgmVolume = 1.0f;
            this.sfxVolume = 1.0f;
            this.uiVolume = 1.0f;
            this.listener = new AudioListener();
        }

        public float BgmVolume
        {
            get { return this.bgmVolume; }
            set { this.bgmVolume = value; this.bgmCategory.SetVolume(value); }
        }

        public float SfxVolume
        {
            get { return this.sfxVolume; }
            set { this.sfxVolume = value; this.sfxCategory.SetVolume(value); }
        }

        public float UIVolume
        {
            get { return this.uiVolume; }
            set { this.uiVolume = value; this.uiCategory.SetVolume(value); }
        }

        public void StartBgm(string trackName)
        {
            if (this.bgmCue != null)
            {
                this.bgmCue.Dispose();
            }

            this.bgmCue = this.bgmSoundBank.GetCue(trackName);
            this.bgmCue.Play();
        }

        public void PauseBgm()
        {
            this.bgmCue.Pause();
        }

        public void ResumeBgm()
        {
            this.bgmCue.Resume();
        }

        public void StopBgm()
        {
            this.bgmCue.Stop(AudioStopOptions.AsAuthored);
        }

        public ISoundEffect CreateSfx(string cueName)
        {
            Cue cue = this.sfxSoundBank.GetCue(cueName);
            XactSoundEffect instance = new XactSoundEffect(cue);
            return instance;
        }

        public IPositionedSoundEffect CreatePositionedSfx(string cueName)
        {
            Cue cue = this.sfxSoundBank.GetCue(cueName);
            AudioEmitter emitter = new AudioEmitter();
            cue.Apply3D(this.listener, emitter);
            XactPositionedSoundEffect instance = new XactPositionedSoundEffect(cue, emitter);
            this.positionedInstances.Add(instance);
            return instance;
        }

        public void Recycle(ISoundEffect sfx)
        {
            XactSoundEffect instance = sfx as XactSoundEffect;
            instance.CueInstance.Dispose();
        }

        public void PauseAllSfx()
        {
            foreach (XactPositionedSoundEffect instance in this.positionedInstances)
            {
                instance.Pause();
            }
        }

        public void ResumeAllSfx()
        {
            foreach (XactPositionedSoundEffect instance in this.positionedInstances)
            {
                instance.Resume();
            }
        }

        public void PlayUICue(string cueName)
        {
            this.uiSoundBank.PlayCue(cueName);
        }

        public override void Initialize()
        {
            string rootDirectory = this.Game.Content.RootDirectory;
            
            this.audioEngine = new AudioEngine(rootDirectory + "\\Audio\\main.xgs");
            this.bgmWaveBank = new WaveBank(this.audioEngine, rootDirectory + "\\Audio\\BGM.xwb", 0, 16);
            this.bgmSoundBank = new SoundBank(this.audioEngine, rootDirectory + "\\Audio\\BGM.xsb");
            //this.sfxWaveBank = new WaveBank(this.audioEngine, rootDirectory + "\\Audio\\SFX.xwb");
            //this.sfxSoundBank = new SoundBank(this.audioEngine, rootDirectory + "\\Audio\\SFX.xsb");
            this.audioEngine.Update();
            this.bgmCategory = this.audioEngine.GetCategory("Music");
            //this.sfxCategory = this.audioEngine.GetCategory("SFX");
            //this.uiCategory = this.audioEngine.GetCategory("User Interface");
        }

        public override void Update(GameTime gameTime)
        {
            // TODO: update listener

            foreach (XactPositionedSoundEffect instance in this.positionedInstances)
            {
                Cue cue = instance.CueInstance;
                if (cue.IsPlaying)
                {
                    cue.Apply3D(this.listener, instance.Emitter);
                }
            }

            this.audioEngine.Update();
        }

        private AudioEngine audioEngine;

        private WaveBank bgmWaveBank;
        private SoundBank bgmSoundBank;
        private AudioCategory bgmCategory;
        private float bgmVolume;
        private Cue bgmCue;

        private WaveBank sfxWaveBank;
        private SoundBank sfxSoundBank;
        private AudioCategory sfxCategory;
        private float sfxVolume;

        private WaveBank uiWaveBank;
        private SoundBank uiSoundBank;
        private AudioCategory uiCategory;
        private float uiVolume;

        private AudioListener listener;
        private readonly ISet<XactPositionedSoundEffect> positionedInstances = new HashSet<XactPositionedSoundEffect>();
    }
}
