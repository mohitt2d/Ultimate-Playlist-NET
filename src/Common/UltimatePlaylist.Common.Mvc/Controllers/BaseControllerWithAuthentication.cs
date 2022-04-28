#region Usings

using Microsoft.AspNetCore.Http;
using UltimatePlaylist.Common.Mvc.Attributes;

#endregion

namespace UltimatePlaylist.Common.Mvc.Controllers
{
    /// <summary>
    /// This controller is protected by "Bearer" attribute so you don't have to add this attribute to your controllers.
    /// </summary>
    [ProducesEmptyEnvelope(StatusCodes.Status401Unauthorized)]
    [ProducesEmptyEnvelope(StatusCodes.Status403Forbidden)]
    [Bearer]
    public class BaseControllerWithAuthentication : BaseController
    {
    }
}
