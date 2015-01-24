using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// Draws an indicator showing where the map borders are.
    /// </summary>
    public class MapBorderDrawer
    {
        /// <summary>
        /// Creates a Rectangle from two points.
        /// </summary>
        /// <param name="min">Minimum points.</param>
        /// <param name="max">Maximum points.</param>
        /// <returns>Rectangle for the two points.</returns>
        static Rectangle CreateRect(Vector2 min, Vector2 max)
        {
            Vector2 size = max - min;
            return new Rectangle((int)min.X, (int)min.Y, (int)size.X, (int)size.Y);
        }

        /// <summary>
        /// Draws the map borders.
        /// </summary>
        /// <param name="sb">SpriteBatch to draw to.</param>
        /// <param name="map">Map to draw the borders for.</param>
        /// <param name="camera">Camera used to view the map.</param>
        public virtual void Draw(SpriteBatch sb, IMap map, Camera2D camera)
        {
            if (sb == null || sb.IsDisposed)
                return;
            if (map == null)
                return;
            if (camera == null)
                return;

            // Left border and corners
            if (camera.Min.X < 0)
            {
                Vector2 min = camera.Min;
                Vector2 max = new Vector2(Math.Min(0, camera.Max.X), camera.Max.Y);
                DrawBorder(sb, min, max);
            }

            // Right border and corners
            if (camera.Max.X > map.Width)
            {
                Vector2 min = new Vector2(Math.Max(camera.Min.X, map.Width), camera.Min.Y);
                Vector2 max = camera.Max;
                DrawBorder(sb, min, max);
            }

            // Top border
            if (camera.Min.Y < 0)
            {
                Vector2 min = new Vector2(Math.Max(camera.Min.X, 0), camera.Min.Y);
                Vector2 max = new Vector2(Math.Min(camera.Max.X, map.Width), Math.Min(camera.Max.Y, 0));
                DrawBorder(sb, min, max);
            }

            // Bottom border
            if (camera.Max.Y > map.Height)
            {
                Vector2 min = new Vector2(Math.Max(camera.Min.X, 0), Math.Max(camera.Min.Y, map.Height));
                Vector2 max = new Vector2(Math.Min(camera.Max.X, map.Width), camera.Max.Y);
                DrawBorder(sb, min, max);
            }
        }

        /// <summary>
        /// Draws a border.
        /// </summary>
        /// <param name="sb">SpriteBatch to draw to.</param>
        /// <param name="min">Minimum point to draw.</param>
        /// <param name="max">Maximum point to draw.</param>
        protected virtual void DrawBorder(SpriteBatch sb, Vector2 min, Vector2 max)
        {
            Color drawColor = new Color(255, 0, 0, 175);
            XNARectangle.Draw(sb, CreateRect(min, max), drawColor);
        }
    }
}