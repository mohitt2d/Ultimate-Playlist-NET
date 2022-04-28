namespace UltimatePlaylist.Common.Mvc.Interface
{
    public interface IUserEmailChangeConfirmedFromWebService
    {
        Task CheckIfUserShouldBeLogOut(Guid userExternalId, string userToken);
    }
}
