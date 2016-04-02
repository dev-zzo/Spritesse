using System;
using Microsoft.Xna.Framework.Content;

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
        public string Name;
        public bool Looped;
        public AnimationFrame[] Frames;
    }

    [Serializable]
    public sealed class AnimationSetContent
    {
        public string SpriteSheet;

        [ContentSerializer(CollectionItemName = "Sequence")]
        public AnimationSequence[] Animations;
    }
}
