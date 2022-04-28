#region Usings

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CSharpFunctionalExtensions;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Common.Exceptions;
using UltimatePlaylist.Database.Infrastructure.Entities.File;
using UltimatePlaylist.Database.Infrastructure.Entities.File.Specifications;
using UltimatePlaylist.Database.Infrastructure.Repositories.Interfaces;
using UltimatePlaylist.Services.Common.Interfaces.Files;
using UltimatePlaylist.Services.Common.Interfaces.Media;
using UltimatePlaylist.Services.Common.Models.Files;
using UltimatePlaylist.Services.Common.Models.Media;

#endregion

namespace UltimatePlaylist.Services.File
{
    public class AudioFileService : FileService<AudioFileEntity>, IAudioFileService
    {
        #region Private fields

        private readonly Lazy<IMediaAssetJobStateValidatorService> MediaAssetJobStateValidatorServiceProvider;

        #endregion

        #region Constructor(s)

        public AudioFileService(
            Lazy<IMapper> mapperProvider,
            Lazy<IMediaAssetJobStateValidatorService> mediaAssetJobStateValidatorServiceProvider,
            Lazy<IRepository<AudioFileEntity>> fileRepositoryProvider,
            Lazy<ICloudStorageService> cloudStorageServiceProvider,
            Lazy<IFileNameFormatterService> fileNameFormatterServiceProvider)
            : base(mapperProvider, fileRepositoryProvider, cloudStorageServiceProvider, fileNameFormatterServiceProvider)
        {
            MediaAssetJobStateValidatorServiceProvider = mediaAssetJobStateValidatorServiceProvider;
        }

        #endregion

        #region Properties

        private IMediaAssetJobStateValidatorService MediaAssetJobStateValidator => MediaAssetJobStateValidatorServiceProvider.Value;

        #endregion

        #region Public methods

        public Task<Result<AudioFileReadServiceModel>> MarkAsCanceledAsync(string jobName)
        {
            return MarkNewJobStateAsync(jobName, MediaServicesJobState.Canceled);
        }

        public async Task<Result<AudioFileReadServiceModel>> MarkAsErrorAsync(
            string jobName,
            MediaServicesJobErrorCode errorCode,
            string errorMessage)
        {
            return await GetByJobNameAsync(jobName)
                .Bind(audio => MediaAssetJobStateValidator.Validate(audio.JobState, MediaServicesJobState.Error)
                    .Map(() => audio))
                .Tap(audio =>
                {
                    audio.JobErrorCode = errorCode;
                    audio.JobErrorMessage = errorMessage;
                    audio.JobState = MediaServicesJobState.Error;
                })
                .Tap(async audio => await FileRepository.UpdateAndSaveAsync(audio))
                .Map(audio => Mapper.Map<AudioFileReadServiceModel>(audio));
        }

        public async Task<Result<AudioFileReadServiceModel>> MarkAsFinishedAsync(
            string jobName,
            AudioAssetPublishedReadServiceModel videoAssetPublishedReadServiceModel)
        {
            return await GetByJobNameAsync(jobName)
                .Bind(audio => MediaAssetJobStateValidator.Validate(audio.JobState, MediaServicesJobState.Finished)
                    .Map(() => audio))
                .Tap(audio =>
                {
                    audio.StreamingUrl = videoAssetPublishedReadServiceModel.StreamingUris.First().AbsoluteUri;
                    audio.JobState = MediaServicesJobState.Finished;
                })
                .Tap(async audio => await FileRepository.UpdateAndSaveAsync(audio))
                .Map(audio => Mapper.Map<AudioFileReadServiceModel>(audio));
        }

        public Task<Result<AudioFileReadServiceModel>> MarkAsProcessingAsync(string jobName)
        {
            return MarkNewJobStateAsync(jobName, MediaServicesJobState.Processing);
        }

        public async Task<Result<AudioFileReadServiceModel>> MarkAsQueuedAsync(AudioFileQueuedWriteServiceModel writeServiceModel)
        {
            return await GetByExternalIdAsync(writeServiceModel.ExternalId)
                .Bind(audio => MediaAssetJobStateValidator.Validate(audio.JobState, MediaServicesJobState.Queued)
                    .Map(() => audio))
                .Tap(audio =>
                {
                    audio.JobName = writeServiceModel.JobName;
                    audio.JobState = MediaServicesJobState.Queued;
                    audio.OutputAssetName = writeServiceModel.OutputAssetName;
                    audio.OutputContainerName = writeServiceModel.OutputContainerName;
                    audio.TransformName = writeServiceModel.TransformName;
                })
                .Tap(async audio => await FileRepository.UpdateAndSaveAsync(audio))
                .Map(audio => Mapper.Map<AudioFileReadServiceModel>(audio));
        }

        public Task<Result<AudioFileReadServiceModel>> MarkAsScheduledAsync(string jobName)
        {
            return MarkNewJobStateAsync(jobName, MediaServicesJobState.Scheduled);
        }

        public Task<FileReadServiceModel> SaveNewAudioFileAsync(
            string videoAssetName,
            string containerName,
            Stream fileStream,
            string fileName)
        {
            var newFile = new AudioFileEntity
            {
                InputAssetName = videoAssetName,
                InputContainerName = containerName,
                JobState = MediaServicesJobState.None,
            };

            return SaveNewFileAsync(newFile, containerName, fileStream, fileName);
        }

        public async Task RemoveAudioFileAsync(string fileName)
        {
            var fileEntity = await FileRepository.FirstOrDefaultAsync(new AudioFileSpecification().ByFileName(fileName))
                ?? throw new NotFoundException(ErrorType.AudioFileNotFound.ToString());
            await RemoveFileAsync(fileEntity);
            await FileRepository.DeleteAsync(new AudioFileSpecification().ById(fileEntity.Id));
        }

        public Task<FileReadServiceModel> SaveWithoutMediaServiceAudioFileAsync(
            Stream fileStream,
            string fileName)
        {
            var newFile = new AudioFileEntity
            {
                InputAssetName = GetFileNameForMedia(fileName),
                InputContainerName = ContainerType.AudioFiles.ToString(),
                JobState = MediaServicesJobState.None,
            };

            return SaveNewFileAsync(newFile, ContainerType.AudioFiles, fileStream, fileName);
        }

        #endregion

        #region Private methods

        private async Task<Result<AudioFileEntity>> GetByExternalIdAsync(Guid externalId)
        {
            var audio = await FileRepository.FirstOrDefaultAsync(new AudioFileSpecification()
                .ByExternalId(externalId));

            return Maybe<AudioFileEntity>.From(audio)
                .ToResult(ErrorType.AudioFileNotFound.ToString());
        }

        private async Task<Result<AudioFileEntity>> GetByJobNameAsync(string jobName)
        {
            var audio = await FileRepository.FirstOrDefaultAsync(
                new AudioFileSpecification()
                    .ByJobName(jobName));

            return Maybe<AudioFileEntity>.From(audio)
                .ToResult(ErrorType.AudioFileNotFound.ToString());
        }

        private async Task<Result<AudioFileReadServiceModel>> MarkNewJobStateAsync(
            string jobName,
            MediaServicesJobState newJobState)
        {
            return await GetByJobNameAsync(jobName)
                .Bind(video => MediaAssetJobStateValidator.Validate(video.JobState, newJobState)
                    .Map(() => video))
                .Tap(video => video.JobState = newJobState)
                .Tap(async video => await FileRepository.UpdateAndSaveAsync(video))
                .Map(video => Mapper.Map<AudioFileReadServiceModel>(video));
        }

        private string GetFileNameForMedia(string fileName) => $"{Guid.NewGuid()}/{FileNameFormatterService.Format(fileName)}";

        #endregion
    }
}
