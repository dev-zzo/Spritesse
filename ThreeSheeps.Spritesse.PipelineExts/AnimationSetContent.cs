﻿using Microsoft.Xna.Framework.Content;

namespace ThreeSheeps.Spritesse.PipelineExts
{
    public sealed class AnimationFrame
    {
        public int SpriteIndex;
        public uint Delay;
    }

    public sealed class AnimationEvent
    {
        public uint Position;
        public string Name;
    }

    public sealed class AnimationSequence
    {
        public string Name;
        public bool Looped;
        [ContentSerializerIgnore]
        public uint TotalLength;
        public AnimationFrame[] Frames;
        [ContentSerializer(Optional = true)]
        public AnimationEvent[] Events;
    }

    public sealed class AnimationSetContent
    {
        [ContentSerializer(CollectionItemName = "Sequence")]
        public AnimationSequence[] Animations;
    }
}
