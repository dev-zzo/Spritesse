using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using ThreeSheeps.Spritesse.Content;

namespace ThreeSheeps.Spritesse.PipelineExts
{
    [ContentTypeWriter]
    public class AnimationSetWriter : ContentTypeWriter<AnimationSetContent>
    {
        protected override void Write(ContentWriter output, AnimationSetContent value)
        {
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
                if (seq.Events != null)
                {
                    output.Write(seq.Events.Length);
                    foreach (AnimationEvent e in seq.Events)
                    {
                        output.Write(e.Position);
                        output.Write(e.Name);
                    }
                }
                else
                {
                    output.Write((int)0);
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
