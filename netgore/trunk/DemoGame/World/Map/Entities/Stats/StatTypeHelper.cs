using System.Collections.Generic;
using System.Linq;
using NetGore.IO;

namespace DemoGame
{
    public sealed class StatTypeHelper : EnumIOHelper<StatType>
    {
        static readonly StatTypeHelper _instance;

        /// <summary>
        /// Initializes the <see cref="StatTypeHelper"/> class.
        /// </summary>
        static StatTypeHelper()
        {
            _instance = new StatTypeHelper();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StatTypeHelper"/> class.
        /// </summary>
        StatTypeHelper()
        {
        }

        /// <summary>
        /// Gets the <see cref="StatTypeHelper"/> instance.
        /// </summary>
        public static StatTypeHelper Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Gets an IEnumerable of all of the <see cref="StatType"/>s who's base value can be raised by a Character.
        /// </summary>
        public static IEnumerable<StatType> RaisableStats
        {
            get { return Values; }
        }

        /// <summary>
        /// When overridden in the derived class, casts an int to type <see cref="StatType"/>.
        /// </summary>
        /// <param name="value">The int value.</param>
        /// <returns>The <paramref name="value"/> casted to type <see cref="StatType"/>.</returns>
        protected override StatType FromInt(int value)
        {
            return (StatType)value;
        }

        /// <summary>
        /// When overridden in the derived class, casts type <see cref="StatType"/> to an int.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The <paramref name="value"/> casted to an int.</returns>
        protected override int ToInt(StatType value)
        {
            return (int)value;
        }
    }
}