namespace UltimatePlaylist.Services.Common.Interfaces.Identity
{
    public interface IUserActiveStore
    {
        void Set(Guid externalId, bool isActive);

        bool TryGet(Guid externalId, out bool isActive);
    }
}
