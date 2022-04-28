#region Usings

using AutoMapper;
using UltimatePlaylist.AdminApi.Models.Schedule;
using UltimatePlaylist.Services.Common.Models.Playlist;
using UltimatePlaylist.Services.Common.Models.Schedule;

#endregion

namespace UltimatePlaylist.AdminApi.Mappings
{
    public class ScheduleProfile : Profile
    {
        public ScheduleProfile()
        {
            // Request Model => Write Service Model
            CreateMap<PlaylistBaseRequestModel, PlaylistBaseWriteServiceModel>();

            CreateMap<AddSongToPlaylistRequestModel, AddSongToPlaylistWriteServiceModel>();

            CreateMap<RemoveSongFromPlaylistRequestModel, RemoveSongFromPlaylistWriteServiceModel>()
                .IncludeBase<PlaylistBaseRequestModel, PlaylistBaseWriteServiceModel>();

            // Read service model => Response model
            CreateMap<PlaylistScheduleCalendarReadServiceModel, PlaylistScheduleCalendarResponseModel>();

            CreateMap<PlaylistsScheduleCalendarInfoReadServiceModel, PlaylistsScheduleCalendarInfoResponseModel>();

            CreateMap<AdminPlaylistReadServiceModel, PlaylistsResponseModel>();
        }
    }
}
