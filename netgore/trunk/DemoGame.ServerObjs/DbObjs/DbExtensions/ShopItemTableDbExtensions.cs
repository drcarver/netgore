using System;
using System.Data;
using System.Linq;
using NetGore.Db;

namespace DemoGame.Server.DbObjs
{
    /// <summary>
    /// Contains extension methods for class ShopItemTable that assist in performing
    /// reads and writes to and from a database.
    /// </summary>
    public static class ShopItemTableDbExtensions
    {
        /// <summary>
        /// Copies the column values into the given DbParameterValues using the database column name
        /// with a prefixed @ as the key. The keys must already exist in the DbParameterValues;
        ///  this method will not create them if they are missing.
        /// </summary>
        /// <param name="source">The object to copy the values from.</param>
        /// <param name="paramValues">The DbParameterValues to copy the values into.</param>
        public static void CopyValues(this IShopItemTable source, DbParameterValues paramValues)
        {
            paramValues["@item_template_id"] = (UInt16)source.ItemTemplateID;
            paramValues["@shop_id"] = (UInt16)source.ShopID;
        }

        /// <summary>
        /// Reads the values from an IDataReader and assigns the read values to this
        /// object's properties. The database column's name is used to as the key, so the value
        /// will not be found if any aliases are used or not all columns were selected.
        /// </summary>
        /// <param name="source">The object to add the extension method to.</param>
        /// <param name="dataReader">The IDataReader to read the values from. Must already be ready to be read from.</param>
        public static void ReadValues(this ShopItemTable source, IDataReader dataReader)
        {
            Int32 i;

            i = dataReader.GetOrdinal("item_template_id");
            source.ItemTemplateID = (ItemTemplateID)dataReader.GetUInt16(i);

            i = dataReader.GetOrdinal("shop_id");
            source.ShopID = (ShopID)dataReader.GetUInt16(i);
        }

        /// <summary>
        /// Copies the column values into the given DbParameterValues using the database column name
        /// with a prefixed @ as the key. The key must already exist in the DbParameterValues
        /// for the value to be copied over. If any of the keys in the DbParameterValues do not
        /// match one of the column names, or if there is no field for a key, then it will be
        /// ignored. Because of this, it is important to be careful when using this method
        /// since columns or keys can be skipped without any indication.
        /// </summary>
        /// <param name="source">The object to copy the values from.</param>
        /// <param name="paramValues">The DbParameterValues to copy the values into.</param>
        public static void TryCopyValues(this IShopItemTable source, DbParameterValues paramValues)
        {
            for (int i = 0; i < paramValues.Count; i++)
            {
                switch (paramValues.GetParameterName(i))
                {
                    case "@item_template_id":
                        paramValues[i] = (UInt16)source.ItemTemplateID;
                        break;

                    case "@shop_id":
                        paramValues[i] = (UInt16)source.ShopID;
                        break;
                }
            }
        }

        /// <summary>
        /// Reads the values from an IDataReader and assigns the read values to this
        /// object's properties. Unlike ReadValues(), this method not only doesn't require
        /// all values to be in the IDataReader, but also does not require the values in
        /// the IDataReader to be a defined field for the table this class represents.
        /// Because of this, you need to be careful when using this method because values
        /// can easily be skipped without any indication.
        /// </summary>
        /// <param name="source">The object to add the extension method to.</param>
        /// <param name="dataReader">The IDataReader to read the values from. Must already be ready to be read from.</param>
        public static void TryReadValues(this ShopItemTable source, IDataReader dataReader)
        {
            for (int i = 0; i < dataReader.FieldCount; i++)
            {
                switch (dataReader.GetName(i))
                {
                    case "item_template_id":
                        source.ItemTemplateID = (ItemTemplateID)dataReader.GetUInt16(i);
                        break;

                    case "shop_id":
                        source.ShopID = (ShopID)dataReader.GetUInt16(i);
                        break;
                }
            }
        }
    }
}