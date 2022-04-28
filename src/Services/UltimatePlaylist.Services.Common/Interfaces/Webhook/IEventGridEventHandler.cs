#region Usings

using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Newtonsoft.Json.Linq;
using UltimatePlaylist.Common.Enums;

#endregion

namespace UltimatePlaylist.Services.Common.Interfaces.Webhook
{
    public interface IEventGridEventHandler
    {
        EventGridEventType EventGridEventType { get; }

        Task<Result> HandleAsync(JToken jsonToken);
    }
}
