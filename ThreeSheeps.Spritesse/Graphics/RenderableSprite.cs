using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ThreeSheeps.Spritesse.Content;

namespace ThreeSheeps.Spritesse.Graphics
{
    /// <summary>
    /// Handles animation events.
    /// </summary>
    /// <param name="name">Event name which has occurred</param>
    public delegate void AnimationEventDelegate(string name);

    /// <summary>
    /// Implements a single (animated) sprite on the screen.
    /// </summary>
    public class RenderableSprite : IRenderable
    {
        public RenderableSprite(SpriteSheet spriteSheet, int spriteIndex, AnimationSet animationSet = null, string animationName = null)
        {
            this.spriteSheet = spriteSheet;
            this.spriteIndex = spriteIndex;
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

        public bool Visible { get; set; }

        public Rectangle BoundingBox
        {
            get
            {
                SpriteDefinition definition = spriteSheet.Definitions[this.spriteIndex];
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
        /// Fired when an event occurs in the currently playing animation.
        /// </summary>
        public event AnimationEventDelegate AnimationEventOccurred;

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
            this.frameTimeElapsed = 0.0f;
            this.currentFrame = 0;
            this.nextEvent = 0;
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
                float elapsedTime = this.AnimationSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                this.UpdateAnimation(elapsedTime);
                this.UpdateEvents(elapsedTime);
            }
        }

        /// <summary>
        /// Renders the sprite.
        /// </summary>
        /// <param name="context">Rendering context object</param>
        public void Draw(SceneRenderContext context)
        {
            SpriteDefinition definition = spriteSheet.Definitions[this.spriteIndex];
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

        private void UpdateAnimation(float elapsedTime)
        {
            this.frameTimeElapsed += elapsedTime;
            float totalTime = this.currentSequence.TotalTime;
            if (this.frameTimeElapsed >= totalTime)
            {
                if (this.currentSequence.Looped)
                {
                    this.currentFrame = 0;
                    while (this.frameTimeElapsed >= totalTime)
                    {
                        this.frameTimeElapsed -= totalTime;
                    }
                }
                else
                {
                    this.AnimationPlaying = false;
                }
            }

            AnimationFrame[] frames = this.currentSequence.Frames;
            for (int nextFrame = this.currentFrame; nextFrame < frames.Length; ++nextFrame)
            {
                if (frames[nextFrame].Position > this.frameTimeElapsed)
                    break;
                this.currentFrame = nextFrame;
            }

            this.spriteIndex = frames[this.currentFrame].SpriteIndex;
        }

        private void UpdateEvents(float elapsedTime)
        {
            if (this.currentSequence.Events == null)
            {
                return;
            }

            // Check remaining events
            this.eventTimeElapsed += elapsedTime;
            this.CheckEvents();

            if (this.currentSequence.Looped)
            {
                // Handle looping
                float totalTime = this.currentSequence.TotalTime;
                while (this.eventTimeElapsed >= totalTime)
                {
                    this.eventTimeElapsed -= totalTime;
                    this.nextEvent = 0;
                    this.CheckEvents();
                }
            }
        }

        private void CheckEvents()
        {
            AnimationEvent[] events = this.currentSequence.Events;
            int index;
            for (index = this.nextEvent; index < events.Length; ++index)
            {
                if (events[index].Position > this.frameTimeElapsed)
                    break;
                // Report the event.
                if (this.AnimationEventOccurred != null)
                {
                    this.AnimationEventOccurred(events[index].Name);
                }
            }
            this.nextEvent = index;
        }

        private SpriteSheet spriteSheet;
        private int spriteIndex;

        private AnimationSet animationSet;
        private float frameTimeElapsed;
        private AnimationSequence currentSequence;
        private int currentFrame;

        private float eventTimeElapsed;
        private int nextEvent;

        #endregion
    }
}
