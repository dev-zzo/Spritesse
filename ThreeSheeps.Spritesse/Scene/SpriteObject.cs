using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ThreeSheeps.Spritesse.Behaviour;
using ThreeSheeps.Spritesse.Content;

namespace ThreeSheeps.Spritesse.Scene
{
    public class SpriteObject : IDrawableSceneObject, ISmartSceneObject
    {
        public SpriteObject(SpriteSheet spriteSheet, int spriteIndex, AnimationSet animationSet = null)
        {
            this.spriteSheet = spriteSheet;
            this.SpriteIndex = spriteIndex;
            this.animationSet = animationSet;
            this.Visible = true;
            this.Depth = -1.0f;
            this.TintColor = Color.White;
            this.Effects = SpriteEffects.None;
            this.AnimationPlaying = false;
            this.SpeedFactor = 1.0f;
        }

        #region ISceneObject implementation

        public Point Position { get; set; }

        public void Update(GameTime gameTime, SceneState sceneState)
        {
            if (this.Behaviour != null)
            {
                this.Behaviour.Update(gameTime);
            }
            if (this.AnimationPlaying)
            {
                this.UpdateAnimation(gameTime);
            }
        }

        #endregion

        /// <summary>
        /// Color to be used for tinting
        /// </summary>
        public Color TintColor { get; set; }

        /// <summary>
        /// Flipping effects to apply
        /// </summary>
        public SpriteEffects Effects { get; set; }

        public int SpriteIndex { get; set; }

        public bool AnimationPlaying { get; set; }

        public float SpeedFactor { get; set; }

        #region IDrawableSceneObject implementation

        public float Depth { get; set; }

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

        public void Draw(SpriteBatch spriteBatch, SceneState sceneState, float depth)
        {
            SpriteDefinition definition = spriteSheet.Definitions[this.SpriteIndex];
            // Calculate a corrected screen position
            Vector2 position = new Vector2(
                this.Position.X - definition.PivotOffset.X,
                this.Position.Y - definition.PivotOffset.Y);
            // Issue a draw call
            spriteBatch.Draw(
                spriteSheet.Texture,
                position,
                definition.SourceRectangle,
                this.TintColor,
                0.0f,
                Vector2.Zero,
                1.0f,
                this.Effects,
                depth);
        }

        #endregion

        public IBehaviour Behaviour { get; set; }

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

        private void UpdateAnimation(GameTime gameTime)
        {
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
            this.SpriteIndex = this.currentSequence.Frames[this.currentFrame].SpriteIndex;
        }

        private SpriteSheet spriteSheet;
        private AnimationSet animationSet;
        private float timeElapsed;
        private AnimationSequence currentSequence;
        private int currentFrame;
    }
}
