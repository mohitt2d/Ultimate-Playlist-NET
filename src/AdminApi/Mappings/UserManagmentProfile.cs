#region Usings

using AutoMapper;
using UltimatePlaylist.AdminApi.Models.User;
using UltimatePlaylist.Common.Mvc.Extensions;
using UltimatePlaylist.Services.Common.Models.UserManagment;

#endregion

namespace UltimatePlaylist.AdminApi.Mappings
{
    public class UserManagmentProfile : Profile
    {
        public UserManagmentProfile()
        {
            CreateMap<UserListItemReadServiceModel, UserListItemResponseModel>()
                .ForMemberMapFrom(dst => dst.AvarageTimeListened, src => GetTimeSpan(src.AvarageTimeListened))
                .ForMemberMapFrom(dst => dst.TotalMinutesListened, src => GetTimeSpan(src.TotalMinutesListened));
            CreateMap<PlayerPaymentManagementListItemReadServiceModel, PlayerPaymentManagementListItemResponseModel>();
            CreateMap<PlayerPaymentReadServiceModel, PlayerPaymentFileResponseModel>();

            CreateMap<ListenersStatisticsReadServiceModel, ListenersStatisticsResponseModel>()
                .ForMemberMapFrom(dst => dst.AverageTimeListenedUser, src => GetTimeSpan(src.AverageTimeListenedUser));

            CreateMap<ListenersStatisticsRequestModel, ListenersReadServiceModel>();
            CreateMap<PlayerPaymentManagementFilterRequestModel, PlayerPaymentManagementFilterReadServiceModel>();
        }

        private TimeSpan GetTimeSpan(double value)
        {
            var hours = value > 60 ? value / 60 : 0;
            var minutes = value - (hours * 60);
            return new TimeSpan(Convert.ToInt32(hours), Convert.ToInt32(minutes), seconds: 0);
        }
    }
}
