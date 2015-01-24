using System;
using System.Collections.Generic;
using System.Linq;

namespace NetGore.Db.ClassCreator
{
    /// <summary>
    /// Provides the mapping for the custom Type to use for a column or collection of columns.
    /// </summary>
    public class CustomTypeMapping
    {
        /// <summary>
        /// The columns to use this mapping on.
        /// </summary>
        public readonly IEnumerable<string> Columns;

        /// <summary>
        /// The custom type to expose.
        /// </summary>
        public readonly string CustomType;

        /// <summary>
        /// The tables to use this mapping on.
        /// </summary>
        public readonly IEnumerable<string> Tables;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomTypeMapping"/> class.
        /// </summary>
        /// <param name="tables">The tables.</param>
        /// <param name="columns">The columns.</param>
        /// <param name="customType">Type of the custom.</param>
        public CustomTypeMapping(IEnumerable<string> tables, IEnumerable<string> columns, string customType)
        {
            Tables = tables;
            Columns = columns.ToCompact();
            CustomType = customType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomTypeMapping"/> class.
        /// </summary>
        /// <param name="tables">The tables.</param>
        /// <param name="columns">The columns.</param>
        /// <param name="customType">Type of the custom.</param>
        /// <param name="formatter">The formatter.</param>
        public CustomTypeMapping(IEnumerable<string> tables, IEnumerable<string> columns, Type customType, CodeFormatter formatter)
            : this(tables, columns, formatter.GetTypeString(customType))
        {
        }
    }
}