using System.IO;
using System.Linq;

namespace NetGore
{
    /// <summary>
    /// An immutable string that represents a file or directory path. This can be implicitly converted
    /// both to and from a normal string.
    /// </summary>
    public sealed class PathString
    {
        readonly string _path;

        /// <summary>
        /// Initializes a new instance of the <see cref="PathString"/> class.
        /// </summary>
        /// <param name="path">Path of this PathString.</param>
        public PathString(string path)
        {
            _path = path;
        }

        /// <summary>
        /// Joins this <see cref="PathString"/> with a sub-path.
        /// </summary>
        /// <param name="subPath">Name of the child file or folder from this PathString.</param>
        /// <returns>A new PathString containing the path joining this PathString with the <paramref name="subPath"/>.</returns>
        public PathString Join(string subPath)
        {
            string newPath = Path.Combine(this, subPath);

            if (newPath == this)
                return this;

            return new PathString(newPath);
        }

        ///<summary>
        ///Returns a <see cref="T:System.String" /> that represents the current <see cref="T:System.Object" />.
        ///</summary>
        ///
        ///<returns>
        ///A <see cref="T:System.String" /> that represents the current <see cref="T:System.Object" />.
        ///</returns>
        ///
        public override string ToString()
        {
            return _path;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="NetGore.PathString"/> to <see cref="System.String"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator string(PathString value)
        {
            return value._path;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.String"/> to <see cref="NetGore.PathString"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator PathString(string value)
        {
            return new PathString(value);
        }
    }
}