#region Usings

using AutoMapper;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using UltimatePlaylist.Common.Mvc.Attributes;
using UltimatePlaylist.Common.Mvc.Controllers;
using UltimatePlaylist.MobileApi.Models.Notification;
using UltimatePlaylist.Services.Common.Interfaces.Notification;
using static UltimatePlaylist.Common.Mvc.Consts.Consts;

#endregion

namespace UltimatePlaylist.MobileApi.Area.Notification
{
    [Area("Notification")]
    [Route("[controller]")]
    [ApiExplorerSettings(GroupName = MobileApiGroups.User)]
    public class NotificationController : BaseControllerWithAuthentication
    {
        #region Private Members

        private readonly Lazy<IMapper> MapperProvider;
        private readonly Lazy<INotificationsSettingsService> NotificationsSettingsServiceProvider;

        #endregion

        #region Constructor(s)

        public NotificationController(
            Lazy<IMapper> mapperProvider,
            Lazy<INotificationsSettingsService> notificationsSettingsServiceProvider)
        {
            MapperProvider = mapperProvider;
            NotificationsSettingsServiceProvider = notificationsSettingsServiceProvider;
        }

        #endregion

        #region Private Properites

        private IMapper Mapper => MapperProvider.Value;

        private INotificationsSettingsService NotificationsSettingsService => NotificationsSettingsServiceProvider.Value;

        #endregion

        #region POST

        [HttpPost("device-token")]
        [ProducesEmptyEnvelope(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateDeviceToken([FromBody] DeviceTokenRequestModel requestModel)
        {
            return await NotificationsSettingsService.SetDeviceToken(requestModel.DeviceToken, XUserExternalId)
               .Finally(BuildEnvelopeResult);
        }

        [HttpPost("logout")]
        [ProducesEmptyEnvelope(StatusCodes.Status200OK)]
        public async Task<IActionResult> Logout()
        {
            return await NotificationsSettingsService.RemoveDeviceToken(XUserExternalId)
                .Finally(BuildEnvelopeResult);
        }

        [HttpPost("update-enable-status")]
        [ProducesEmptyEnvelope(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateEnableStatus([FromBody] NotificationEnableRequestModel notificationEnableRequestModel)
        {
            return await NotificationsSettingsService.UpdateEnableStatusAsync(XUserExternalId, notificationEnableRequestModel.IsEnabled)
                .Finally(BuildEnvelopeResult);
        }

        #endregion
    }
}
