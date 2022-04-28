#region Usings

using CSharpFunctionalExtensions;

#endregion

namespace UltimatePlaylist.Services.Common.Interfaces.Webhook
{
    public interface IEventGridEventKeyValidatorService
    {
        Result Validate(string queryKey);
    }
}
