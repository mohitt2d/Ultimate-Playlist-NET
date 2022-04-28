namespace UltimatePlaylist.Common.Mvc.Interface
{
    public interface IUserBlacklistTokenStore
    {
        void Set(Guid externalId, string token);

        bool TryGet(Guid externalId, out string token);
    }
}
