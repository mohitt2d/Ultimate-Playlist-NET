namespace UltimatePlaylist.Common.Mvc.Interface
{
    public interface IUserActiveService
    {
        Task CheckActiveStatusAndSetTokenOnBlacklist(Guid userExternalId, string token);

        void UpdateActiveStatusInStore(Guid userExternalId, bool isActive);
    }
}
