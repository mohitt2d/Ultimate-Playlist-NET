#region Usings

using UltimatePlaylist.Services.Common.Models.Webhook.Events;

#endregion

namespace UltimatePlaylist.Services.Common.Models.Media.Events
{
    public class MediaServiceEventGridEvent<TEventData> : EventGridEvent<TEventData>
        where TEventData : class
    {
        public string JobName { get; set; }
    }
}
