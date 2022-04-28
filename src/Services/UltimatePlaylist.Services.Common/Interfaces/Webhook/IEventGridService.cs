#region Usings

using System.Threading.Tasks;
using CSharpFunctionalExtensions;

#endregion

namespace UltimatePlaylist.Services.Common.Interfaces.Webhook
{
    public interface IEventGridService
    {
        Task<Result> ProcessAsync(string key);
    }
}
