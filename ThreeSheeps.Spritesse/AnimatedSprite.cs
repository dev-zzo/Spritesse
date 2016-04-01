using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ThreeSheeps.Spritesse.Content;

namespace ThreeSheeps.Spritesse
{
    /// <summary>
    /// A sprite that supports frame animation.
    /// </summary>
    public class AnimatedSprite : Sprite
    {
        public AnimatedSprite(AnimationSet set, string name)
        {
            this.SpeedFactor = 1.0f;
            this.SetAnimation(set, name);
        }

        /// <summary>
        /// Returns whether an animation is being played.
        /// </summary>
        public bool Playing { get; set; }

        /// <summary>
        /// Playback speed factor; 2.0 means the animation plays 2x faster.
        /// </summary>
        public float SpeedFactor { get; set; }

        /// <summary>
        /// Set the specific animation and start playing
        /// </summary>
        /// <param name="set">Animation set content to use</param>
        /// <param name="name">Animation sequence name to play</param>
        public void SetAnimation(AnimationSet set, string name)
        {
            this.animationSet = set;
            this.spriteSheet = set.SpriteSheet;
            this.SetAnimation(name);
        }

        /// <summary>
        /// Set the specific animation and start playing
        /// </summary>
        /// <param name="name">Animation sequence name to play</param>
        public void SetAnimation(string name)
        {
            this.currentSequence = this.animationSet[name];
            this.ResetAnimation();
        }

        /// <summary>
        /// Reset the animation state so playback starts from the beginning.
        /// </summary>
        public void ResetAnimation()
        {
            this.timeElapsed = 0.0f;
            this.currentFrame = 0;
        }

        #region Overrides

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // No animation updates if we're done playing
            if (!this.Playing)
                return;

            // Add the elapsed game time
            this.timeElapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds * this.SpeedFactor;
            // Check the elapsed time against the current frame's delay
            AnimationFrame[] frames = this.currentSequence.Frames;
            int currentFrame = this.currentFrame;
            float delay = (float)frames[currentFrame].Delay;
            while (this.timeElapsed > delay)
            {
                // Subtract the delay and advance the frame counter
                this.timeElapsed -= delay;
                int nextFrame = currentFrame + 1;
                // Check whether we reached the end of animation sequence
                if (nextFrame >= frames.Length)
                {
                    // TODO: Fire an event?

                    if (this.currentSequence.Looped)
                    {
                        // If the animation is looped, then simply reset the counter.
                        currentFrame = 0;
                    }
                    else
                    {
                        // We are done playing.
                        this.Playing = false;
                        break;
                    }
                }
                else
                {
                    // Advance to the next frame
                    currentFrame = nextFrame;
                }
                // We are not done, so update the delay to check the next frame
                // This can happen if we skip animation frames due to update period being large.
                delay = (float)frames[currentFrame].Delay;
            }
            // Update the value
            this.currentFrame = currentFrame;
        }

        public override void Draw(SpriteBatch batch, float depth)
        {
            // Get the correct sprite definition
            AnimationFrame frame = this.currentSequence.Frames[this.currentFrame];
            SpriteDefinition definition = spriteSheet.Definitions[frame.SpriteIndex];
            // Calculate a corrected screen position
            Vector2 position = new Vector2(
                this.Position.X - definition.PivotOffset.X,
                this.Position.Y - definition.PivotOffset.Y);
            // Issue a draw call
            batch.Draw(
                spriteSheet.Texture,
                position,
                definition.SourceRectangle,
                this.TintColor,
                this.Rotation,
                Vector2.Zero,
                this.Scale,
                this.Effects,
                depth);
        }

        #endregion

        #region Private members

        protected AnimationSet animationSet;
        protected SpriteSheet spriteSheet;
        protected float timeElapsed;
        protected AnimationSequence currentSequence;
        protected int currentFrame;

        #endregion
    }
}
