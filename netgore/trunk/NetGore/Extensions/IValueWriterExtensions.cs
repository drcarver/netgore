using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore.IO;

namespace NetGore
{
    /// <summary>
    /// Extensions for the IValueWriter.
    /// </summary>
    public static class IValueWriterExtensions
    {
        public static void Write(this IValueWriter writer, string name, SpriteCategory value)
        {
            // TODO: ?? Try to remove as many usages of this as possible
            writer.Write(name, value.ToString());
        }

        public static void Write(this IValueWriter writer, string name, ContentAssetName value)
        {
            // TODO: ?? Try to remove as many usages of this as possible
            writer.Write(name, value.Value);
        }

        public static void Write(this IValueWriter writer, string name, SpriteTitle value)
        {
            // TODO: ?? Try to remove as many usages of this as possible
            writer.Write(name, value.ToString());
        }

        public static void Write(this IValueWriter writer, string name, SpriteCategorization value)
        {
            writer.Write(name, value.ToString());
        }

        /// <summary>
        /// Writes an unsigned integer with the specified range to an IValueWriter.
        /// </summary>
        /// <param name="writer">IValueWriter to write to.</param>
        /// <param name="value">Value to write.</param>
        /// <param name="name">Name of the value to write.</param>
        /// <param name="minValue">Minimum (inclusive) value that the written value can be.</param>
        /// <param name="maxValue">Maximum (inclusive) value that the written value can be.</param>
        public static void Write(this IValueWriter writer, string name, uint value, uint minValue, uint maxValue)
        {
            if (value < minValue || value > maxValue)
                throw new ArgumentOutOfRangeException("value", "Value parameter must be between minValue and maxValue.");
            if (maxValue < minValue)
                throw new ArgumentOutOfRangeException("maxValue", "MaxValue must be greater than or equal to MinValue.");

            // Find the number of bits required for the range of desired values
            uint maxWriteValue = maxValue - minValue;
            int bitsRequired = BitOps.RequiredBits(maxWriteValue);

            // Subtract the minimum value from the value since we want to write how high above the minimum value
            // the value is, not the actual value
            uint offsetFromMin = value - minValue;
            writer.Write(name, offsetFromMin, bitsRequired);

            Debug.Assert((value - minValue) <= maxWriteValue);
            Debug.Assert((1 << bitsRequired) >= maxWriteValue);
        }

        /// <summary>
        /// Writes a signed integer with the specified range to an IValueWriter.
        /// </summary>
        /// <param name="writer">IValueWriter to write to.</param>
        /// <param name="value">Value to write.</param>
        /// <param name="name">Name of the value to write.</param>
        /// <param name="minValue">Minimum (inclusive) value that the written value can be.</param>
        /// <param name="maxValue">Maximum (inclusive) value that the written value can be.</param>
        public static void Write(this IValueWriter writer, string name, int value, int minValue, int maxValue)
        {
            if (value < minValue || value > maxValue)
                throw new ArgumentOutOfRangeException("value", "Value parameter must be between minValue and maxValue.");
            if (maxValue < minValue)
                throw new ArgumentOutOfRangeException("maxValue", "MaxValue must be greater than or equal to MinValue.");

            // Find the number of bits required for the range of desired values
            uint maxWriteValue = (uint)(maxValue - minValue);
            int bitsRequired = BitOps.RequiredBits(maxWriteValue);

            // Subtract the minimum value from the value since we want to write how high above the minimum value
            // the value is, not the actual value
            uint offsetFromMin = (uint)(value - minValue);
            writer.Write(name, offsetFromMin, bitsRequired);

            Debug.Assert((value - minValue) <= maxWriteValue);
            Debug.Assert((1 << bitsRequired) >= maxWriteValue);
        }

        /// <summary>
        /// Writes a Vector2.
        /// </summary>
        /// <param name="writer">IValueWriter to write to.</param>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        public static void Write(this IValueWriter writer, string name, Vector2 value)
        {
            if (writer.SupportsNameLookup)
            {
                // We are using name lookup, so we have to combine the values so we use only one name
                string x = Parser.Invariant.ToString(value.X);
                string y = Parser.Invariant.ToString(value.Y);
                writer.Write(name, x + "," + y);
            }
            else
            {
                // Not using name lookup, so just write them out
                writer.Write(null, value.X);
                writer.Write(null, value.Y);
            }
        }

        /// <summary>
        /// Writes a Rectangle.
        /// </summary>
        /// <param name="writer">IValueWriter to write to.</param>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        public static void Write(this IValueWriter writer, string name, Rectangle value)
        {
            if (writer.SupportsNameLookup)
            {
                // We are using name lookup, so we have to combine the values so we use only one name
                string x = Parser.Invariant.ToString(value.X);
                string y = Parser.Invariant.ToString(value.Y);
                string w = Parser.Invariant.ToString(value.Width);
                string h = Parser.Invariant.ToString(value.Height);
                writer.Write(name, x + "," + y + "," + w + "," + h);
            }
            else
            {
                // Not using name lookup, so just write them out
                writer.Write(null, value.X);
                writer.Write(null, value.Y);
                writer.Write(null, value.Width);
                writer.Write(null, value.Height);
            }
        }

        /// <summary>
        /// Writes a Color.
        /// </summary>
        /// <param name="writer">IValueWriter to write to.</param>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        public static void Write(this IValueWriter writer, string name, Color value)
        {
            if (writer.SupportsNameLookup)
            {
                // We are using name lookup, so we have to combine the values so we use only one name
                const string delimiter = ",";
                StringBuilder sb = new StringBuilder(16);
                sb.Append(Parser.Invariant.ToString(value.R));
                sb.Append(delimiter);
                sb.Append(Parser.Invariant.ToString(value.G));
                sb.Append(delimiter);
                sb.Append(Parser.Invariant.ToString(value.B));
                sb.Append(delimiter);
                sb.Append(Parser.Invariant.ToString(value.A));
                writer.Write(name, sb.ToString());
            }
            else
            {
                // Not using name lookup, so just write them out
                // ReSharper disable RedundantCast
                writer.Write(null, (byte)value.R);
                writer.Write(null, (byte)value.G);
                writer.Write(null, (byte)value.B);
                writer.Write(null, (byte)value.A);
                // ReSharper restore RedundantCast
            }
        }
    }
}