#region Usings

using AutoMapper;
using UltimatePlaylist.MobileApi.Models.Leaderboard;
using UltimatePlaylist.Services.Common.Models.Leaderboard;

#endregion

namespace UltimatePlaylist.MobileApi.Mappings
{
    public class LeaderboardProfile : Profile
    {
        #region Constructor(s)

        public LeaderboardProfile()
        {
            // Read Service Model => Response Model
            CreateMap<LeaderboardUserScoresReadServiceModel, LeaderboardUserScoresResponseModel>();

            CreateMap<LeaderboardOtherUserScoresReadServiceModel, LeaderboardOtherUserScoresResponseModel>();

            CreateMap<LeaderboardReadServiceModel, LeaderboardResponseModel>();
        }

        #endregion
    }
}
