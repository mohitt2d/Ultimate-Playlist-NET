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
    public class JobScheduledEventGridEventHandler : BaseMediaServiceEventGridEventHandler<JobStateChangedNonFinalEventData>
    {
        #region Constructor(s)

        public JobScheduledEventGridEventHandler(
            Lazy<IMapper> mapperProvider,
            Lazy<IAudioFileService> audioFileServiceServiceProvider,
            Lazy<IMediaServicesJobNameParserService> mediaServicesJobNameParserServiceProvider,
            Lazy<IEventGridEventDeserializerService> eventGridEventDeserializerProvider,
            Lazy<IEventGridEventTypeValidatorService> eventGridEventTypeValidatorServiceProvider,
            ILogger<JobScheduledEventGridEventHandler> logger)
            : base(
                mapperProvider,
                audioFileServiceServiceProvider,
                mediaServicesJobNameParserServiceProvider,
                eventGridEventDeserializerProvider,
                eventGridEventTypeValidatorServiceProvider,
                logger)
        {
        }

        #endregion

        #region Properties

        public override EventGridEventType EventGridEventType => EventGridEventType.MediaServicesJobScheduled;

        #endregion

        #region Protected methods

        protected override async Task<Result> HandleMediaServiceEventAsync(
            MediaServiceEventGridEvent<JobStateChangedNonFinalEventData> mediaServiceEvent)
        {
            return await AudioFileService.MarkAsScheduledAsync(mediaServiceEvent.JobName);
        }

        #endregion
    }
}
