namespace UltimatePlaylist.Common.Mvc.Interface
{
    public interface IUserLastActiveService
    {
        Task SetLastActiveToUtcNow(Guid userExternalId);
    }
}
