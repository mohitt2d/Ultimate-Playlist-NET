namespace UltimatePlaylist.Services.Common.Interfaces.Webhook
{
    public interface IEventGridEventHandler<TEventData> : IEventGridEventHandler
        where TEventData : class
    {
    }
}
