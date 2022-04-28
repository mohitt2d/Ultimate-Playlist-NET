#region Usings

using AutoMapper;
using UltimatePlaylist.Common.Mvc.Extensions;
using UltimatePlaylist.Database.Infrastructure.Entities.Identity;
using UltimatePlaylist.Database.Infrastructure.Views;
using UltimatePlaylist.Services.Common.Models.UserManagment;

#endregion

namespace UltimatePlaylist.Services.UserManagement.Mapping
{
    public class UserManagementProfile : Profile
    {
        #region Constructor(s)

        public UserManagementProfile()
        {
            CreateMap<UserManagementProcedureView, UserListItemReadServiceModel>();
            CreateMap<ListenersStatisticsProcedureView, ListenersStatisticsReadServiceModel>();
        }

        #endregion
    }
}
