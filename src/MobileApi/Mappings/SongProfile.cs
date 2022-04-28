#region Usings

using AutoMapper;
using UltimatePlaylist.MobileApi.Models.Song;
using UltimatePlaylist.Services.Common.Models.Song;
using UltimatePlaylist.Services.Common.Models.Ticket;

#endregion

namespace UltimatePlaylist.MobileApi.Mappings
{
    public class SongProfile : Profile
    {
        #region Constructor(s)

        public SongProfile()
        {
            // Request model => Write Service Mdoel
            CreateMap<RateSongRequestModel, RateSongWriteServiceModel>();

            // Read Service Model => Response Model
            CreateMap<EarnedTicketsReadServiceModel, RatingEarnedTicketsResponseModel>();

            CreateMap<UserSongReadServiceModel, UserSongHistoryResponseModel>();
        }

        #endregion
    }
}
