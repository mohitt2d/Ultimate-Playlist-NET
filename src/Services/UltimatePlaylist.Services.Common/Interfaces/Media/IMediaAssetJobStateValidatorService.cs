#region Usings

using CSharpFunctionalExtensions;
using UltimatePlaylist.Common.Enums;

#endregion

namespace UltimatePlaylist.Services.Common.Interfaces.Media
{
    public interface IMediaAssetJobStateValidatorService
    {
        Result Validate(MediaServicesJobState currentJobState, MediaServicesJobState jobStateToValidate);
    }
}
