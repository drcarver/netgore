using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MySql.Data.MySqlClient;

namespace InstallationValidator.SchemaChecker
{
    [Serializable]
    public class ColumnSchema
    {
        /// <summary>
        /// All the schema value names possible for the <see cref="ColumnSchema"/>.
        /// </summary>
        static readonly string[] _valueNames = new string[]
        {
            "TABLE_NAME", "COLUMN_NAME", "DATA_TYPE", "CHARACTER_MAXIMUM_LENGTH", "CHARACTER_OCTET_LENGTH", "NUMERIC_PRECISION",
            "NUMERIC_SCALE", "COLUMN_TYPE", "CHARACTER_SET_NAME", "COLUMN_KEY", "EXTRA", "COLUMN_DEFAULT", "IS_NULLABLE"
        };

        /// <summary>
        /// Gets the name of the column.
        /// </summary>
        public string Name
        {
            get { return this["COLUMN_NAME"]; }
        }

        /// <summary>
        /// Gets the name of the table this column is on.
        /// </summary>
        public string TableName
        {
            get { return this["TABLE_NAME"]; }
        }

        /// <summary>
        /// Checks if the values of this <see cref="ColumnSchema"/> are equal to the values in the
        /// <paramref name="other"/>.
        /// </summary>
        /// <param name="other">The other <see cref="ColumnSchema"/>.</param>
        /// <returns>True if all values are equal; otherwise false.</returns>
        public bool EqualValues(ColumnSchema other)
        {
            foreach (var kvp in _values)
            {
                var otherValue = other[kvp.Key];
                if (otherValue != kvp.Value)
                    return false;
            }

            return true;
        }

        readonly IDictionary<string, string> _values;

        /// <summary>
        /// Gets an IEnumerable of all the schema value names possible for the <see cref="ColumnSchema"/>.
        /// </summary>
        public static IEnumerable<string> ValueNames
        {
            get { return _valueNames; }
        }

        public string this[string key]
        {
            get { return _values[key]; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnSchema"/> class.
        /// </summary>
        /// <param name="values">The values.</param>
        public ColumnSchema(IDictionary<string, string> values)
        {
            _values = values;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnSchema"/> class.
        /// </summary>
        /// <param name="r">The <see cref="IDataReader"/> to read the values from.</param>
        public ColumnSchema(IDataReader r)
        {
            _values = ReadValues(r);
        }

        public static IDictionary<string, string> ReadValues(IDataReader r)
        {
            Dictionary<string, string> d = new Dictionary<string, string>(_valueNames.Length);
            foreach (var v in ValueNames)
            {
                int ordinal = r.GetOrdinal(v);
                if (r.IsDBNull(ordinal))
                    d.Add(v, null);
                else
                    d.Add(v, r.GetString(ordinal));
            }

            return d;
        }
    }
}