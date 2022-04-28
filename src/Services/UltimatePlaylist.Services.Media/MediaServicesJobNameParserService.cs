#region Usings

using System.Linq;
using CSharpFunctionalExtensions;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Services.Common.Interfaces.Media;

#endregion

namespace UltimatePlaylist.Services.Media
{
    public class MediaServicesJobNameParserService : IMediaServicesJobNameParserService
    {
        #region Public methods

        public Result<string> Parse(string eventGridSubject)
        {
            return Result.FailureIf(
                    string.IsNullOrWhiteSpace(eventGridSubject),
                    eventGridSubject,
                    ErrorType.InvalidEventSubject.ToString())
                .Map(value => value.Split("/"))
                .Ensure(parts => parts.Length == 4, ErrorType.InvalidEventSubject.ToString())
                .Map(parts => parts.Last());
        }

        #endregion
    }
}
