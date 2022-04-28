#region Usings

using CSharpFunctionalExtensions;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Services.Common.Interfaces.Media;

#endregion

namespace UltimatePlaylist.Services.Media.Validators
{
    public class MediaAssetJobStateValidatorService : IMediaAssetJobStateValidatorService
    {
        #region Public methods

        public Result Validate(MediaServicesJobState currentJobState, MediaServicesJobState jobStateToValidate)
        {
            return Result.FailureIf(
                currentJobState == jobStateToValidate,
                ErrorType.CannotSetSameJobStateTwice.ToString());
        }

        #endregion
    }
}
