using System;

namespace ThreeSheeps.Spritesse.PipelineExts
{
    [Serializable]
    public sealed class AnimationFrame
    {
        public int SpriteIndex { get; set; }
        public float Delay { get; set; }
    }

    [Serializable]
    public sealed class AnimationSequence
    {
        public string Name { get; set; }
        public bool Looped { get; set; }
        public AnimationFrame[] Frames { get; set; }
    }

    [Serializable]
    public sealed class AnimationSet
    {
        public string SpriteSheetName { get; set; }
        public AnimationSequence[] Animations { get; set; }
    }
}
