#region Usings

using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using UltimatePlaylist.Database.Infrastructure.Entities.File;
using UltimatePlaylist.Database.Infrastructure.Repositories.Interfaces;
using UltimatePlaylist.Services.Common.Interfaces.Files;
using UltimatePlaylist.Services.Common.Models.Files;

#endregion

namespace UltimatePlaylist.Services.File
{
    public abstract class FileService<TFile> : IFileService<TFile>
        where TFile : BaseFileEntity
    {
        #region Private fields

        private readonly Lazy<IRepository<TFile>> FileRepositoryProvider;
        private readonly Lazy<IMapper> MapperProvider;
        private readonly Lazy<IFileNameFormatterService> FileNameFormatterServiceProvider;
        private readonly Lazy<ICloudStorageService> CloudStorageServiceProvider;

        #endregion

        #region Constructor(s)

        protected FileService(
            Lazy<IMapper> mapperProvider,
            Lazy<IRepository<TFile>> fileRepositoryProvider,
            Lazy<ICloudStorageService> cloudStorageServiceProvider,
            Lazy<IFileNameFormatterService> fileNameFormatterServiceProvider)
        {
            CloudStorageServiceProvider = cloudStorageServiceProvider;
            MapperProvider = mapperProvider;
            FileRepositoryProvider = fileRepositoryProvider;
            FileNameFormatterServiceProvider = fileNameFormatterServiceProvider;
        }

        #endregion

        #region Properties

        protected IFileNameFormatterService FileNameFormatterService => FileNameFormatterServiceProvider.Value;

        protected IRepository<TFile> FileRepository => FileRepositoryProvider.Value;

        protected IMapper Mapper => MapperProvider.Value;

        private ICloudStorageService CloudStorageService => CloudStorageServiceProvider.Value;

        #endregion

        #region Public methods

        public Task<string> GetSecurityLinkAsync(ContainerType containerType, string fileName)
        {
            var containerName = GetContainerName(containerType);

            return GetSecurityLinkAsync(containerName, fileName);
        }

        public Task<string> GetSecurityLinkAsync(string containerName, string fileName)
        {
            return CloudStorageService.GetSecurityLinkAsync(containerName, fileName);
        }

        #endregion

        #region Protected methods

        protected Task<FileReadServiceModel> SaveNewFileAsync(
            TFile newFile,
            ContainerType containerType,
            Stream fileStream,
            string fileName)
        {
            var containerName = GetContainerName(containerType);

            return SaveNewFileAsync(newFile, containerName, fileStream, fileName);
        }

        protected async Task<FileReadServiceModel> SaveNewFileAsync(
            TFile newFile,
            string containerName,
            Stream fileStream,
            string fileName)
        {
            newFile.Container = containerName;
            newFile.Extension = GetFileExtension(fileName);
            newFile.FileName = fileName;

            var fileUploadInfo = await CloudStorageService.UploadFileAsync(fileStream, containerName, fileName);
            FillFileEntityUrl(newFile, fileUploadInfo);
            await FileRepository.AddAsync(newFile);

            return Mapper.Map<FileReadServiceModel>(newFile);
        }

        protected string GetFileExtension(string fileName)
        {
            return Path.GetExtension(fileName);
        }

        protected Task<FileReadServiceModel> UpdateFileAsync(
            ContainerType containerType,
            Stream fileStream,
            string fileName,
            TFile existingFileEntity)
        {
            var containerName = GetContainerName(containerType);

            return UpdateFileAsync(containerName, fileStream, fileName, existingFileEntity);
        }

        protected async Task<FileReadServiceModel> UpdateFileAsync(
            string containerName,
            Stream fileStream,
            string fileName,
            TFile existingFileEntity)
        {
            // Upload new file
            var fileUploadInfo = await CloudStorageService.UploadFileAsync(fileStream, containerName, fileName);

            // Delete old file
            await CloudStorageService.DeleteFileIfExistsAsync(existingFileEntity.Container, existingFileEntity.FileName);

            // Update file entity info
            existingFileEntity.Container = containerName;
            existingFileEntity.Extension = GetFileExtension(fileName);
            existingFileEntity.FileName = fileName;
            FillFileEntityUrl(existingFileEntity, fileUploadInfo);

            await FileRepository.UpdateAndSaveAsync(existingFileEntity);

            return Mapper.Map<FileReadServiceModel>(existingFileEntity);
        }

        protected async Task RemoveFileAsync(
           TFile existingFileEntity)
        {
            await CloudStorageService.DeleteFileIfExistsAsync(existingFileEntity.Container, existingFileEntity.FileName);
        }

        #endregion

        #region Private methods

        private void FillFileEntityUrl(TFile fileEntity, FileUploadInfoReadServiceModel fileUploadInfo)
        {
            fileEntity.Url = fileUploadInfo.Uri.AbsoluteUri;
        }

        private string GetContainerName(ContainerType containerType)
        {
            return containerType.ToString().ToLower();
        }

        #endregion
    }
}
