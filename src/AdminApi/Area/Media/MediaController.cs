#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using UltimatePlaylist.AdminApi.Models.Media;
using UltimatePlaylist.Common.Config;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Common.Mvc.Attributes;
using UltimatePlaylist.Common.Mvc.Controllers;
using UltimatePlaylist.Services.Common.Interfaces.Files;
using UltimatePlaylist.Services.Common.Interfaces.Media;
using static UltimatePlaylist.Common.Mvc.Consts.Consts;

#endregion

namespace UltimatePlaylist.AdminApi.Area.Media
{
    [Area("Media")]
    [Route("[controller]")]
    [ApiExplorerSettings(GroupName = AdminApiGroups.Administrator)]
    public class MediaController : BaseControllerWithAuthentication
    {
        #region Private Members

        private readonly Lazy<IMapper> MapperProvider;
        private readonly Lazy<ICoverFileService> CoverFileServiceProvider;
        private readonly Lazy<IAudioFileService> AudioFileServiceProvider;
        private readonly Lazy<IMediaService> MediaServiceProvider;
        private readonly FilesConfig FilesConfig;

        #endregion

        #region Constructor(s)

        public MediaController(
            Lazy<IMapper> mapperProvider,
            Lazy<ICoverFileService> coverFileServiceProvider,
            Lazy<IAudioFileService> audioFileServiceProvider,
            Lazy<IMediaService> mediaServiceProvider,
            IOptions<FilesConfig> filesConfig)
        {
            MapperProvider = mapperProvider;
            CoverFileServiceProvider = coverFileServiceProvider;
            AudioFileServiceProvider = audioFileServiceProvider;
            MediaServiceProvider = mediaServiceProvider;
            FilesConfig = filesConfig.Value;
        }

        #endregion

        #region Private Properites

        private IMapper Mapper => MapperProvider.Value;

        private ICoverFileService CoverFileService => CoverFileServiceProvider.Value;

        private IAudioFileService AudioFileService => AudioFileServiceProvider.Value;

        private IMediaService MediaService => MediaServiceProvider.Value;

        #endregion

        #region POST

        [HttpPost("cover", Name = "Upload image file")]
        [ProducesEnvelope(typeof(FileResponseModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> UploadAudioFileAsync(IFormFile formfile)
        {
            if (formfile is null)
            {
                return BuildEnvelopeResult(Result.Failure(ErrorType.FileNotFound.ToString()));
            }

            if (formfile.Length > FilesConfig.Cover.MaxSizeBytes)
            {
                return BuildEnvelopeResult(Result.Failure(ErrorType.FileIsTooLarge.ToString()));
            }

            if (!FilesConfig.Cover.Formats.Contains(formfile.ContentType))
            {
                return BuildEnvelopeResult(Result.Failure(ErrorType.WrongFileFormat.ToString()));
            }

            return await Result.Success()
                .Map(async () => await CoverFileService.SaveNewFileAsync(formfile.OpenReadStream(), formfile.FileName))
                .Map(fileServiceModel => Mapper.Map<FileResponseModel>(fileServiceModel))
                .Finally(BuildEnvelopeResult);
        }

        [HttpPost("audio", Name = "Upload audio file")]
        [ProducesEnvelope(typeof(FileResponseModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> UploadVideoAsync(IFormFile formfile)
        {
            if (formfile is null)
            {
                return BuildEnvelopeResult(Result.Failure(ErrorType.FileNotFound.ToString()));
            }

            if (formfile.Length > FilesConfig.Audio.MaxSizeBytes)
            {
                return BuildEnvelopeResult(Result.Failure(ErrorType.FileIsTooLarge.ToString()));
            }

            if (!FilesConfig.Audio.Formats.Contains(formfile.ContentType))
            {
                return BuildEnvelopeResult(Result.Failure(ErrorType.WrongFileFormat.ToString()));
            }

            return await Result.Success()
                .Map(async () => await MediaService.CreateAudioAssetAsync(formfile.OpenReadStream(), formfile.FileName))
                .Map(fileServiceModel => Mapper.Map<FileResponseModel>(fileServiceModel))
                .Finally(BuildEnvelopeResult);
        }

        #endregion
    }
}
