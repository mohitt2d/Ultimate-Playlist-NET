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
    public class JobCanceledEventGridEventHandler : BaseMediaServiceEventGridEventHandler<JobStateChangedFinalEventData>
    {
        #region Constructor(s)

        public JobCanceledEventGridEventHandler(
            Lazy<IMapper> mapperProvider,
            Lazy<IAudioFileService> audioFileServiceProvider,
            Lazy<IMediaServicesJobNameParserService> mediaServicesJobNameParserServiceProvider,
            Lazy<IEventGridEventDeserializerService> eventGridEventDeserializerProvider,
            Lazy<IEventGridEventTypeValidatorService> eventGridEventTypeValidatorServiceProvider,
            ILogger<JobCanceledEventGridEventHandler> logger)
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

        public override EventGridEventType EventGridEventType => EventGridEventType.MediaServicesJobCanceled;

        #endregion

        #region Protected methods

        protected override async Task<Result> HandleMediaServiceEventAsync(
            MediaServiceEventGridEvent<JobStateChangedFinalEventData> mediaServiceEvent)
        {
            return await AudioFileService.MarkAsCanceledAsync(mediaServiceEvent.JobName);
        }

        #endregion
    }
}
