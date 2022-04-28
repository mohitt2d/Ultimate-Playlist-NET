#region Usings

using CSharpFunctionalExtensions;
using Newtonsoft.Json.Linq;

#endregion

namespace UltimatePlaylist.Services.Common.Interfaces.Webhook
{
    public interface IEventGridEventArrayReaderService
    {
        Result<JArray> ReadArray(string json);
    }
}
