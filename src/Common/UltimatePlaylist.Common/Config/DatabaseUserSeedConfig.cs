#region Usings

using System.Collections.Generic;

#endregion

namespace UltimatePlaylist.Common.Config
{
    public class DatabaseUserSeedConfig
    {
        public DatabaseUserSeedConfig()
        {
            FixedAccounts = new List<DatabaseSeedUserFixedAccountConfig>();
        }

        public IList<DatabaseSeedUserFixedAccountConfig> FixedAccounts { get; set; }
    }
}
