#region Usings

using AutoMapper;
using UltimatePlaylist.MobileApi.Models.Games;
using UltimatePlaylist.Services.Common.Models.Games;

#endregion

namespace UltimatePlaylist.MobileApi.Mappings
{
    public class GamesProfile : Profile
    {
        #region Constructor(s)

        public GamesProfile()
        {
            CreateMap<GamesinfoReadServiceModel, GamesInfoResponseModel>();
            CreateMap<UserWinningReadServicModel, UserWinningResponseModel>()
                .ForMember(i => i.Amount, opt => opt.MapFrom(c => $"${c.Amount}"));
            CreateMap<WinnerProfileReadServiceModel, WinnerProfileResponseModel>()
                .ForMember(i => i.Amount, opt => opt.MapFrom(c => $"${c.Amount}"));
            CreateMap<WinnersReadServiceModel, WinnersResponseModel>();

            CreateMap<UltimatePayoutReadServiceModel, UltimatePayoutResponseModel>()
             .ForMember(i => i.NextUltimatePrize, opt => opt.MapFrom(c => $"${c.NextUltimatePrize}"));
            CreateMap<UltimatePayoutWinnerReadServiceModel, UltimatePayoutWinnerResponseModel>()
                .ForMember(i => i.Amount, opt => opt.MapFrom(c => $"${c.Amount}"));
        }

        #endregion
    }
}
