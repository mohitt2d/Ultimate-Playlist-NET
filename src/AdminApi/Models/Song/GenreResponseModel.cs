#region Usings

using System;

#endregion

namespace UltimatePlaylist.AdminApi.Models.Song
{
    public class GenreResponseModel
    {
        public Guid ExternalId { get; set; }

        public string Name { get; set; }
    }
}
