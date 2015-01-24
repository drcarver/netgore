using System.Data;
using System.Linq;
using NetGore.IO;

namespace NetGore
{
    /// <summary>
    /// A stat value type with an underlying value type of an int.
    /// </summary>
    public struct StatValueInt : IStatValueType
    {
        /// <summary>
        /// The underlying value.
        /// </summary>
        readonly int _value;

        /// <summary>
        /// StatValueInt constructor.
        /// </summary>
        /// <param name="value">The initial value.</param>
        public StatValueInt(int value)
        {
            _value = value;
        }

        #region IStatValueType Members

        /// <summary>
        /// Gets the value of this IStatValueType as an integer.
        /// </summary>
        /// <returns>The value of this IStatValueType as an integer.</returns>
        public int GetValue()
        {
            return _value;
        }

        /// <summary>
        /// Sets the value of this IStatValueType.
        /// </summary>
        /// <param name="value">The integer value to set this IStatValueType.</param>
        public IStatValueType SetValue(int value)
        {
            return new StatValueInt(value);
        }

        /// <summary>
        /// Writes this IStatValueType's value to the given <paramref name="bitStream"/>.
        /// </summary>
        /// <param name="bitStream">The <paramref name="bitStream"/> to write this IStatValueType to.</param>
        public void Write(BitStream bitStream)
        {
            bitStream.Write(_value);
        }

        /// <summary>
        /// Reads the value of this IStatValueType from a <paramref name="bitStream"/>.
        /// </summary>
        /// <param name="bitStream">The BitStream to read the value from.</param>
        public IStatValueType Read(BitStream bitStream)
        {
            int value = bitStream.ReadInt();
            return new StatValueInt(value);
        }

        /// <summary>
        /// Reads the value of this IStatValueType from a <paramref name="dataRecord"/> at the given <paramref name="ordinal"/>.
        /// </summary>
        /// <param name="dataRecord">The IDataReader to read from.</param>
        /// <param name="ordinal">The ordinal in the <paramref name="dataRecord"/> to read from.</param>
        public IStatValueType Read(IDataRecord dataRecord, int ordinal)
        {
            int value = dataRecord.GetInt32(ordinal);
            return new StatValueInt(value);
        }

        /// <summary>
        /// Creates a deep copy of the IStatValueType. The returned IStatValueType is of the same Type of the object that
        /// this method was called on, and contains the same value.
        /// </summary>
        /// <returns>The deep copy of this IStatValueType.</returns>
        public IStatValueType DeepCopy()
        {
            return new StatValueInt(_value);
        }

        #endregion
    }
}