using System;
using System.Collections.Generic;
using System.Linq;
using DemoGame.DbObjs;

namespace DemoGame.Server.DbObjs
{
    /// <summary>
    /// Provides a strongly-typed structure for the database table `account`.
    /// </summary>
    public class AccountTable : IAccountTable
    {
        /// <summary>
        /// The number of columns in the database table that this class represents.
        /// </summary>
        public const Int32 ColumnCount = 7;

        /// <summary>
        /// The name of the database table that this class represents.
        /// </summary>
        public const String TableName = "account";

        /// <summary>
        /// Array of the database column names.
        /// </summary>
        static readonly String[] _dbColumns = new string[]
        { "current_ip", "email", "id", "name", "password", "time_created", "time_last_login" };

        /// <summary>
        /// Array of the database column names for columns that are primary keys.
        /// </summary>
        static readonly String[] _dbColumnsKeys = new string[] { "id" };

        /// <summary>
        /// Array of the database column names for columns that are not primary keys.
        /// </summary>
        static readonly String[] _dbColumnsNonKey = new string[]
        { "current_ip", "email", "name", "password", "time_created", "time_last_login" };

        /// <summary>
        /// The field that maps onto the database column `current_ip`.
        /// </summary>
        uint? _currentIp;

        /// <summary>
        /// The field that maps onto the database column `email`.
        /// </summary>
        String _email;

        /// <summary>
        /// The field that maps onto the database column `id`.
        /// </summary>
        Int32 _iD;

        /// <summary>
        /// The field that maps onto the database column `name`.
        /// </summary>
        String _name;

        /// <summary>
        /// The field that maps onto the database column `password`.
        /// </summary>
        String _password;

        /// <summary>
        /// The field that maps onto the database column `time_created`.
        /// </summary>
        DateTime _timeCreated;

        /// <summary>
        /// The field that maps onto the database column `time_last_login`.
        /// </summary>
        DateTime _timeLastLogin;

        /// <summary>
        /// AccountTable constructor.
        /// </summary>
        public AccountTable()
        {
        }

        /// <summary>
        /// AccountTable constructor.
        /// </summary>
        /// <param name="currentIp">The initial value for the corresponding property.</param>
        /// <param name="email">The initial value for the corresponding property.</param>
        /// <param name="iD">The initial value for the corresponding property.</param>
        /// <param name="name">The initial value for the corresponding property.</param>
        /// <param name="password">The initial value for the corresponding property.</param>
        /// <param name="timeCreated">The initial value for the corresponding property.</param>
        /// <param name="timeLastLogin">The initial value for the corresponding property.</param>
        public AccountTable(uint? @currentIp, String @email, AccountID @iD, String @name, String @password, DateTime @timeCreated,
                            DateTime @timeLastLogin)
        {
            CurrentIp = @currentIp;
            Email = @email;
            ID = @iD;
            Name = @name;
            Password = @password;
            TimeCreated = @timeCreated;
            TimeLastLogin = @timeLastLogin;
        }

        /// <summary>
        /// AccountTable constructor.
        /// </summary>
        /// <param name="source">IAccountTable to copy the initial values from.</param>
        public AccountTable(IAccountTable source)
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
        public static void CopyValues(IAccountTable source, IDictionary<String, Object> dic)
        {
            dic["@current_ip"] = source.CurrentIp;
            dic["@email"] = source.Email;
            dic["@id"] = source.ID;
            dic["@name"] = source.Name;
            dic["@password"] = source.Password;
            dic["@time_created"] = source.TimeCreated;
            dic["@time_last_login"] = source.TimeLastLogin;
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
        /// Copies the values from the given <paramref name="source"/> into this AccountTable.
        /// </summary>
        /// <param name="source">The IAccountTable to copy the values from.</param>
        public void CopyValuesFrom(IAccountTable source)
        {
            CurrentIp = source.CurrentIp;
            Email = source.Email;
            ID = source.ID;
            Name = source.Name;
            Password = source.Password;
            TimeCreated = source.TimeCreated;
            TimeLastLogin = source.TimeLastLogin;
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
                case "current_ip":
                    return new ColumnMetadata("current_ip",
                                              "IP address currently logged in to the account, or null if nobody is logged in.",
                                              "int(10) unsigned", null, typeof(uint?), true, false, false);

                case "email":
                    return new ColumnMetadata("email", "The email address.", "varchar(60)", null, typeof(String), false, false,
                                              false);

                case "id":
                    return new ColumnMetadata("id", "The account ID.", "int(11)", null, typeof(Int32), false, true, false);

                case "name":
                    return new ColumnMetadata("name", "The account name.", "varchar(30)", null, typeof(String), false, false, true);

                case "password":
                    return new ColumnMetadata("password", "The account password.", "varchar(30)", null, typeof(String), false,
                                              false, false);

                case "time_created":
                    return new ColumnMetadata("time_created", "The DateTime of when the account was created.", "datetime", null,
                                              typeof(DateTime), false, false, false);

                case "time_last_login":
                    return new ColumnMetadata("time_last_login", "The DateTime that the account was last logged in to.",
                                              "datetime", null, typeof(DateTime), false, false, false);

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
                case "current_ip":
                    return CurrentIp;

                case "email":
                    return Email;

                case "id":
                    return ID;

                case "name":
                    return Name;

                case "password":
                    return Password;

                case "time_created":
                    return TimeCreated;

                case "time_last_login":
                    return TimeLastLogin;

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
                case "current_ip":
                    CurrentIp = (uint?)value;
                    break;

                case "email":
                    Email = (String)value;
                    break;

                case "id":
                    ID = (AccountID)value;
                    break;

                case "name":
                    Name = (String)value;
                    break;

                case "password":
                    Password = (String)value;
                    break;

                case "time_created":
                    TimeCreated = (DateTime)value;
                    break;

                case "time_last_login":
                    TimeLastLogin = (DateTime)value;
                    break;

                default:
                    throw new ArgumentException("Field not found.", "columnName");
            }
        }

        #region IAccountTable Members

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `current_ip`.
        /// The underlying database type is `int(10) unsigned`. The database column contains the comment: 
        /// "IP address currently logged in to the account, or null if nobody is logged in.".
        /// </summary>
        public uint? CurrentIp
        {
            get { return _currentIp; }
            set { _currentIp = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `email`.
        /// The underlying database type is `varchar(60)`. The database column contains the comment: 
        /// "The email address.".
        /// </summary>
        public String Email
        {
            get { return _email; }
            set { _email = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `id`.
        /// The underlying database type is `int(11)`. The database column contains the comment: 
        /// "The account ID.".
        /// </summary>
        public AccountID ID
        {
            get { return (AccountID)_iD; }
            set { _iD = (Int32)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `name`.
        /// The underlying database type is `varchar(30)`. The database column contains the comment: 
        /// "The account name.".
        /// </summary>
        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `password`.
        /// The underlying database type is `varchar(30)`. The database column contains the comment: 
        /// "The account password.".
        /// </summary>
        public String Password
        {
            get { return _password; }
            set { _password = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `time_created`.
        /// The underlying database type is `datetime`. The database column contains the comment: 
        /// "The DateTime of when the account was created.".
        /// </summary>
        public DateTime TimeCreated
        {
            get { return _timeCreated; }
            set { _timeCreated = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `time_last_login`.
        /// The underlying database type is `datetime`. The database column contains the comment: 
        /// "The DateTime that the account was last logged in to.".
        /// </summary>
        public DateTime TimeLastLogin
        {
            get { return _timeLastLogin; }
            set { _timeLastLogin = value; }
        }

        /// <summary>
        /// Creates a deep copy of this table. All the values will be the same
        /// but they will be contained in a different object instance.
        /// </summary>
        /// <returns>
        /// A deep copy of this table.
        /// </returns>
        public IAccountTable DeepCopy()
        {
            return new AccountTable(this);
        }

        #endregion
    }
}