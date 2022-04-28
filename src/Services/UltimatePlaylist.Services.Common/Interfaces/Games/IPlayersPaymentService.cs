#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Common.Filters.Models;
using UltimatePlaylist.Common.Models;
using UltimatePlaylist.Services.Common.Models;
using UltimatePlaylist.Services.Common.Models.UserManagment;

#endregion

namespace UltimatePlaylist.Services.Common.Interfaces.Games
{
    public interface IPlayersPaymentService
    {
        Task<Result<WinningStatus>> ChangePaymentStatus(Guid userExternalId, WinningStatus paymentStatus, Guid gameExternalId);

        Task<Result<PaginatedReadServiceModel<PlayerPaymentManagementListItemReadServiceModel>>> GetWinnersList(Pagination pagination, PlayerPaymentManagementFilterReadServiceModel serviceModel);

        Task<Result<IReadOnlyList<PlayerPaymentReadServiceModel>>> GetPlayerPaymentFileData();
    }
}
