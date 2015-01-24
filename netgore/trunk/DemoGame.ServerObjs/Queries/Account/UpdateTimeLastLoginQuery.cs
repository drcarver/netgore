using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries.Account
{
    [DbControllerQuery]
    public class UpdateTimeLastLoginQuery : DbQueryNonReader<AccountID>
    {
        static readonly string _queryStr = string.Format("UPDATE `{0}` SET `time_last_login` = NOW() WHERE `id`=@id",
                                                         AccountTable.TableName);

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateTimeLastLoginQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public UpdateTimeLastLoginQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryStr)
        {
            QueryAsserts.ContainsColumns(AccountTable.DbColumns, "id", "time_last_login");
        }

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>IEnumerable of all the <see cref="DbParameter"/>s needed for this class to perform database queries.
        /// If null, no parameters will be used.</returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("@id");
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters values <paramref name="p"/>
        /// based on the values specified in the given <paramref name="item"/> parameter.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="item">The value or object/struct containing the values used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, AccountID item)
        {
            p["@id"] = (int)item;
        }
    }
}