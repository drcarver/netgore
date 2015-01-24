using System.Data;
using System.Data.Common;
using System.Linq;
using NetGore.Db;
using NUnit.Framework;

namespace NetGore.Tests.Db.MySql
{
    [TestFixture]
    public class MySqlDbConnectionPoolTests
    {
        [Test]
        public void ConnectionCloseTest()
        {
            DbConnectionPool pool = DbManagerTestSettings.CreateConnectionPool();

            IDbConnection conn;
            using (PooledDbConnection connPool = pool.Create())
            {
                conn = connPool.Connection;
            }
            Assert.AreEqual(ConnectionState.Closed, conn.State);
        }

        [Test]
        public void ConnectionOpenTest()
        {
            DbConnectionPool pool = DbManagerTestSettings.CreateConnectionPool();

            using (PooledDbConnection connPool = pool.Create())
            {
                DbConnection conn = connPool.Connection;
                Assert.IsNotNull(conn);
                Assert.AreEqual(ConnectionState.Open, conn.State);
            }
        }

        [Test]
        public void MultiplePoolItemsTest()
        {
            DbConnectionPool pool = DbManagerTestSettings.CreateConnectionPool();

            Assert.AreEqual(0, pool.Count);
            using (PooledDbConnection a = pool.Create())
            {
                Assert.AreEqual(1, pool.Count);
                using (PooledDbConnection b = pool.Create())
                {
                    Assert.AreEqual(2, pool.Count);
                    using (PooledDbConnection c = pool.Create())
                    {
                        Assert.AreEqual(3, pool.Count);
                        Assert.IsNotNull(a);
                        Assert.IsNotNull(b);
                        Assert.IsNotNull(c);
                    }
                    Assert.AreEqual(2, pool.Count);
                }
                Assert.AreEqual(1, pool.Count);
            }
            Assert.AreEqual(0, pool.Count);
        }

        [Test]
        public void SelectQueryTest()
        {
            DbConnectionPool pool = DbManagerTestSettings.CreateConnectionPool();

            using (PooledDbConnection connPool = pool.Create())
            {
                using (DbCommand cmd = connPool.Connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT 100 + 500";
                    using (DbDataReader r = cmd.ExecuteReader())
                    {
                        r.Read();
                        Assert.AreEqual(600, r[0]);
                    }
                }
            }
        }
    }
}