﻿using System;
using Microsoft.Xna.Framework.Content;

namespace ThreeSheeps.Spritesse.Content
{
    /// <summary>
    /// Defines a single animation frame in a sequence
    /// </summary>
    public struct AnimationFrame
    {
        /// <summary>
        /// Index of a sprite in the sprite sheet
        /// </summary>
        public ushort SpriteIndex;

        /// <summary>
        /// Delay in milliseconds
        /// </summary>
        public ushort Delay;
    }

    /// <summary>
    /// Defines an animation sequence consisting of 1+ frames
    /// </summary>
    public sealed class AnimationSequence
    {
        public AnimationSequence(string name, bool looped, AnimationFrame[] frames)
        {
            this.Name = name;
            this.Looped = looped;
            this.Frames = frames;
        }

        /// <summary>
        /// A name to identify this animation
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Whether the animation will start again when the end is reached
        /// </summary>
        public bool Looped { get; private set; }

        /// <summary>
        /// Frames defined in the animation sequence
        /// </summary>
        public AnimationFrame[] Frames { get; private set; }
    }

    /// <summary>
    /// Defines an animation set (1+ animations) over a sprite sheet.
    /// </summary>
    public sealed class AnimationSet
    {
        public AnimationSet(SpriteSheet spriteSheet, AnimationSequence[] animations)
        {
            this.SpriteSheet = spriteSheet;
            this.animations = animations;
        }

        /// <summary>
        /// Associated sprite sheet for these animations
        /// </summary>
        public SpriteSheet SpriteSheet { get; private set; }

        /// <summary>
        /// Retrieve a specific animation sequence
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Animation sequence instance</returns>
        public AnimationSequence this[string name]
        {
            get
            {
                foreach (AnimationSequence a in this.animations)
                {
                    if (a.Name == name)
                    {
                        return a;
                    }
                }
                throw new IndexOutOfRangeException("name");
            }
        }

        private AnimationSequence[] animations;
    }

    /// <summary>
    /// Loader class for the animation set content type.
    /// </summary>
    public sealed class AnimationSetReader : ContentTypeReader<AnimationSet>
    {
        protected override AnimationSet Read(ContentReader input, AnimationSet existingInstance)
        {
            string spriteSheetName = input.ReadString();
            SpriteSheet spriteSheet = input.ContentManager.Load<SpriteSheet>(spriteSheetName);
            int animationCount = input.ReadInt32();
            AnimationSequence[] animations = new AnimationSequence[animationCount];
            for (int animIndex = 0; animIndex < animationCount; ++animIndex)
            {
                string name = input.ReadString();
                bool looped = input.ReadBoolean();
                int frameCount = input.ReadInt32();
                AnimationFrame[] frames = new AnimationFrame[frameCount];
                for (int frameIndex = 0; frameIndex < frameCount; ++frameIndex)
                {
                    frames[frameIndex].SpriteIndex = input.ReadUInt16();
                    frames[frameIndex].Delay = input.ReadUInt16();
                }
                animations[animIndex] = new AnimationSequence(name, looped, frames);
            }
            return new AnimationSet(spriteSheet, animations);
        }
    }
}
