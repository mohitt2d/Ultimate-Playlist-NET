#region Usings

using CSharpFunctionalExtensions;
using UltimatePlaylist.Common.Enums;

#endregion

namespace UltimatePlaylist.Services.Common.Interfaces.Media
{
    public interface IMediaServiceJobStateConverterService
    {
        Result<MediaServicesJobState> Convert(string jobStateName);
    }
}
