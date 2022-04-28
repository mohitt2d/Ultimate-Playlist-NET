#region Usings

using System;
using CSharpFunctionalExtensions;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Services.Common.Interfaces.Media;

#endregion

namespace UltimatePlaylist.Services.Media
{
    public class MediaServiceJobStateConverterService : IMediaServiceJobStateConverterService
    {
        #region Publice methods

        public Result<MediaServicesJobState> Convert(string jobStateName)
        {
            return Result.SuccessIf(
                Enum.TryParse<MediaServicesJobState>(jobStateName, ignoreCase: true, out var jobState),
                jobState,
                ErrorType.InvalidMediaServiceJobState.ToString());
        }

        #endregion
    }
}
