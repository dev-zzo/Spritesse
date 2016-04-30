using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ThreeSheeps.Spritesse.Content;

namespace ThreeSheeps.Spritesse.Graphics
{
    public class RenderableSprite : IRenderable
    {
        public RenderableSprite(SpriteSheet spriteSheet, int spriteIndex, AnimationSet animationSet = null, string animationName = null)
        {
            this.spriteSheet = spriteSheet;
            this.SpriteIndex = spriteIndex;
            this.Visible = true;
            this.TintColor = Color.White;
            this.Effects = SpriteEffects.None;
            this.AnimationSpeed = 1.0f;
            this.animationSet = animationSet;
            if (animationName != null)
            {
                this.AnimationPlaying = true;
                SetAnimation(animationName);
            }
            else
            {
                this.AnimationPlaying = false;
            }
        }

        /// <summary>
        /// Object position in screen space, in pixels
        /// </summary>
        public Point Position { get; set; }

        /// <summary>
        /// Color to be used for tinting
        /// </summary>
        public Color TintColor { get; set; }

        /// <summary>
        /// Flipping effects to apply
        /// </summary>
        public SpriteEffects Effects { get; set; }

        public int SpriteIndex { get; set; }

        public bool Visible { get; set; }

        public Rectangle BoundingBox
        {
            get
            {
                SpriteDefinition definition = spriteSheet.Definitions[this.SpriteIndex];
                return new Rectangle(
                    this.Position.X - definition.PivotOffset.X,
                    this.Position.Y - definition.PivotOffset.Y,
                    definition.SourceRectangle.Width,
                    definition.SourceRectangle.Height);
            }
        }

        #region Animation related knobs

        /// <summary>
        /// Whether an animation is currently playing
        /// </summary>
        public bool AnimationPlaying { get; set; }

        /// <summary>
        /// Speed factor for animation playback
        /// </summary>
        public float AnimationSpeed { get; set; }

        /// <summary>
        /// Set the specific animation and start playing
        /// </summary>
        /// <param name="set">Animation set content to use</param>
        /// <param name="name">Animation sequence name to play</param>
        public void SetAnimation(AnimationSet set, string name)
        {
            this.animationSet = set;
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

        #endregion

        /// <summary>
        /// Updates the sprite state, if needed.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            if (this.AnimationPlaying)
            {
                this.UpdateAnimation(gameTime);
            }
        }

        /// <summary>
        /// Renders the sprite.
        /// </summary>
        /// <param name="context">Rendering context object</param>
        public void Draw(SceneRenderContext context)
        {
            SpriteDefinition definition = spriteSheet.Definitions[this.SpriteIndex];
            // Issue a draw call
            context.SpriteBatch.Draw(
                spriteSheet.Texture,
                this.BoundingBox,
                definition.SourceRectangle,
                this.TintColor,
                0.0f,
                Vector2.Zero,
                this.Effects,
                0.0f);
        }

        #region Implementation details

        private void UpdateAnimation(GameTime gameTime)
        {
            // Add the elapsed game time
            this.timeElapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds * this.AnimationSpeed;
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
                        this.AnimationPlaying = false;
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
            this.SpriteIndex = this.currentSequence.Frames[currentFrame].SpriteIndex;
        }

        private SpriteSheet spriteSheet;
        private AnimationSet animationSet;
        private float timeElapsed;
        private AnimationSequence currentSequence;
        private int currentFrame;

        #endregion
    }
}
