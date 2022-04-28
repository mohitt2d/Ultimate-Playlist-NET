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
    public class JobFinishedEventGridEventHandler : BaseMediaServiceEventGridEventHandler<JobStateChangedFinalEventData>
    {
        #region Private fields

        private readonly Lazy<IMediaService> mediaServiceProvider;

        #endregion

        #region Constructor(s)

        public JobFinishedEventGridEventHandler(
            Lazy<IMediaService> mediaServiceProvider,
            Lazy<IMapper> mapperProvider,
            Lazy<IAudioFileService> audioFileServiceProvider,
            Lazy<IMediaServicesJobNameParserService> mediaServicesJobNameParserServiceProvider,
            Lazy<IEventGridEventDeserializerService> eventGridEventDeserializerProvider,
            Lazy<IEventGridEventTypeValidatorService> eventGridEventTypeValidatorServiceProvider,
            ILogger<JobFinishedEventGridEventHandler> logger)
            : base(
                mapperProvider,
                audioFileServiceProvider,
                mediaServicesJobNameParserServiceProvider,
                eventGridEventDeserializerProvider,
                eventGridEventTypeValidatorServiceProvider,
                logger)
        {
            this.mediaServiceProvider = mediaServiceProvider;
        }

        #endregion

        #region Properties

        public override EventGridEventType EventGridEventType => EventGridEventType.MediaServicesJobFinished;

        private IMediaService MediaService => mediaServiceProvider.Value;

        #endregion

        #region Protected methods

        protected override async Task<Result> HandleMediaServiceEventAsync(
            MediaServiceEventGridEvent<JobStateChangedFinalEventData> mediaServiceEvent)
        {
            return await Result.Success()
                .Map(() => GetAssetName(mediaServiceEvent))
                .Map(async assetName => await MediaService.PublishAudioAssetAsync(assetName))
                .Bind(async audioUris => await AudioFileService.MarkAsFinishedAsync(mediaServiceEvent.JobName, audioUris));
        }

        #endregion

        #region Private methods

        private string GetAssetName(MediaServiceEventGridEvent<JobStateChangedFinalEventData> mediaServiceEvent)
        {
            return mediaServiceEvent.Data.Outputs.First().AssetName;
        }

        #endregion
    }
}
