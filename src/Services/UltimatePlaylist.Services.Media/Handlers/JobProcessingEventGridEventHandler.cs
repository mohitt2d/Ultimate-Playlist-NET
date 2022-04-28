#region Usings

using System;
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
    public class JobProcessingEventGridEventHandler : BaseMediaServiceEventGridEventHandler<JobStateChangedNonFinalEventData>
    {
        #region Constructor(s)

        public JobProcessingEventGridEventHandler(
            Lazy<IMapper> mapperProvider,
            Lazy<IAudioFileService> audioFileServiceProvider,
            Lazy<IMediaServicesJobNameParserService> mediaServicesJobNameParserServiceProvider,
            Lazy<IEventGridEventDeserializerService> eventGridEventDeserializerProvider,
            Lazy<IEventGridEventTypeValidatorService> eventGridEventTypeValidatorServiceProvider,
            ILogger<JobProcessingEventGridEventHandler> logger)
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

        public override EventGridEventType EventGridEventType => EventGridEventType.MediaServicesJobProcessing;

        #endregion

        #region Protected methods

        protected override async Task<Result> HandleMediaServiceEventAsync(
            MediaServiceEventGridEvent<JobStateChangedNonFinalEventData> mediaServiceEvent)
        {
            return await AudioFileService.MarkAsProcessingAsync(mediaServiceEvent.JobName);
        }

        #endregion
    }
}
