using Microsoft.Xna.Framework.Content;

namespace ThreeSheeps.Spritesse.PipelineExts
{
    public sealed class AnimationFrame
    {
        public int SpriteIndex { get; set; }
        public float Delay { get; set; }
    }

    public sealed class AnimationSequence
    {
        public string Name;
        public bool Looped;
        public AnimationFrame[] Frames;
    }

    public sealed class AnimationSetContent
    {
        [ContentSerializer(CollectionItemName = "Sequence")]
        public AnimationSequence[] Animations;
    }
}
