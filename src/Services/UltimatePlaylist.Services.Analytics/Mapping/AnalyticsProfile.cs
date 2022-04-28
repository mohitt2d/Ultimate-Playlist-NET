#region Usings

using AutoMapper;
using UltimatePlaylist.Services.Common.Models.Analytics;
using UltimatePlaylist.Services.Common.Models.Song;

#endregion

namespace UltimatePlaylist.Services.Analytics.Mapping
{
    public class AnalyticsProfile : Profile
    {
        #region Constructor(s)

        public AnalyticsProfile()
        {
            CreateMap<SkipSongReadServiceModel, AnalyticsLastEarnedTicketsReadServiceModel>();
        }

        #endregion
    }
}
