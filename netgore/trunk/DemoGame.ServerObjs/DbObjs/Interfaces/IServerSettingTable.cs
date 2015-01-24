using System;
using System.Linq;

namespace DemoGame.Server.DbObjs
{
    /// <summary>
    /// Interface for a class that can be used to serialize values to the database table `server_setting`.
    /// </summary>
    public interface IServerSettingTable
    {
        /// <summary>
        /// Gets the value of the database column `motd`.
        /// </summary>
        String Motd { get; }

        /// <summary>
        /// Creates a deep copy of this table. All the values will be the same
        /// but they will be contained in a different object instance.
        /// </summary>
        /// <returns>
        /// A deep copy of this table.
        /// </returns>
        IServerSettingTable DeepCopy();
    }
}