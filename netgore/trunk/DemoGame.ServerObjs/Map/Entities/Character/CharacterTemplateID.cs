using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using DemoGame.Server.Queries;
using NetGore;
using NetGore.Db;
using NetGore.IO;

namespace DemoGame.Server
{
    /// <summary>
    /// Represents a unique ID for a Character template.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct CharacterTemplateID : IComparable, IConvertible, IFormattable, IComparable<int>, IEquatable<int>
    {
        /// <summary>
        /// Represents the largest possible value of CharacterTemplateID. This field is constant.
        /// </summary>
        public const int MaxValue = int.MaxValue;

        /// <summary>
        /// Represents the smallest possible value of CharacterTemplateID. This field is constant.
        /// </summary>
        public const int MinValue = int.MinValue;

        /// <summary>
        /// The underlying value. This contains the actual value of the struct instance.
        /// </summary>
        readonly int _value;

        /// <summary>
        /// CharacterTemplateID constructor.
        /// </summary>
        /// <param name="value">Value to assign to the new CharacterTemplateID.</param>
        public CharacterTemplateID(int value)
        {
            if (value < MinValue || value > MaxValue)
                throw new ArgumentOutOfRangeException("value");

            _value = value;
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <returns>
        /// True if <paramref name="other"/> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        /// <param name="other">Another object to compare to. 
        /// </param>
        public bool Equals(CharacterTemplateID other)
        {
            return other._value == _value;
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <returns>
        /// True if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        /// <param name="obj">Another object to compare to. 
        /// </param>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (obj.GetType() != typeof(CharacterTemplateID))
                return false;
            return Equals((CharacterTemplateID)obj);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        /// <summary>
        /// Gets the raw internal value of this CharacterTemplateID.
        /// </summary>
        /// <returns>The raw internal value.</returns>
        public int GetRawValue()
        {
            return _value;
        }

        /// <summary>
        /// Reads an CharacterTemplateID from an IValueReader.
        /// </summary>
        /// <param name="reader">IValueReader to read from.</param>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>The CharacterTemplateID read from the IValueReader.</returns>
        public static CharacterTemplateID Read(IValueReader reader, string name)
        {
            var value = reader.ReadInt(name);
            return new CharacterTemplateID(value);
        }

        /// <summary>
        /// Reads an CharacterTemplateID from an IDataReader.
        /// </summary>
        /// <param name="reader">IDataReader to get the value from.</param>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The CharacterTemplateID read from the IDataReader.</returns>
        public static CharacterTemplateID Read(IDataReader reader, int i)
        {
            var value = reader.GetValue(i);
            if (value is int)
                return new CharacterTemplateID((int)value);

            var convertedValue = Convert.ToInt32(value);
            return new CharacterTemplateID(convertedValue);
        }

        /// <summary>
        /// Reads an CharacterTemplateID from an IDataReader.
        /// </summary>
        /// <param name="reader">IDataReader to get the value from.</param>
        /// <param name="name">The name of the field to find.</param>
        /// <returns>The CharacterTemplateID read from the IDataReader.</returns>
        public static CharacterTemplateID Read(IDataReader reader, string name)
        {
            return Read(reader, reader.GetOrdinal(name));
        }

        /// <summary>
        /// Reads an CharacterTemplateID from an IValueReader.
        /// </summary>
        /// <param name="bitStream">BitStream to read from.</param>
        /// <returns>The CharacterTemplateID read from the BitStream.</returns>
        public static CharacterTemplateID Read(BitStream bitStream)
        {
            var value = bitStream.ReadInt();
            return new CharacterTemplateID(value);
        }

        /// <summary>
        /// Converts the numeric value of this instance to its equivalent string representation.
        /// </summary>
        /// <returns>The string representation of the value of this instance, consisting of a sequence
        /// of digits ranging from 0 to 9, without leading zeroes.</returns>
        public override string ToString()
        {
            return _value.ToString();
        }

        /// <summary>
        /// Writes the CharacterTemplateID to an IValueWriter.
        /// </summary>
        /// <param name="writer">IValueWriter to write to.</param>
        /// <param name="name">Unique name of the CharacterTemplateID that will be used to distinguish it
        /// from other values when reading.</param>
        public void Write(IValueWriter writer, string name)
        {
            writer.Write(name, _value);
        }

        /// <summary>
        /// Writes the CharacterTemplateID to an IValueWriter.
        /// </summary>
        /// <param name="bitStream">BitStream to write to.</param>
        public void Write(BitStream bitStream)
        {
            bitStream.Write(_value);
        }

        #region IComparable Members

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has these meanings: 
        ///                     Value 
        ///                     Meaning 
        ///                     Less than zero 
        ///                     This instance is less than <paramref name="obj"/>. 
        ///                     Zero 
        ///                     This instance is equal to <paramref name="obj"/>. 
        ///                     Greater than zero 
        ///                     This instance is greater than <paramref name="obj"/>. 
        /// </returns>
        /// <param name="obj">An object to compare with this instance. 
        ///                 </param><exception cref="T:System.ArgumentException"><paramref name="obj"/> is not the same type as this instance. 
        ///                 </exception>
        public int CompareTo(object obj)
        {
            return _value.CompareTo(obj);
        }

        #endregion

        #region IComparable<int> Members

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: 
        ///                     Value 
        ///                     Meaning 
        ///                     Less than zero 
        ///                     This object is less than the <paramref name="other"/> parameter.
        ///                     Zero 
        ///                     This object is equal to <paramref name="other"/>. 
        ///                     Greater than zero 
        ///                     This object is greater than <paramref name="other"/>. 
        /// </returns>
        /// <param name="other">An object to compare with this object.
        ///                 </param>
        public int CompareTo(int other)
        {
            return _value.CompareTo(other);
        }

        #endregion

        #region IConvertible Members

        /// <summary>
        /// Returns the <see cref="T:System.TypeCode"/> for this instance.
        /// </summary>
        /// <returns>
        /// The enumerated constant that is the <see cref="T:System.TypeCode"/> of the class or value type that implements this interface.
        /// </returns>
        public TypeCode GetTypeCode()
        {
            return _value.GetTypeCode();
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent Boolean value using the specified culture-specific formatting information.
        /// </summary>
        /// <returns>
        /// A Boolean value equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information. 
        ///                 </param>
        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            return ((IConvertible)_value).ToBoolean(provider);
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent Unicode character using the specified culture-specific formatting information.
        /// </summary>
        /// <returns>
        /// A Unicode character equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information. 
        ///                 </param>
        char IConvertible.ToChar(IFormatProvider provider)
        {
            return ((IConvertible)_value).ToChar(provider);
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 8-bit signed integer using the specified culture-specific formatting information.
        /// </summary>
        /// <returns>
        /// An 8-bit signed integer equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information. 
        ///                 </param>
        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            return ((IConvertible)_value).ToSByte(provider);
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 8-bit unsigned integer using the specified culture-specific formatting information.
        /// </summary>
        /// <returns>
        /// An 8-bit unsigned integer equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information. 
        ///                 </param>
        byte IConvertible.ToByte(IFormatProvider provider)
        {
            return ((IConvertible)_value).ToByte(provider);
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 16-bit signed integer using the specified culture-specific formatting information.
        /// </summary>
        /// <returns>
        /// An 16-bit signed integer equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information. 
        ///                 </param>
        short IConvertible.ToInt16(IFormatProvider provider)
        {
            return ((IConvertible)_value).ToInt16(provider);
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 16-bit unsigned integer using the specified culture-specific formatting information.
        /// </summary>
        /// <returns>
        /// An 16-bit unsigned integer equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information. 
        ///                 </param>
        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            return ((IConvertible)_value).ToUInt16(provider);
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 32-bit signed integer using the specified culture-specific formatting information.
        /// </summary>
        /// <returns>
        /// An 32-bit signed integer equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information. 
        ///                 </param>
        int IConvertible.ToInt32(IFormatProvider provider)
        {
            return ((IConvertible)_value).ToInt32(provider);
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 32-bit unsigned integer using the specified culture-specific formatting information.
        /// </summary>
        /// <returns>
        /// An 32-bit unsigned integer equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information. 
        ///                 </param>
        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            return ((IConvertible)_value).ToUInt32(provider);
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 64-bit signed integer using the specified culture-specific formatting information.
        /// </summary>
        /// <returns>
        /// An 64-bit signed integer equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information. 
        ///                 </param>
        long IConvertible.ToInt64(IFormatProvider provider)
        {
            return ((IConvertible)_value).ToInt64(provider);
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 64-bit unsigned integer using the specified culture-specific formatting information.
        /// </summary>
        /// <returns>
        /// An 64-bit unsigned integer equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information. 
        ///                 </param>
        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            return ((IConvertible)_value).ToUInt64(provider);
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent single-precision floating-point number using the specified culture-specific formatting information.
        /// </summary>
        /// <returns>
        /// A single-precision floating-point number equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information. 
        ///                 </param>
        float IConvertible.ToSingle(IFormatProvider provider)
        {
            return ((IConvertible)_value).ToSingle(provider);
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent double-precision floating-point number using the specified culture-specific formatting information.
        /// </summary>
        /// <returns>
        /// A double-precision floating-point number equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information. 
        ///                 </param>
        double IConvertible.ToDouble(IFormatProvider provider)
        {
            return ((IConvertible)_value).ToDouble(provider);
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent <see cref="T:System.Decimal"/> number using the specified culture-specific formatting information.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Decimal"/> number equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information. 
        ///                 </param>
        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            return ((IConvertible)_value).ToDecimal(provider);
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent <see cref="T:System.DateTime"/> using the specified culture-specific formatting information.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.DateTime"/> instance equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information. 
        ///                 </param>
        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            return ((IConvertible)_value).ToDateTime(provider);
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent <see cref="T:System.String"/> using the specified culture-specific formatting information.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> instance equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information. 
        ///                 </param>
        public string ToString(IFormatProvider provider)
        {
            return ((IConvertible)_value).ToString(provider);
        }

        /// <summary>
        /// Converts the value of this instance to an <see cref="T:System.Object"/> of the specified <see cref="T:System.Type"/> that has an equivalent value, using the specified culture-specific formatting information.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Object"/> instance of type <paramref name="conversionType"/> whose value is equivalent to the value of this instance.
        /// </returns>
        /// <param name="conversionType">The <see cref="T:System.Type"/> to which the value of this instance is converted. 
        ///                 </param><param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information. 
        ///                 </param>
        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            return ((IConvertible)_value).ToType(conversionType, provider);
        }

        #endregion

        #region IEquatable<int> Members

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.
        ///                 </param>
        public bool Equals(int other)
        {
            return _value.Equals(other);
        }

        #endregion

        #region IFormattable Members

        /// <summary>
        /// Formats the value of the current instance using the specified format.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> containing the value of the current instance in the specified format.
        /// </returns>
        /// <param name="format">The <see cref="T:System.String"/> specifying the format to use.
        ///                     -or- 
        ///                 null to use the default format defined for the type of the <see cref="T:System.IFormattable"/> implementation. 
        ///                 </param><param name="formatProvider">The <see cref="T:System.IFormatProvider"/> to use to format the value.
        ///                     -or- 
        ///                 null to obtain the numeric format information from the current locale setting of the operating system. 
        ///                 </param>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return _value.ToString(format, formatProvider);
        }

        #endregion

        /// <summary>
        /// Implements operator ++.
        /// </summary>
        /// <param name="l">The CharacterTemplateID to increment.</param>
        /// <returns>The incremented CharacterTemplateID.</returns>
        public static CharacterTemplateID operator ++(CharacterTemplateID l)
        {
            return new CharacterTemplateID(l._value + 1);
        }

        /// <summary>
        /// Implements operator --.
        /// </summary>
        /// <param name="l">The CharacterTemplateID to decrement.</param>
        /// <returns>The decremented CharacterTemplateID.</returns>
        public static CharacterTemplateID operator --(CharacterTemplateID l)
        {
            return new CharacterTemplateID(l._value - 1);
        }

        /// <summary>
        /// Implements operator +.
        /// </summary>
        /// <param name="left">Left side argument.</param>
        /// <param name="right">Right side argument.</param>
        /// <returns>Result of the left side plus the right side.</returns>
        public static CharacterTemplateID operator +(CharacterTemplateID left, CharacterTemplateID right)
        {
            return new CharacterTemplateID(left._value + right._value);
        }

        /// <summary>
        /// Implements operator -.
        /// </summary>
        /// <param name="left">Left side argument.</param>
        /// <param name="right">Right side argument.</param>
        /// <returns>Result of the left side minus the right side.</returns>
        public static CharacterTemplateID operator -(CharacterTemplateID left, CharacterTemplateID right)
        {
            return new CharacterTemplateID(left._value - right._value);
        }

        /// <summary>
        /// Implements operator ==.
        /// </summary>
        /// <param name="left">Left side argument.</param>
        /// <param name="right">Right side argument.</param>
        /// <returns>If the two arguments are equal.</returns>
        public static bool operator ==(CharacterTemplateID left, int right)
        {
            return left._value == right;
        }

        /// <summary>
        /// Implements operator !=.
        /// </summary>
        /// <param name="left">Left side argument.</param>
        /// <param name="right">Right side argument.</param>
        /// <returns>If the two arguments are not equal.</returns>
        public static bool operator !=(CharacterTemplateID left, int right)
        {
            return left._value != right;
        }

        /// <summary>
        /// Implements operator ==.
        /// </summary>
        /// <param name="left">Left side argument.</param>
        /// <param name="right">Right side argument.</param>
        /// <returns>If the two arguments are equal.</returns>
        public static bool operator ==(int left, CharacterTemplateID right)
        {
            return left == right._value;
        }

        /// <summary>
        /// Implements operator !=.
        /// </summary>
        /// <param name="left">Left side argument.</param>
        /// <param name="right">Right side argument.</param>
        /// <returns>If the two arguments are not equal.</returns>
        public static bool operator !=(int left, CharacterTemplateID right)
        {
            return left != right._value;
        }

        /// <summary>
        /// Casts a CharacterTemplateID to an Int32.
        /// </summary>
        /// <param name="CharacterTemplateID">CharacterTemplateID to cast.</param>
        /// <returns>The Int32.</returns>
        public static explicit operator int(CharacterTemplateID CharacterTemplateID)
        {
            return CharacterTemplateID._value;
        }

        /// <summary>
        /// Casts an Int32 to a CharacterTemplateID.
        /// </summary>
        /// <param name="value">Int32 to cast.</param>
        /// <returns>The CharacterTemplateID.</returns>
        public static explicit operator CharacterTemplateID(int value)
        {
            return new CharacterTemplateID(value);
        }

        /// <summary>
        /// Implements operator >.
        /// </summary>
        /// <param name="left">Left side argument.</param>
        /// <param name="right">Right side argument.</param>
        /// <returns>If the left argument is greater than the right.</returns>
        public static bool operator >(int left, CharacterTemplateID right)
        {
            return left > right._value;
        }

        /// <summary>
        /// Implements operator <.
        /// </summary>
        /// <param name="left">Left side argument.</param>
        /// <param name="right">Right side argument.</param>
        /// <returns>If the right argument is greater than the left.</returns>
        public static bool operator <(int left, CharacterTemplateID right)
        {
            return left < right._value;
        }

        /// <summary>
        /// Implements operator >.
        /// </summary>
        /// <param name="left">Left side argument.</param>
        /// <param name="right">Right side argument.</param>
        /// <returns>If the left argument is greater than the right.</returns>
        public static bool operator >(CharacterTemplateID left, CharacterTemplateID right)
        {
            return left._value > right._value;
        }

        /// <summary>
        /// Implements operator <.
        /// </summary>
        /// <param name="left">Left side argument.</param>
        /// <param name="right">Right side argument.</param>
        /// <returns>If the right argument is greater than the left.</returns>
        public static bool operator <(CharacterTemplateID left, CharacterTemplateID right)
        {
            return left._value < right._value;
        }

        /// <summary>
        /// Implements operator >.
        /// </summary>
        /// <param name="left">Left side argument.</param>
        /// <param name="right">Right side argument.</param>
        /// <returns>If the left argument is greater than the right.</returns>
        public static bool operator >(CharacterTemplateID left, int right)
        {
            return left._value > right;
        }

        /// <summary>
        /// Implements operator <.
        /// </summary>
        /// <param name="left">Left side argument.</param>
        /// <param name="right">Right side argument.</param>
        /// <returns>If the right argument is greater than the left.</returns>
        public static bool operator <(CharacterTemplateID left, int right)
        {
            return left._value < right;
        }

        /// <summary>
        /// Implements operator >=.
        /// </summary>
        /// <param name="left">Left side argument.</param>
        /// <param name="right">Right side argument.</param>
        /// <returns>If the left argument is greater than or equal to the right.</returns>
        public static bool operator >=(int left, CharacterTemplateID right)
        {
            return left >= right._value;
        }

        /// <summary>
        /// Implements operator <=.
        /// </summary>
        /// <param name="left">Left side argument.</param>
        /// <param name="right">Right side argument.</param>
        /// <returns>If the right argument is greater than or equal to the left.</returns>
        public static bool operator <=(int left, CharacterTemplateID right)
        {
            return left <= right._value;
        }

        /// <summary>
        /// Implements operator >=.
        /// </summary>
        /// <param name="left">Left side argument.</param>
        /// <param name="right">Right side argument.</param>
        /// <returns>If the left argument is greater than or equal to the right.</returns>
        public static bool operator >=(CharacterTemplateID left, int right)
        {
            return left._value >= right;
        }

        /// <summary>
        /// Implements operator <=.
        /// </summary>
        /// <param name="left">Left side argument.</param>
        /// <param name="right">Right side argument.</param>
        /// <returns>If the right argument is greater than or equal to the left.</returns>
        public static bool operator <=(CharacterTemplateID left, int right)
        {
            return left._value <= right;
        }

        /// <summary>
        /// Implements operator >=.
        /// </summary>
        /// <param name="left">Left side argument.</param>
        /// <param name="right">Right side argument.</param>
        /// <returns>If the left argument is greater than or equal to the right.</returns>
        public static bool operator >=(CharacterTemplateID left, CharacterTemplateID right)
        {
            return left._value >= right._value;
        }

        /// <summary>
        /// Implements operator <=.
        /// </summary>
        /// <param name="left">Left side argument.</param>
        /// <param name="right">Right side argument.</param>
        /// <returns>If the right argument is greater than or equal to the left.</returns>
        public static bool operator <=(CharacterTemplateID left, CharacterTemplateID right)
        {
            return left._value <= right._value;
        }

        /// <summary>
        /// Implements operator !=.
        /// </summary>
        /// <param name="left">Left side argument.</param>
        /// <param name="right">Right side argument.</param>
        /// <returns>If the two arguments are not equal.</returns>
        public static bool operator !=(CharacterTemplateID left, CharacterTemplateID right)
        {
            return left._value != right._value;
        }

        /// <summary>
        /// Implements operator ==.
        /// </summary>
        /// <param name="left">Left side argument.</param>
        /// <param name="right">Right side argument.</param>
        /// <returns>If the two arguments are equal.</returns>
        public static bool operator ==(CharacterTemplateID left, CharacterTemplateID right)
        {
            return left._value == right._value;
        }
    }

    /// <summary>
    /// Adds extensions to some data I/O objects for performing Read and Write operations for the CharacterTemplateID.
    /// All of the operations are implemented in the CharacterTemplateID struct. These extensions are provided
    /// purely for the convenience of accessing all the I/O operations from the same place.
    /// </summary>
    public static class CharacterTemplateIDReadWriteExtensions
    {
        /// <summary>
        /// Gets the value in the <paramref name="dict"/> entry at the given <paramref name="key"/> as type CharacterTemplateID.
        /// </summary>
        /// <typeparam name="T">The key Type.</typeparam>
        /// <param name="dict">The IDictionary.</param>
        /// <param name="key">The key for the value to get.</param>
        /// <returns>The value at the given <paramref name="key"/> parsed as a CharacterTemplateID.</returns>
        public static CharacterTemplateID AsCharacterTemplateID<T>(this IDictionary<T, string> dict, T key)
        {
            return Parser.Invariant.ParseCharacterTemplateID(dict[key]);
        }

        /// <summary>
        /// Tries to get the value in the <paramref name="dict"/> entry at the given <paramref name="key"/> as type CharacterTemplateID.
        /// </summary>
        /// <typeparam name="T">The key Type.</typeparam>
        /// <param name="dict">The IDictionary.</param>
        /// <param name="key">The key for the value to get.</param>
        /// <param name="defaultValue">The value to use if the value at the <paramref name="key"/> could not be parsed.</param>
        /// <returns>The value at the given <paramref name="key"/> parsed as an int, or the
        /// <paramref name="defaultValue"/> if the <paramref name="key"/> did not exist in the <paramref name="dict"/>
        /// or the value at the given <paramref name="key"/> could not be parsed.</returns>
        public static CharacterTemplateID AsCharacterTemplateID<T>(this IDictionary<T, string> dict, T key,
                                                                   CharacterTemplateID defaultValue)
        {
            string value;
            if (!dict.TryGetValue(key, out value))
                return defaultValue;

            CharacterTemplateID parsed;
            if (!Parser.Invariant.TryParse(value, out parsed))
                return defaultValue;

            return parsed;
        }

        /// <summary>
        /// Reads the CharacterTemplateID from an IDataReader.
        /// </summary>
        /// <param name="dataReader">IDataReader to read the CharacterTemplateID from.</param>
        /// <param name="i">The field index to read.</param>
        /// <returns>The CharacterTemplateID read from the IDataReader.</returns>
        public static CharacterTemplateID GetCharacterTemplateID(this IDataReader dataReader, int i)
        {
            return CharacterTemplateID.Read(dataReader, i);
        }

        /// <summary>
        /// Reads the CharacterTemplateID from an IDataReader.
        /// </summary>
        /// <param name="dataReader">IDataReader to read the CharacterTemplateID from.</param>
        /// <param name="name">The name of the field to read the value from.</param>
        /// <returns>The CharacterTemplateID read from the IDataReader.</returns>
        public static CharacterTemplateID GetCharacterTemplateID(this IDataReader dataReader, string name)
        {
            return CharacterTemplateID.Read(dataReader, name);
        }

        /// <summary>
        /// Parses the CharacterTemplateID from a string.
        /// </summary>
        /// <param name="parser">The Parser to use.</param>
        /// <param name="value">The string to parse.</param>
        /// <returns>The CharacterTemplateID parsed from the string.</returns>
        public static CharacterTemplateID ParseCharacterTemplateID(this Parser parser, string value)
        {
            return new CharacterTemplateID(parser.ParseInt(value));
        }

        /// <summary>
        /// Reads the CharacterTemplateID from a BitStream.
        /// </summary>
        /// <param name="bitStream">BitStream to read the CharacterTemplateID from.</param>
        /// <returns>The CharacterTemplateID read from the BitStream.</returns>
        public static CharacterTemplateID ReadCharacterTemplateID(this BitStream bitStream)
        {
            return CharacterTemplateID.Read(bitStream);
        }

        /// <summary>
        /// Reads the CharacterTemplateID from an IValueReader.
        /// </summary>
        /// <param name="valueReader">IValueReader to read the CharacterTemplateID from.</param>
        /// <param name="name">The unique name of the value to read.</param>
        /// <returns>The CharacterTemplateID read from the IValueReader.</returns>
        public static CharacterTemplateID ReadCharacterTemplateID(this IValueReader valueReader, string name)
        {
            return CharacterTemplateID.Read(valueReader, name);
        }

        /// <summary>
        /// Tries to parse the CharacterTemplateID from a string.
        /// </summary>
        /// <param name="parser">The Parser to use.</param>
        /// <param name="value">The string to parse.</param>
        /// <param name="outValue">If this method returns true, contains the parsed CharacterTemplateID.</param>
        /// <returns>True if the parsing was successfully; otherwise false.</returns>
        public static bool TryParse(this Parser parser, string value, out CharacterTemplateID outValue)
        {
            int tmp;
            var ret = parser.TryParse(value, out tmp);
            outValue = new CharacterTemplateID(tmp);
            return ret;
        }

        /// <summary>
        /// Writes a CharacterTemplateID to a BitStream.
        /// </summary>
        /// <param name="bitStream">BitStream to write to.</param>
        /// <param name="value">CharacterTemplateID to write.</param>
        public static void Write(this BitStream bitStream, CharacterTemplateID value)
        {
            value.Write(bitStream);
        }

        /// <summary>
        /// Writes a CharacterTemplateID to a IValueWriter.
        /// </summary>
        /// <param name="valueWriter">IValueWriter to write to.</param>
        /// <param name="name">Unique name of the CharacterTemplateID that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">CharacterTemplateID to write.</param>
        public static void Write(this IValueWriter valueWriter, string name, CharacterTemplateID value)
        {
            value.Write(valueWriter, name);
        }

        #region Non-Templated Code

        /// <summary>
        /// Checks if the CharacterTemplate with the given CharacterTemplateID exists in the database.
        /// </summary>
        /// <param name="id">CharacterTemplateID to check.</param>
        /// <returns>True if a CharacterTemplate with the given id exists; otherwise false.</returns>
        public static bool TemplateExists(this CharacterTemplateID id)
        {
            IDbController dbController = DbControllerBase.GetInstance();
            var query = dbController.GetQuery<SelectCharacterTemplateQuery>();

            try
            {
                var result = query.Execute(id);
                if (result == null)
                    return false;
            }
            catch (ArgumentException)
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}