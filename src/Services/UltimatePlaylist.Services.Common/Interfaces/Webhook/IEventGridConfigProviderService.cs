#region Usings

using UltimatePlaylist.Services.Common.Models.Webhook;

#endregion

namespace UltimatePlaylist.Services.Common.Interfaces.Webhook
{
    public interface IEventGridConfigProviderService
    {
        EventGridConfigReadServiceModel GetConfig();
    }
}
