using System;
using System.Linq;

namespace NetGore
{
    /// <summary>
    /// Describes the different types of characters in a string.
    /// </summary>
    [Flags]
    public enum CharType
    {
        /// <summary>
        /// Uppercase alphabet characters (A to Z).
        /// </summary>
        AlphaUpper = 1 << 0,

        /// <summary>
        /// Lowercase alphabet characters (a to z).
        /// </summary>
        AlphaLower = 1 << 1,

        /// <summary>
        /// Numeric characters (0 to 9).
        /// </summary>
        Numeric = 1 << 2,

        /// <summary>
        /// Punctuation characters (ie: !, ", _, -, etc...)
        /// </summary>
        Punctuation = 1 << 3,

        /// <summary>
        /// Whitespace characters (ie: spaces).
        /// </summary>
        Whitespace = 1 << 4,

        /// <summary>
        /// Uppercase or lowercase alphabet characters.
        /// </summary>
        Alpha = AlphaUpper | AlphaLower,

        /// <summary>
        /// All defined character groups.
        /// </summary>
        All = -1,
    }
}