﻿using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using ThreeSheeps.Spritesse.Content;

namespace ThreeSheeps.Spritesse.PipelineExts
{
    [ContentTypeWriter]
    public class AnimationSetWriter : ContentTypeWriter<AnimationSetContent>
    {
        protected override void Write(ContentWriter output, AnimationSetContent value)
        {
            output.Write(value.SpriteSheet);
            output.Write(value.Animations.Length);
            foreach (AnimationSequence seq in value.Animations)
            {
                output.Write(seq.Name);
                output.Write(seq.Looped);
                output.Write(seq.Frames.Length);
                foreach (AnimationFrame frame in seq.Frames)
                {
                    output.Write((ushort)frame.SpriteIndex);
                    output.Write((ushort)frame.Delay);
                }
            }
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(AnimationSetReader).AssemblyQualifiedName;
        }

        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return typeof(AnimationSet).AssemblyQualifiedName;
        }
    }
}
