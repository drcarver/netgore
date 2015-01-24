using System;
using System.Collections.Generic;
using System.Linq;
using DemoGame.DbObjs;

namespace DemoGame.Server.DbObjs
{
    /// <summary>
    /// Provides a strongly-typed structure for the database table `character_template_inventory`.
    /// </summary>
    public class CharacterTemplateInventoryTable : ICharacterTemplateInventoryTable
    {
        /// <summary>
        /// The number of columns in the database table that this class represents.
        /// </summary>
        public const Int32 ColumnCount = 6;

        /// <summary>
        /// The name of the database table that this class represents.
        /// </summary>
        public const String TableName = "character_template_inventory";

        /// <summary>
        /// Array of the database column names.
        /// </summary>
        static readonly String[] _dbColumns = new string[]
        { "chance", "character_template_id", "id", "item_template_id", "max", "min" };

        /// <summary>
        /// Array of the database column names for columns that are primary keys.
        /// </summary>
        static readonly String[] _dbColumnsKeys = new string[] { "id" };

        /// <summary>
        /// Array of the database column names for columns that are not primary keys.
        /// </summary>
        static readonly String[] _dbColumnsNonKey = new string[]
        { "chance", "character_template_id", "item_template_id", "max", "min" };

        /// <summary>
        /// The field that maps onto the database column `chance`.
        /// </summary>
        UInt16 _chance;

        /// <summary>
        /// The field that maps onto the database column `character_template_id`.
        /// </summary>
        UInt16 _characterTemplateID;

        /// <summary>
        /// The field that maps onto the database column `id`.
        /// </summary>
        Int32 _iD;

        /// <summary>
        /// The field that maps onto the database column `item_template_id`.
        /// </summary>
        UInt16 _itemTemplateID;

        /// <summary>
        /// The field that maps onto the database column `max`.
        /// </summary>
        Byte _max;

        /// <summary>
        /// The field that maps onto the database column `min`.
        /// </summary>
        Byte _min;

        /// <summary>
        /// CharacterTemplateInventoryTable constructor.
        /// </summary>
        public CharacterTemplateInventoryTable()
        {
        }

        /// <summary>
        /// CharacterTemplateInventoryTable constructor.
        /// </summary>
        /// <param name="chance">The initial value for the corresponding property.</param>
        /// <param name="characterTemplateID">The initial value for the corresponding property.</param>
        /// <param name="iD">The initial value for the corresponding property.</param>
        /// <param name="itemTemplateID">The initial value for the corresponding property.</param>
        /// <param name="max">The initial value for the corresponding property.</param>
        /// <param name="min">The initial value for the corresponding property.</param>
        public CharacterTemplateInventoryTable(ItemChance @chance, CharacterTemplateID @characterTemplateID, Int32 @iD,
                                               ItemTemplateID @itemTemplateID, Byte @max, Byte @min)
        {
            Chance = @chance;
            CharacterTemplateID = @characterTemplateID;
            ID = @iD;
            ItemTemplateID = @itemTemplateID;
            Max = @max;
            Min = @min;
        }

        /// <summary>
        /// CharacterTemplateInventoryTable constructor.
        /// </summary>
        /// <param name="source">ICharacterTemplateInventoryTable to copy the initial values from.</param>
        public CharacterTemplateInventoryTable(ICharacterTemplateInventoryTable source)
        {
            CopyValuesFrom(source);
        }

        /// <summary>
        /// Gets an IEnumerable of strings containing the names of the database columns for the table that this class represents.
        /// </summary>
        public static IEnumerable<String> DbColumns
        {
            get { return _dbColumns; }
        }

        /// <summary>
        /// Gets an IEnumerable of strings containing the names of the database columns that are primary keys.
        /// </summary>
        public static IEnumerable<String> DbKeyColumns
        {
            get { return _dbColumnsKeys; }
        }

        /// <summary>
        /// Gets an IEnumerable of strings containing the names of the database columns that are not primary keys.
        /// </summary>
        public static IEnumerable<String> DbNonKeyColumns
        {
            get { return _dbColumnsNonKey; }
        }

        /// <summary>
        /// Copies the column values into the given Dictionary using the database column name
        /// with a prefixed @ as the key. The keys must already exist in the Dictionary;
        /// this method will not create them if they are missing.
        /// </summary>
        /// <param name="source">The object to copy the values from.</param>
        /// <param name="dic">The Dictionary to copy the values into.</param>
        public static void CopyValues(ICharacterTemplateInventoryTable source, IDictionary<String, Object> dic)
        {
            dic["@chance"] = source.Chance;
            dic["@character_template_id"] = source.CharacterTemplateID;
            dic["@id"] = source.ID;
            dic["@item_template_id"] = source.ItemTemplateID;
            dic["@max"] = source.Max;
            dic["@min"] = source.Min;
        }

        /// <summary>
        /// Copies the column values into the given Dictionary using the database column name
        /// with a prefixed @ as the key. The keys must already exist in the Dictionary;
        /// this method will not create them if they are missing.
        /// </summary>
        /// <param name="dic">The Dictionary to copy the values into.</param>
        public void CopyValues(IDictionary<String, Object> dic)
        {
            CopyValues(this, dic);
        }

        /// <summary>
        /// Copies the values from the given <paramref name="source"/> into this CharacterTemplateInventoryTable.
        /// </summary>
        /// <param name="source">The ICharacterTemplateInventoryTable to copy the values from.</param>
        public void CopyValuesFrom(ICharacterTemplateInventoryTable source)
        {
            Chance = source.Chance;
            CharacterTemplateID = source.CharacterTemplateID;
            ID = source.ID;
            ItemTemplateID = source.ItemTemplateID;
            Max = source.Max;
            Min = source.Min;
        }

        /// <summary>
        /// Gets the data for the database column that this table represents.
        /// </summary>
        /// <param name="columnName">The database name of the column to get the data for.</param>
        /// <returns>
        /// The data for the database column with the name <paramref name="columnName"/>.
        /// </returns>
        public static ColumnMetadata GetColumnData(String columnName)
        {
            switch (columnName)
            {
                case "chance":
                    return new ColumnMetadata("chance", "", "smallint(5) unsigned", null, typeof(UInt16), false, false, false);

                case "character_template_id":
                    return new ColumnMetadata("character_template_id", "", "smallint(5) unsigned", null, typeof(UInt16), false,
                                              false, true);

                case "id":
                    return new ColumnMetadata("id", "", "int(11)", null, typeof(Int32), false, true, false);

                case "item_template_id":
                    return new ColumnMetadata("item_template_id", "", "smallint(5) unsigned", null, typeof(UInt16), false, false,
                                              true);

                case "max":
                    return new ColumnMetadata("max", "", "tinyint(3) unsigned", null, typeof(Byte), false, false, false);

                case "min":
                    return new ColumnMetadata("min", "", "tinyint(3) unsigned", null, typeof(Byte), false, false, false);

                default:
                    throw new ArgumentException("Field not found.", "columnName");
            }
        }

        /// <summary>
        /// Gets the value of a column by the database column's name.
        /// </summary>
        /// <param name="columnName">The database name of the column to get the value for.</param>
        /// <returns>
        /// The value of the column with the name <paramref name="columnName"/>.
        /// </returns>
        public Object GetValue(String columnName)
        {
            switch (columnName)
            {
                case "chance":
                    return Chance;

                case "character_template_id":
                    return CharacterTemplateID;

                case "id":
                    return ID;

                case "item_template_id":
                    return ItemTemplateID;

                case "max":
                    return Max;

                case "min":
                    return Min;

                default:
                    throw new ArgumentException("Field not found.", "columnName");
            }
        }

        /// <summary>
        /// Sets the <paramref name="value"/> of a column by the database column's name.
        /// </summary>
        /// <param name="columnName">The database name of the column to get the <paramref name="value"/> for.</param>
        /// <param name="value">Value to assign to the column.</param>
        public void SetValue(String columnName, Object value)
        {
            switch (columnName)
            {
                case "chance":
                    Chance = (ItemChance)value;
                    break;

                case "character_template_id":
                    CharacterTemplateID = (CharacterTemplateID)value;
                    break;

                case "id":
                    ID = (Int32)value;
                    break;

                case "item_template_id":
                    ItemTemplateID = (ItemTemplateID)value;
                    break;

                case "max":
                    Max = (Byte)value;
                    break;

                case "min":
                    Min = (Byte)value;
                    break;

                default:
                    throw new ArgumentException("Field not found.", "columnName");
            }
        }

        #region ICharacterTemplateInventoryTable Members

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `chance`.
        /// The underlying database type is `smallint(5) unsigned`.
        /// </summary>
        public ItemChance Chance
        {
            get { return (ItemChance)_chance; }
            set { _chance = (UInt16)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `character_template_id`.
        /// The underlying database type is `smallint(5) unsigned`.
        /// </summary>
        public CharacterTemplateID CharacterTemplateID
        {
            get { return (CharacterTemplateID)_characterTemplateID; }
            set { _characterTemplateID = (UInt16)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `id`.
        /// The underlying database type is `int(11)`.
        /// </summary>
        public Int32 ID
        {
            get { return _iD; }
            set { _iD = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `item_template_id`.
        /// The underlying database type is `smallint(5) unsigned`.
        /// </summary>
        public ItemTemplateID ItemTemplateID
        {
            get { return (ItemTemplateID)_itemTemplateID; }
            set { _itemTemplateID = (UInt16)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `max`.
        /// The underlying database type is `tinyint(3) unsigned`.
        /// </summary>
        public Byte Max
        {
            get { return _max; }
            set { _max = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `min`.
        /// The underlying database type is `tinyint(3) unsigned`.
        /// </summary>
        public Byte Min
        {
            get { return _min; }
            set { _min = value; }
        }

        /// <summary>
        /// Creates a deep copy of this table. All the values will be the same
        /// but they will be contained in a different object instance.
        /// </summary>
        /// <returns>
        /// A deep copy of this table.
        /// </returns>
        public ICharacterTemplateInventoryTable DeepCopy()
        {
            return new CharacterTemplateInventoryTable(this);
        }

        #endregion
    }
}