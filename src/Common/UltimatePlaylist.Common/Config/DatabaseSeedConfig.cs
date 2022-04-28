#region Usings

using System.Collections.Generic;

#endregion

namespace UltimatePlaylist.Common.Config
{
    public class DatabaseSeedConfig
    {
        public DatabaseUserSeedConfig Admins { get; set; }

        public DatabaseUserSeedConfig Users { get; set; }

        public IList<DatabaseGenderSeedConfig> Genders { get; set; }

        public IList<string> MusicGenres { get; set; }

        public List<DatabaseSongSeedConfig> Songs { get; set; }
    }
}
