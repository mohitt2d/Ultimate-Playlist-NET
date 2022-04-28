#region Usings

using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Services.Common.Interfaces.Files;
using UltimatePlaylist.Services.Common.Interfaces.Media;
using UltimatePlaylist.Services.Common.Interfaces.Webhook;
using UltimatePlaylist.Services.Common.Models.Media.Events;
using UltimatePlaylist.Services.Common.Models.Media.Events.Data;
using UltimatePlaylist.Services.Media.Handlers.Abstractions;

#endregion

namespace UltimatePlaylist.Services.Media.Handlers
{
    public class JobErroredEventGridEventHandler : BaseMediaServiceEventGridEventHandler<JobStateChangedFinalEventData>
    {
        #region Constructor(s)

        public JobErroredEventGridEventHandler(
            Lazy<IMapper> mapperProvider,
            Lazy<IAudioFileService> audioFileServiceProvider,
            Lazy<IMediaServicesJobNameParserService> mediaServicesJobNameParserServiceProvider,
            Lazy<IEventGridEventDeserializerService> eventGridEventDeserializerProvider,
            Lazy<IEventGridEventTypeValidatorService> eventGridEventTypeValidatorServiceProvider,
            ILogger<JobErroredEventGridEventHandler> logger)
            : base(
                mapperProvider,
                audioFileServiceProvider,
                mediaServicesJobNameParserServiceProvider,
                eventGridEventDeserializerProvider,
                eventGridEventTypeValidatorServiceProvider,
                logger)
        {
        }

        #endregion

        #region Properties

        public override EventGridEventType EventGridEventType => EventGridEventType.MediaServicesJobErrored;

        #endregion

        #region Protected methods

        protected override async Task<Result> HandleMediaServiceEventAsync(
            MediaServiceEventGridEvent<JobStateChangedFinalEventData> mediaServiceEvent)
        {
            var outputError = GetAssetError(mediaServiceEvent);

            return await AudioFileService.MarkAsErrorAsync(
                mediaServiceEvent.JobName,
                outputError.Code,
                outputError.Message);
        }

        #endregion

        #region Private methods

        private OutputError GetAssetError(MediaServiceEventGridEvent<JobStateChangedFinalEventData> mediaServiceEvent)
        {
            return mediaServiceEvent.Data.Outputs.First().Error;
        }

        #endregion
    }
}
