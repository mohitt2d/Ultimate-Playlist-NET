#region Usings

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using CSharpFunctionalExtensions;
using Microsoft.Azure.Management.Media;
using Microsoft.Azure.Management.Media.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using UltimatePlaylist.Common.Config;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Common.Extensions;
using UltimatePlaylist.Services.Common.Interfaces.Files;
using UltimatePlaylist.Services.Common.Interfaces.Media;
using UltimatePlaylist.Services.Common.Models.Files;
using UltimatePlaylist.Services.Common.Models.Media;

#endregion

namespace UltimatePlaylist.Services.Media
{
    public class AzureMediaService : IMediaService
    {
        #region Private consts

        private const string DefaultStreamingEndpointName = "default";
        private const string EncodingTransformName = "encoding-adaptive-streaming-transform";
        private static readonly PredefinedStreamingPolicy StreamingPolicy = PredefinedStreamingPolicy.ClearStreamingOnly;
        private static readonly EncoderNamedPreset EncoderPreset = EncoderNamedPreset.AdaptiveStreaming;
        private static readonly TimeSpan StreamingLocatorLifetime = TimeSpan.FromDays(365 * 200);

        #endregion

        #region Private fields

        private readonly Lazy<ILogger<AzureMediaService>> LoggerProvider;
        private readonly Lazy<IOptions<AzureMediaServicesConfig>> AzureMediaServicesConfigProvider;
        private readonly Lazy<IAzureClientService> ServiceClientCredentialsServiceProvider;
        private readonly Lazy<IAudioFileService> AudioFileServiceProvider;
        private readonly Lazy<IMediaAssetNameFormatterService> MediaAssetNameFormatterServiceProvider;
        private readonly Lazy<IMediaAssetContainerNameFormatterService> MediaAssetContainerNameFormatterServiceProvider;
        private readonly Lazy<IMediaServiceJobStateConverterService> MediaServiceJobStateConverterServiceProvider;

        #endregion

        #region Constructor(s)

        public AzureMediaService(
            Lazy<ILogger<AzureMediaService>> loggerProvider,
            Lazy<IOptions<AzureMediaServicesConfig>> azureMediaServicesConfigProvider,
            Lazy<IAzureClientService> serviceClientCredentialsServiceProvider,
            Lazy<IAudioFileService> audioFileServiceProvider,
            Lazy<IMediaAssetNameFormatterService> mediaAssetNameFormatterServiceProvider,
            Lazy<IMediaAssetContainerNameFormatterService> mediaAssetContainerNameFormatterServiceProvider,
            Lazy<IMediaServiceJobStateConverterService> mediaServiceJobStateConverterServiceProvider)
        {
            LoggerProvider = loggerProvider;
            AzureMediaServicesConfigProvider = azureMediaServicesConfigProvider;
            ServiceClientCredentialsServiceProvider = serviceClientCredentialsServiceProvider;
            AudioFileServiceProvider = audioFileServiceProvider;
            MediaAssetNameFormatterServiceProvider = mediaAssetNameFormatterServiceProvider;
            MediaAssetContainerNameFormatterServiceProvider = mediaAssetContainerNameFormatterServiceProvider;
            MediaServiceJobStateConverterServiceProvider = mediaServiceJobStateConverterServiceProvider;
        }

        #endregion

        #region Properties

        private ILogger<AzureMediaService> Logger => LoggerProvider.Value;

        private AzureMediaServicesConfig AzureMediaServicesConfig => AzureMediaServicesConfigProvider.Value.Value;

        private IAzureClientService ServiceClientCredentialsService => ServiceClientCredentialsServiceProvider.Value;

        private IAudioFileService AudioFileService => AudioFileServiceProvider.Value;

        private IMediaAssetNameFormatterService MediaAssetNameFormatter => MediaAssetNameFormatterServiceProvider.Value;

        private IMediaAssetContainerNameFormatterService MediaAssetContainerNameFormatter => MediaAssetContainerNameFormatterServiceProvider.Value;

        private IMediaServiceJobStateConverterService MediaServiceJobStateConverter => MediaServiceJobStateConverterServiceProvider.Value;

        private string ResourceGroupName => AzureMediaServicesConfig.ResourceGroup;

        private string AccountName => AzureMediaServicesConfig.AccountName;

        #endregion

        #region Public methods

        public async Task<AudioAssetCreatedReadServiceModel> CreateAudioAssetAsync(Stream fileStream, string assetName)
        {
            var timestamp = DateTime.UtcNow;
            var assetUniqueGuid = Guid.NewGuid();

            try
            {
                using var client = await GetMediaServicesClientAsync();
                var inputAsset = await CreateAssetAsync(client, MediaAssetType.Input, assetName, timestamp, assetUniqueGuid);
                var file = await SaveAssetAsync(client, fileStream, inputAsset);
                var outputAsset = await CreateAssetAsync(client, MediaAssetType.Output, assetName, timestamp, assetUniqueGuid);
                var transform = await GetOrCreateTransformAsync(client);
                var job = await SubmitJobAsync(client, transform.Name, inputAsset.Name, outputAsset.Name);
                await MarkVideoAsQueuedAsync(file, outputAsset, transform, job);
                var mediaServiceJobState = ConvertToMediaServiceJobState(job.State.ToString());

                return new AudioAssetCreatedReadServiceModel
                {
                    FileUrl = file.Url,
                    FileExternalId = file.ExternalId,
                    InputAssetName = inputAsset.Name,
                    InputContainerName = inputAsset.Container,
                    JobName = job.Name,
                    JobState = mediaServiceJobState,
                    OutputAssetName = outputAsset.Name,
                    OutputContainerName = outputAsset.Container,
                    TransformName = transform.Name,
                };
            }
            catch (Exception exception)
            {
                LogApiError(assetName, exception);

                throw;
            }
        }

        public async Task DeleteAudioAssetAsync(string assetName)
        {
            try
            {
                using var client = await GetMediaServicesClientAsync();
                await DeleteAssetAsync(client, assetName);
                await AudioFileService.RemoveAudioFileAsync(assetName);
            }
            catch (Exception exception)
            {
                LogApiError(assetName, exception);

                throw;
            }
        }

        public async Task<AudioAssetPublishedReadServiceModel> PublishAudioAssetAsync(string assetName)
        {
            try
            {
                using var client = await GetMediaServicesClientAsync();
                var streamingLocator = await CreateStreamingLocatorAsync(client, assetName, StreamingLocatorLifetime);
                var streamingEndpoint = await GetStreamingEndpointAsync(client, DefaultStreamingEndpointName);
                var (downloadUris, streamingUris) = await GetStreamingUrisAsync(client, streamingEndpoint, streamingLocator.Name);

                return new AudioAssetPublishedReadServiceModel
                {
                    DownloadUris = downloadUris,
                    StreamingUris = streamingUris,
                };
            }
            catch (Exception exception)
            {
                LogApiError(assetName, exception);

                throw;
            }
        }

        #endregion

        #region Private methods

        private async Task<IAzureMediaServicesClient> GetMediaServicesClientAsync()
        {
            Logger.LogDebug("Connecting to Azure Media Services.");

            var mediaServicesClient = await CreateMediaServicesClientAsync();

            Logger.LogDebug("Successfully connected to Azure Media Services.");

            return mediaServicesClient;
        }

        private async Task<IAzureMediaServicesClient> CreateMediaServicesClientAsync()
        {
            var clientCredential = new ClientCredential(AzureMediaServicesConfig.AadClientId, AzureMediaServicesConfig.AadSecret);
            var serviceCredentials = await ServiceClientCredentialsService.LogInAsync(AzureMediaServicesConfig.AadTenantId, clientCredential);

            return new AzureMediaServicesClient(AzureMediaServicesConfig.ArmEndpoint, serviceCredentials)
            {
                SubscriptionId = AzureMediaServicesConfig.SubscriptionId,
            };
        }

        private async Task<Asset> CreateAssetAsync(IAzureMediaServicesClient client, MediaAssetType mediaAssetType, string assetName, DateTime timestamp, Guid guid)
        {
            Logger.LogDebug("Creating {0} asset.", mediaAssetType);

            var formattedAssetName = MediaAssetNameFormatter.Format(assetName, mediaAssetType, timestamp, guid);
            var assetContainerName = MediaAssetContainerNameFormatter.Format(mediaAssetType, timestamp, guid);

            // Check if an asset already exists.
            var existingAsset = await client.Assets.GetAsync(
                ResourceGroupName,
                AccountName,
                formattedAssetName);
            if (existingAsset is not null)
            {
                throw new InvalidOperationException($"{mediaAssetType} media asset {formattedAssetName} already exists.");
            }

            var asset = await client.Assets.CreateOrUpdateAsync(
                ResourceGroupName,
                AccountName,
                formattedAssetName,
                new Asset
                {
                    Container = assetContainerName,
                });

            Logger.LogDebug("{0} asset created.", mediaAssetType);

            return asset;
        }

        private async Task<FileReadServiceModel> SaveAssetAsync(
            IAzureMediaServicesClient client,
            Stream fileStream,
            Asset asset)
        {
            Logger.LogDebug("Saving new file for media asset.");

            var response = await client.Assets.ListContainerSasAsync(
                ResourceGroupName,
                AccountName,
                asset.Name,
                permissions: AssetContainerPermission.ReadWrite,
                expiryTime: DateTime.UtcNow.AddHours(4).ToUniversalTime());

            var containerUri = new Uri(response.AssetContainerSasUrls.First());
            var blobContainerClient = new BlobContainerClient(containerUri);

            var file = await AudioFileService.SaveNewAudioFileAsync(
               asset.Name,
               blobContainerClient.Name,
               fileStream,
               asset.Name);

            Logger.LogDebug("New file for media asset saved.");

            return file;
        }

        private async Task<Transform> GetOrCreateTransformAsync(IAzureMediaServicesClient client)
        {
            var transformName = EncodingTransformName;

            Logger.LogDebug("Obtaining transform {0}.", transformName);

            var transform = await client.Transforms.GetAsync(ResourceGroupName, AccountName, transformName);
            if (transform is null)
            {
                transform = await CreateTransformAsync(client, transformName);
            }
            else
            {
                Logger.LogDebug("Transform {0} found.", transformName);
            }

            return transform;
        }

        private async Task<Transform> CreateTransformAsync(
            IAzureMediaServicesClient client,
            string transformName)
        {
            Logger.LogDebug("Transform {0} not found. Creating new.", transformName);

            var output = new List<TransformOutput>
            {
                new TransformOutput
                {
                    OnError = OnErrorType.StopProcessingJob,
                    Preset = new BuiltInStandardEncoderPreset()
                    {
                        PresetName = EncoderPreset,
                    },
                    RelativePriority = Priority.Normal,
                },
            };

            var transform = await client.Transforms.CreateOrUpdateAsync(
                ResourceGroupName,
                AccountName,
                transformName,
                output);

            Logger.LogDebug("Transform {0} created.", transformName);

            return transform;
        }

        private async Task<Job> SubmitJobAsync(
            IAzureMediaServicesClient client,
            string transformName,
            string inputAssetName,
            string outputAssetName)
        {
            Logger.LogDebug("Creating job using transform {0}.", transformName);

            var jobName = FormatJobName(transformName);
            var jobInput = new JobInputAsset(assetName: inputAssetName);
            var jobOutputs = new List<JobOutput>
            {
                new JobOutputAsset(outputAssetName),
            };

            var job = await client.Jobs.CreateAsync(
                ResourceGroupName,
                AccountName,
                transformName,
                jobName,
                new Job
                {
                    Input = jobInput,
                    Outputs = jobOutputs,
                });

            Logger.LogDebug("New job using transform {0} created successfully.", transformName);

            return job;
        }

        private async Task<StreamingLocator> CreateStreamingLocatorAsync(IAzureMediaServicesClient client, string assetName, TimeSpan locatorLifetime)
        {
            var streamingPolicyName = StreamingPolicy;
            var locatorName = FormatStreamingLocatorName(streamingPolicyName);

            Logger.LogDebug("Creating streaming locator with {0} policy.", streamingPolicyName);

            var streamingLocator = await client.StreamingLocators.CreateAsync(
                ResourceGroupName,
                AccountName,
                locatorName,
                new StreamingLocator
                {
                    AssetName = assetName,
                    StreamingPolicyName = streamingPolicyName,
                    EndTime = DateTime.UtcNow.Add(locatorLifetime),
                });

            Logger.LogDebug("Streaming locator with {0} policy created successfully.", streamingPolicyName);

            return streamingLocator;
        }

        private async Task<(IList<Uri> DownloadUris, IList<Uri> StreamingUris)> GetStreamingUrisAsync(
            IAzureMediaServicesClient client,
            StreamingEndpoint streamingEndpoint,
            string locatorName)
        {
            Logger.LogDebug("Obtaining streaming locator paths.");

            var paths = await client.StreamingLocators.ListPathsAsync(
                ResourceGroupName,
                AccountName,
                locatorName);

            Logger.LogDebug("Streaming locator paths obtained.");

            var downloadUris = paths.DownloadPaths
                .Select(path => BuildEndpointUri(path, streamingEndpoint))
                .ToList();
            var streamingUris = paths.StreamingPaths
                .Select(streamingPath => BuildEndpointUri(streamingPath.Paths[0], streamingEndpoint))
                .ToList();

            return (downloadUris, streamingUris);
        }

        private async Task<StreamingEndpoint> GetStreamingEndpointAsync(IAzureMediaServicesClient client, string endpointName)
        {
            Logger.LogDebug("Obtaining {0} streaming endpoint.", endpointName);

            var streamingEndpoint = await client.StreamingEndpoints.GetAsync(
                ResourceGroupName,
                AccountName,
                endpointName);
            if (streamingEndpoint is null)
            {
                throw new InvalidOperationException($"Streaming endpoint {endpointName} was not found.");
            }

            Logger.LogDebug("{0} streaming endpoint obtained.", endpointName);

            if (streamingEndpoint.ResourceState != StreamingEndpointResourceState.Running)
            {
                throw new InvalidOperationException($"Streaming endpoint {endpointName} is not running.");
            }

            return streamingEndpoint;
        }

        private async Task MarkVideoAsQueuedAsync(FileReadServiceModel file, Asset outputAsset, Transform transform, Job job)
        {
            var writeServiceModel = new AudioFileQueuedWriteServiceModel
            {
                ExternalId = file.ExternalId,
                JobName = job.Name,
                OutputAssetName = outputAsset.Name,
                OutputContainerName = outputAsset.Container,
                TransformName = transform.Name,
            };

            var result = await AudioFileService.MarkAsQueuedAsync(writeServiceModel);
            if (result.IsFailure)
            {
                throw new InvalidOperationException(result.Error);
            }
        }

        private async Task DeleteAssetAsync(IAzureMediaServicesClient client, string assetName)
        {
            Logger.LogDebug("Deleting asset {0}.", assetName);

            await client.Assets.DeleteAsync(
                ResourceGroupName,
                AccountName,
                assetName);

            Logger.LogDebug("Asset {0} deleted successfully.", assetName);
        }

        private Uri BuildEndpointUri(string path, StreamingEndpoint streamingEndpoint)
        {
            return new UriBuilder
            {
                Scheme = Uri.UriSchemeHttps,
                Host = streamingEndpoint.HostName,
                Path = path,
            }.Uri;
        }

        private MediaServicesJobState ConvertToMediaServiceJobState(string jobStateName)
        {
            var result = MediaServiceJobStateConverter.Convert(jobStateName);
            if (result.IsFailure)
            {
                throw new InvalidOperationException(
                    $"Unable to convert {jobStateName} to one of job states " +
                    $"defined in {typeof(MediaServicesJobState).FullName}.");
            }

            return result.Value;
        }

        private string FormatJobName(string transformName)
        {
            return $"{transformName}_{DateTime.UtcNow.ToUnixTimestamp()}_{Guid.NewGuid()}";
        }

        private string FormatStreamingLocatorName(PredefinedStreamingPolicy streamingPolicyName)
        {
            return $"{streamingPolicyName}_{Guid.NewGuid()}";
        }

        private void LogApiError(string fileName, Exception exception)
        {
            if (exception.GetBaseException() is ApiErrorException apiException)
            {
                Logger.LogError(
                    "An error occured while sending request to Azure Media Services for asset {0}. " +
                    "API call failed with error code {1} and message '{2}'.",
                    fileName,
                    apiException.Body.Error.Code,
                    apiException.Body.Error.Message);
            }
            else
            {
                Logger.LogError("An error occured while sending request to Azure Media Services for asset {0}. ", fileName);
            }
        }

        #endregion
    }
}
