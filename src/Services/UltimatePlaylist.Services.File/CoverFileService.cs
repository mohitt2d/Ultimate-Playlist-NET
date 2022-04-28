#region Usings

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
    public class CoverFileService : FileService<CoverFileEntity>, ICoverFileService
    {
        #region Constructor(s)

        public CoverFileService(
            Lazy<IMapper> mapperProvider,
            Lazy<IRepository<CoverFileEntity>> fileRepositoryProvider,
            Lazy<ICloudStorageService> cloudStorageServiceProvider,
            Lazy<IFileNameFormatterService> fileNameFormatterServiceProvider)
            : base(mapperProvider, fileRepositoryProvider, cloudStorageServiceProvider, fileNameFormatterServiceProvider)
        {
        }

        #endregion

        #region Public methods

        public Task<FileReadServiceModel> SaveNewFileAsync(Stream fileStream, string fileName)
        {
            var campaignMediaFileName = GetFileNameForMedia(fileName);
            return SaveNewFileAsync(new CoverFileEntity(), ContainerType.Covers, fileStream, campaignMediaFileName);
        }

        public async Task RemoveCoverAsync(Guid externalId)
        {
            var fileEntity = await FileRepository.FirstOrDefaultAsync(new CoverFileSpecification().ByExternalId(externalId))
                ?? throw new NotFoundException(ErrorType.FileNotFound.ToString());
            await RemoveFileAsync(fileEntity);
            await FileRepository.DeleteAsync(new CoverFileSpecification().ById(fileEntity.Id));
        }

        #endregion

        #region Private methods

        private string GetFileNameForMedia(string fileName) => $"{Guid.NewGuid()}/{FileNameFormatterService.Format(fileName)}";

        #endregion
    }
}
