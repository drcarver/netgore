using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// Extensions for the SpriteBatch class.
    /// </summary>
    public static class SpriteBatchExtensions
    {
        /// <summary>
        /// Prepares the graphics device for drawing sprites with specified blending, sorting, and render state
        /// options, and a global transform matrix. Also makes the sprites drawn not use filtering.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw to.</param>
        public static void BeginUnfiltered(this SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            SetUnfiltered(spriteBatch);
        }

        /// <summary>
        /// Prepares the graphics device for drawing sprites with specified blending, sorting, and render state
        /// options, and a global transform matrix. Also makes the sprites drawn not use filtering.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw to.</param>
        /// <param name="blendMode">Blending options to use when rendering.</param>
        public static void BeginUnfiltered(this SpriteBatch spriteBatch, SpriteBlendMode blendMode)
        {
            spriteBatch.Begin(blendMode);
            SetUnfiltered(spriteBatch);
        }

        /// <summary>
        /// Prepares the graphics device for drawing sprites with specified blending, sorting, and render state
        /// options, and a global transform matrix. Also makes the sprites drawn not use filtering.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw to.</param>
        /// <param name="blendMode">Blending options to use when rendering.</param>
        /// <param name="sortMode">Sorting options to use when rendering.</param>
        /// <param name="stateMode">Rendering state options.</param>
        public static void BeginUnfiltered(this SpriteBatch spriteBatch, SpriteBlendMode blendMode, SpriteSortMode sortMode,
                                           SaveStateMode stateMode)
        {
            spriteBatch.Begin(blendMode, sortMode, stateMode);
            SetUnfiltered(spriteBatch);
        }

        /// <summary>
        /// Prepares the graphics device for drawing sprites with specified blending, sorting, and render state
        /// options, and a global transform matrix. Also makes the sprites drawn not use filtering.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw to.</param>
        /// <param name="blendMode">Blending options to use when rendering.</param>
        /// <param name="sortMode">Sorting options to use when rendering.</param>
        /// <param name="stateMode">Rendering state options.</param>
        /// <param name="transformMatrix">A matrix to apply to position, rotation, scale, and 
        /// depth data passed to SpriteBatch.Draw.</param>
        public static void BeginUnfiltered(this SpriteBatch spriteBatch, SpriteBlendMode blendMode, SpriteSortMode sortMode,
                                           SaveStateMode stateMode, Matrix transformMatrix)
        {
            spriteBatch.Begin(blendMode, sortMode, stateMode, transformMatrix);
            SetUnfiltered(spriteBatch);
        }

        /// <summary>
        /// Draws a string with shading.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw to.</param>
        /// <param name="spriteFont">SpriteFont to draw the string with.</param>
        /// <param name="text">The string to draw.</param>
        /// <param name="position">The position of the top-left corner of the string to draw.</param>
        /// <param name="fontColor">The font color.</param>
        /// <param name="borderColor">The shading color.</param>
        public static void DrawStringShaded(this SpriteBatch spriteBatch, SpriteFont spriteFont, string text, Vector2 position,
                                            Color fontColor, Color borderColor)
        {
            spriteBatch.DrawString(spriteFont, text, position - new Vector2(0, 1), borderColor);
            spriteBatch.DrawString(spriteFont, text, position - new Vector2(1, 0), borderColor);
            spriteBatch.DrawString(spriteFont, text, position + new Vector2(0, 1), borderColor);
            spriteBatch.DrawString(spriteFont, text, position + new Vector2(1, 0), borderColor);

            spriteBatch.DrawString(spriteFont, text, position, fontColor);
        }

        /// <summary>
        /// Sets a SpriteBatch to not use filtering.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to not use filtering on.</param>
        static void SetUnfiltered(SpriteBatch spriteBatch)
        {
            spriteBatch.GraphicsDevice.SamplerStates[0].MagFilter = TextureFilter.None;
        }
    }
}