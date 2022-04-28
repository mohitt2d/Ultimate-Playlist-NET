#region Usings

using System;
using System.Threading.Tasks;
using AutoMapper;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using UltimatePlaylist.Services.Common.Interfaces.Files;
using UltimatePlaylist.Services.Common.Interfaces.Media;
using UltimatePlaylist.Services.Common.Interfaces.Webhook;
using UltimatePlaylist.Services.Common.Models.Media.Events;
using UltimatePlaylist.Services.Common.Models.Webhook.Events;
using UltimatePlaylist.Services.Webhook.Handlers.Abstractions;

#endregion

namespace UltimatePlaylist.Services.Media.Handlers.Abstractions
{
    public abstract class BaseMediaServiceEventGridEventHandler<TEventData>
        : BaseEventGridEventHandler<TEventData>
        where TEventData : class
    {
        #region Private fields

        private readonly Lazy<IMapper> MapperProvider;
        private readonly Lazy<IMediaServicesJobNameParserService> MediaServicesJobNameParserServiceProvider;
        private readonly Lazy<IAudioFileService> AudioFileServiceProvider;

        #endregion

        #region Constructor(s)

        public BaseMediaServiceEventGridEventHandler(
            Lazy<IMapper> mapperProvider,
            Lazy<IAudioFileService> audioFileServiceProvider,
            Lazy<IMediaServicesJobNameParserService> mediaServicesJobNameParserServiceProvider,
            Lazy<IEventGridEventDeserializerService> eventGridEventDeserializerProvider,
            Lazy<IEventGridEventTypeValidatorService> eventGridEventTypeValidatorServiceProvider,
            ILogger logger)
            : base(eventGridEventDeserializerProvider, eventGridEventTypeValidatorServiceProvider, logger)
        {
            MapperProvider = mapperProvider;
            AudioFileServiceProvider = audioFileServiceProvider;
            MediaServicesJobNameParserServiceProvider = mediaServicesJobNameParserServiceProvider;
        }

        #endregion

        #region Properties

        protected IAudioFileService AudioFileService => AudioFileServiceProvider.Value;

        protected IMapper Mapper => MapperProvider.Value;

        private IMediaServicesJobNameParserService MediaServicesJobNameParser => MediaServicesJobNameParserServiceProvider.Value;

        #endregion

        #region Protected methods

        protected override async Task<Result> HandleEventAsync(EventGridEvent<TEventData> eventGridEvent)
        {
            return await Result.Success(eventGridEvent)
                .Map(@event => Mapper.Map<MediaServiceEventGridEvent<TEventData>>(@event))
                .Bind(mediaServiceEvent => MediaServicesJobNameParser.Parse(eventGridEvent.Subject)
                    .Tap(jobName => mediaServiceEvent.JobName = jobName)
                    .Map(_ => mediaServiceEvent))
                .Bind(async mediaServiceEvent => await HandleMediaServiceEventAsync(mediaServiceEvent));
        }

        protected abstract Task<Result> HandleMediaServiceEventAsync(MediaServiceEventGridEvent<TEventData> mediaServiceEventGridEvent);

        #endregion
    }
}
