#region Usings

using System.Runtime.Serialization;

#endregion

namespace UltimatePlaylist.Services.Common.AppleMusic.Enums
{
    public enum AppleMusicResurceType
    {
        [EnumMember(Value = "activities")]
        Activities,
        [EnumMember(Value = "artists")]
        Artists,
        [EnumMember(Value = "albums")]
        Albums,
        [EnumMember(Value = "apple-curators")]
        AppleCurators,
        [EnumMember(Value = "curators")]
        Curators,
        [EnumMember(Value = "genres")]
        Genres,
        [EnumMember(Value = "playlists")]
        Playlists,
        [EnumMember(Value = "songs")]
        Songs,
        [EnumMember(Value = "stations")]
        Stations,
        [EnumMember(Value = "music-videos")]
        MusicVideos,
    }
}
