using System.Linq;
using NetGore.IO;

namespace NetGore
{
    /// <summary>
    /// Provides helper methods for the <see cref="CollisionType"/> enum.
    /// </summary>
    public sealed class CollisionTypeHelper : EnumIOHelper<CollisionType>
    {
        static readonly CollisionTypeHelper _instance;

        /// <summary>
        /// Initializes the <see cref="CollisionTypeHelper"/> class.
        /// </summary>
        static CollisionTypeHelper()
        {
            _instance = new CollisionTypeHelper();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CollisionTypeHelper"/> class.
        /// </summary>
        CollisionTypeHelper()
        {
        }

        /// <summary>
        /// Gets the <see cref="CollisionTypeHelper"/> instance.
        /// </summary>
        public static CollisionTypeHelper Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// When overridden in the derived class, casts an int to type <see cref="CollisionType"/>.
        /// </summary>
        /// <param name="value">The int value.</param>
        /// <returns>The <paramref name="value"/> casted to type <see cref="CollisionType"/>.</returns>
        protected override CollisionType FromInt(int value)
        {
            return (CollisionType)value;
        }

        /// <summary>
        /// When overridden in the derived class, casts type <see cref="CollisionType"/> to an int.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The <paramref name="value"/> casted to an int.</returns>
        protected override int ToInt(CollisionType value)
        {
            return (int)value;
        }
    }
}