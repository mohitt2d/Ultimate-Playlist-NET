#region Usings

using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UltimatePlaylist.Common.Config;
using UltimatePlaylist.Services.Common.Interfaces.Files;
using UltimatePlaylist.Services.Common.Models.Files;

#endregion

namespace UltimatePlaylist.Services.File
{
    public class AzureCloudStorageService : ICloudStorageService
    {
        #region Private fields

        private readonly Lazy<IOptions<AzureStorageConfig>> AzureStorageConfigProvider;
        private readonly Lazy<ILogger<AzureCloudStorageService>> LoggerProvider;

        #endregion

        #region Constructor(s)

        public AzureCloudStorageService(
            Lazy<IOptions<AzureStorageConfig>> azureOptionsProvider,
            Lazy<ILogger<AzureCloudStorageService>> loggerProvider)
        {
            LoggerProvider = loggerProvider;
            AzureStorageConfigProvider = azureOptionsProvider;
        }

        #endregion

        #region Properties

        private ILogger Logger => LoggerProvider.Value;

        private AzureStorageConfig AzureStorageConfig => AzureStorageConfigProvider.Value.Value;

        #endregion

        #region Public methods

        public async Task DeleteFileIfExistsAsync(string containerName, string fileName)
        {
            var blobServiceClient = GetBlobServiceClient();
            var blobContainerClient = await GetBlobContainerClientAsync(blobServiceClient, containerName);
            var blobClient = GetBlobClient(blobContainerClient, fileName);

            await blobClient.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
        }

        public async Task<FileUploadInfoReadServiceModel> UploadFileAsync(Stream fileStream, string containerName, string fileName)
        {
            var blobServiceClient = GetBlobServiceClient();
            var blobContainerClient = await GetBlobContainerClientAsync(blobServiceClient, containerName);
            var blobClient = GetBlobClient(blobContainerClient, fileName);

            return await UploadToBloadAsync(blobClient, fileStream);
        }

        public async Task<string> GetSecurityLinkAsync(string containerName, string fileName)
        {
            var blobServiceClient = GetBlobServiceClient();
            var blobContainerClient = await GetBlobContainerClientAsync(blobServiceClient, containerName);
            var blobClient = GetBlobClient(blobContainerClient, fileName);

            return blobClient
                .GenerateSasUri(BlobSasPermissions.Read, DateTimeOffset.Now.AddHours(1))
                .AbsoluteUri;
        }

        #endregion

        #region Private methods

        private async Task<FileUploadInfoReadServiceModel> UploadToBloadAsync(BlobClient blobClient, Stream fileStream)
        {
            try
            {
                await blobClient.DeleteIfExistsAsync();
                await blobClient.UploadAsync(fileStream);
            }
            catch (Exception exception)
            {
                throw new Exception("Unexpected problem while uploading file.", exception);
            }

            return new FileUploadInfoReadServiceModel
            {
                Container = blobClient.BlobContainerName,
                Uri = blobClient.Uri,
            };
        }

        private BlobClient GetBlobClient(BlobContainerClient containerClient, string fileName)
        {
            try
            {
                return containerClient.GetBlobClient(fileName);
            }
            catch (Exception exception)
            {
                Logger.LogError("Unable to obtain blob client from blob container client.");

                throw new Exception($"Unexpected problem while creating file {fileName}.", exception);
            }
        }

        private async Task<BlobContainerClient> GetBlobContainerClientAsync(BlobServiceClient blobServiceClient, string containerName)
        {
            try
            {
                var container = blobServiceClient.GetBlobContainerClient(containerName);
                await container.CreateIfNotExistsAsync();
                await container.SetAccessPolicyAsync(PublicAccessType.BlobContainer);

                return container;
            }
            catch (Exception exception)
            {
                Logger.LogError("Unable to obtain blob container client from blob service.");

                throw new Exception("Unexpected error while creating cloud client.", exception);
            }
        }

        private BlobServiceClient GetBlobServiceClient()
        {
            try
            {
                return new BlobServiceClient(AzureStorageConfig.ConnectionString);
            }
            catch (Exception exception)
            {
                throw new Exception("Unexpected error while creating cloud client.", exception);
            }
        }

        #endregion
    }
}
