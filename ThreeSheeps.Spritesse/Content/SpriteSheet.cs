using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ThreeSheeps.Spritesse.Content
{
    /// <summary>
    /// Defines a sprite within a sprite sheet
    /// </summary>
    public sealed class SpriteDefinition
    {
        internal SpriteDefinition(Rectangle sourceRectangle, Point pivotOffset)
        {
            this.SourceRectangle = sourceRectangle;
            this.PivotOffset = pivotOffset;
        }

        /// <summary>
        /// Locates the sprite within the texture
        /// </summary>
        public Rectangle SourceRectangle { get; private set; }

        /// <summary>
        /// Offset of the pivot point (from the top left)
        /// </summary>
        public Point PivotOffset { get; private set; }
    }

    /// <summary>
    /// Defines a number of sprites over the given texture
    /// </summary>
    public sealed class SpriteSheet
    {
        /// <summary>
        /// Constructor available only within this assembly
        /// </summary>
        internal SpriteSheet(Texture2D texture, SpriteDefinition[] definitions)
        {
            this.Texture = texture;
            this.definitions = definitions;
        }

        /// <summary>
        /// The reference to the associated texture
        /// </summary>
        public Texture2D Texture { get; private set; }

        /// <summary>
        /// A list of defined sprites
        /// </summary>
        public IList<SpriteDefinition> Definitions
        {
            get { return this.definitions; }
        }

        private SpriteDefinition[] definitions;
    }

    /// <summary>
    /// Loader class for the sprite sheet content type
    /// </summary>
    public sealed class SpriteSheetReader : ContentTypeReader<SpriteSheet>
    {
        /// <summary>
        /// Loads a sprite sheet
        /// </summary>
        /// <param name="input"></param>
        /// <param name="existingInstance"></param>
        /// <returns>New sprite sheet instance</returns>
        protected override SpriteSheet Read(ContentReader input, SpriteSheet existingInstance)
        {
            // Handle definitions
            uint definitionCount = input.ReadUInt32();
            SpriteDefinition[] definitions = new SpriteDefinition[definitionCount];
            for (uint index = 0; index < definitionCount; ++index)
            {
                definitions[index] = this.ReadDefinition(input);
            }
            Texture2D texture = input.ReadObject<Texture2D>();

            // Done
            return new SpriteSheet(texture, definitions);
        }

        /// <summary>
        /// Read a single definition from the input stream
        /// </summary>
        /// <param name="input"></param>
        /// <returns>New definition instance</returns>
        private SpriteDefinition ReadDefinition(ContentReader input)
        {
            // Read the source rectangle
            int x = input.ReadInt16();
            int y = input.ReadInt16();
            int w = input.ReadInt16();
            int h = input.ReadInt16();

            // Read the pivot offset
            int offset_x = input.ReadInt16();
            int offset_y = input.ReadInt16();

            return new SpriteDefinition(
                new Rectangle(x, y, w, h),
                new Point(offset_x, offset_y));
        }
    }
}
