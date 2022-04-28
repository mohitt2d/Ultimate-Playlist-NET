#region Usings

using AutoMapper;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using UltimatePlaylist.AdminApi.Models.User;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Common.Filters.Models;
using UltimatePlaylist.Common.Models;
using UltimatePlaylist.Common.Mvc.Attributes;
using UltimatePlaylist.Common.Mvc.Controllers;
using UltimatePlaylist.Common.Mvc.File;
using UltimatePlaylist.Common.Mvc.Paging;
using UltimatePlaylist.Services.Common.Interfaces.Games;
using UltimatePlaylist.Services.Common.Interfaces.User;
using UltimatePlaylist.Services.Common.Models.UserManagment;
using static UltimatePlaylist.Common.Mvc.Consts.Consts;

#endregion

namespace UltimatePlaylist.AdminApi.Area.Admin
{
    [Area("Song")]
    [Route("[controller]")]
    [ApiExplorerSettings(GroupName = AdminApiGroups.Administrator)]
    public class PlayerPaymentManagementController : BaseController
    {
        #region Private field(s)

        private readonly Lazy<IMapper> MapperProvider;
        private readonly Lazy<IUserManagementService> UserManagementServiceProvider;
        private readonly Lazy<IPlayersPaymentService> PlayersPaymentServiceProvider;

        #endregion

        #region Constructor(s)

        public PlayerPaymentManagementController(
            Lazy<IMapper> mapperProvider,
            Lazy<IUserManagementService> userManagementServiceProvider,
            Lazy<IPlayersPaymentService> playersPaymentServiceProvider)
        {
            MapperProvider = mapperProvider;
            UserManagementServiceProvider = userManagementServiceProvider;
            PlayersPaymentServiceProvider = playersPaymentServiceProvider;
        }

        #endregion

        #region Private propertie(s)

        private IMapper Mapper => MapperProvider.Value;

        private IUserManagementService UserManagementService => UserManagementServiceProvider.Value;

        private IPlayersPaymentService PlayersPaymentService => PlayersPaymentServiceProvider.Value;

        #endregion

        #region GET

        [HttpPut("file")]
        [ProducesEnvelope(typeof(FileContentResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPlayerPaymentFile()
        {
            var filename = "AZ_1Day_ultimate_playlist_Draw_1.xlsx";
            return await PlayersPaymentService.GetPlayerPaymentFileData()
                .Map(usersList => Mapper.Map<IReadOnlyCollection<PlayerPaymentFileResponseModel>>(usersList))
                .Map(usersList => DataExportUtil.GetPlayerPaymentFile(usersList))
                .Finally(result => BuildFileResult(result, filename));
        }

        #endregion

        #region PUT

        [HttpPut("filter")]
        [ProducesEnvelope(typeof(PaginatedResponse<PlayerPaymentManagementListItemResponseModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Filter([FromBody] PlayerPaymentManagementFilterRequestModel playerPayment)
        {
            var serviceModel = Mapper.Map<PlayerPaymentManagementFilterReadServiceModel>(playerPayment);
            return await PlayersPaymentService.GetWinnersList(XPagination, serviceModel)
                .Map(usersList => Mapper.Map<PaginatedResponse<PlayerPaymentManagementListItemResponseModel>>(usersList))
                .Finally(BuildEnvelopeResult);
        }

        [HttpPut("payment-status")]
        [ProducesEnvelope(typeof(UserPaymentStatusResponseModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> ChangePaymentStatus([FromBody] UserPaymentStatusRequestModel model)
        {
            return await PlayersPaymentService.ChangePaymentStatus(model.UserExternalId, model.PaymentStatus, model.WinningExternalId)
                .Map(status => new UserPaymentStatusResponseModel() { UserExternalId = model.UserExternalId, PaymentStatus = status })
                .Finally(BuildEnvelopeResult);
        }

        [HttpPut("age-verification")]
        [ProducesEnvelope(typeof(UserAgeVerificationResponseModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> ChangeIsAgeVerified([FromBody] UserAgeVerificationRequestModel model)
        {
            return await UserManagementService.ChangeIsAgeVerified(model.UserExternalId, model.IsAgeVerified)
                .Map(isVerified => new UserAgeVerificationResponseModel() { UserExternalId = model.UserExternalId, IsAgeVerified = isVerified })
                .Finally(BuildEnvelopeResult);
        }

        #endregion
    }
}
