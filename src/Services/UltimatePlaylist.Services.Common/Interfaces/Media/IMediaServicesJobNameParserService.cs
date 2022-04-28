#region Usings

using CSharpFunctionalExtensions;

#endregion

namespace UltimatePlaylist.Services.Common.Interfaces.Media
{
    public interface IMediaServicesJobNameParserService
    {
        Result<string> Parse(string eventGridSubject);
    }
}
