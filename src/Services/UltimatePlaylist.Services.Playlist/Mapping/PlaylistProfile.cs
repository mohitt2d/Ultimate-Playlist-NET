#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using UltimatePlaylist.Database.Infrastructure.Entities.Playlist;
using UltimatePlaylist.Database.Infrastructure.Entities.Song;
using UltimatePlaylist.Services.Common.Models.Playlist;
using UltimatePlaylist.Services.Common.Models.Song;

#endregion

namespace UltimatePlaylist.Services.Playlist.Mapping
{
    public class PlaylistProfile : Profile
    {
        #region Constructor(s)

        public PlaylistProfile()
        {
            CreateMap<PlaylistEntity, PlaylistReadServiceModel>()
                .ForMember(m => m.Songs, opt => opt.MapFrom(m => m.PlaylistSongs.Select(s => s.Song)));

            CreateMap<UserPlaylistEntity, PlaylistReadServiceModel>()
                .ForMember(m => m.Songs, opt => opt.MapFrom(m => m.UserPlaylistSongs.Select(s => s.Song)));

            CreateMap<SongEntity, UserSongReadServiceModel>()
                .ForMember(m => m.AudioFileStreamingUrl, opt => opt.MapFrom(m => m.AudioFile.StreamingUrl))
                .ForMember(m => m.AudioFileStreamingUrl, opt => opt.MapFrom(m => m.AudioFile.StreamingUrl))
                .ForMember(m => m.IsAddedToAppleMusic, opt => opt.Ignore())
                .ForMember(m => m.IsAddedToSpotify, opt => opt.Ignore())
                .ForMember(m => m.UserRating, opt => opt.Ignore());

            CreateMap<PlaylistEntity, AdminPlaylistReadServiceModel>()
                .ForMember(m => m.Songs, opt => opt.MapFrom(m => m.PlaylistSongs.Select(s => s.Song)));

            CreateMap<UserPlaylistEntity, PlaylistHistoryReadServiceModel>()
                .IncludeBase<UserPlaylistEntity, PlaylistReadServiceModel>()
                .ForMember(m => m.SongCount, opt => opt.MapFrom(m => m.UserPlaylistSongs.Count));
        }

        #endregion
    }
}
