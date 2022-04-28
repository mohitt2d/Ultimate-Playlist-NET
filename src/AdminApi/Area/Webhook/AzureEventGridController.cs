#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UltimatePlaylist.AdminApi.Models.Webhook;
using UltimatePlaylist.Common.Mvc.Attributes;
using UltimatePlaylist.Common.Mvc.Controllers;
using UltimatePlaylist.Services.Common.Interfaces.Webhook;
using static UltimatePlaylist.Common.Mvc.Consts.Consts;

#endregion

namespace UltimatePlaylist.AdminApi.Area.Webhook
{
    [Area("Webhook")]
    [Route("[area]/[controller]")]
    [ApiExplorerSettings(GroupName = AdminApiGroups.Administrator)]
    public class AzureEventGridController : BaseController
    {
        #region Private fields

        private readonly Lazy<IEventGridService> EventGridServicerProvider;

        #endregion

        #region Constructor(s)

        public AzureEventGridController(
            Lazy<IEventGridService> eventGridServicerProvider)
        {
            EventGridServicerProvider = eventGridServicerProvider;
        }

        #endregion

        #region Properties

        private IEventGridService EventGridService => EventGridServicerProvider.Value;

        #endregion

        #region POST

        [HttpPost("job-status-changed")]
        [ProducesEmptyEnvelope(StatusCodes.Status200OK)]
        public async Task<IActionResult> ReceiveJobStatusChangedEventAsync([FromQuery] EventGridEventRequestModel request)
        {
            return await EventGridService.ProcessAsync(request.Key)
                .Finally(BuildEnvelopeResult);
        }

        #endregion
    }
}
