#region Usings

using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Common.Exceptions;
using UltimatePlaylist.Database.Infrastructure.Entities.File;
using UltimatePlaylist.Database.Infrastructure.Entities.File.Specifications;
using UltimatePlaylist.Database.Infrastructure.Repositories.Interfaces;
using UltimatePlaylist.Services.Common.Interfaces.Files;
using UltimatePlaylist.Services.Common.Models.Files;

#endregion

namespace UltimatePlaylist.Services.File
{
    public class AvatarFileService : FileService<AvatarFileEntity>, IAvatarFileService
    {
        #region Constructor(s)

        public AvatarFileService(
            Lazy<IMapper> mapperProvider,
            Lazy<IRepository<AvatarFileEntity>> fileRepositoryProvider,
            Lazy<ICloudStorageService> cloudStorageServiceProvider,
            Lazy<IFileNameFormatterService> fileNameFormatterServiceProvider)
            : base(mapperProvider, fileRepositoryProvider, cloudStorageServiceProvider, fileNameFormatterServiceProvider)
        {
        }

        #endregion

        #region Public methods

        public Task<FileReadServiceModel> SaveNewAvatarFileAsync(Stream fileStream, string fileName)
        {
            var campaignMediaFileName = GetFileNameForMedia(fileName);
            return SaveNewFileAsync(new AvatarFileEntity(), ContainerType.Avatars, fileStream, campaignMediaFileName);
        }

        public async Task RemoveAvatarAsync(Guid externalId)
        {
            var fileEntity = await FileRepository.FirstOrDefaultAsync(new AvatarFileSpecification().ByExternalId(externalId))
                ?? throw new NotFoundException(ErrorType.FileNotFound.ToString());
            await RemoveFileAsync(fileEntity);
            await FileRepository.DeleteAsync(new AvatarFileSpecification().ById(fileEntity.Id));
        }

        #endregion

        #region Private methods

        private string GetFileNameForMedia(string fileName) => $"{Guid.NewGuid()}/{FileNameFormatterService.Format(fileName)}";

        #endregion
    }
}
