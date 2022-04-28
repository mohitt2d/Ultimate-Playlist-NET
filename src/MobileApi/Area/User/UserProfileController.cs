#region Usings

using System;
using System.Threading.Tasks;
using AutoMapper;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using UltimatePlaylist.Common.Config;
using UltimatePlaylist.Common.Const;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Common.Mvc.Attributes;
using UltimatePlaylist.Common.Mvc.Controllers;
using UltimatePlaylist.MobileApi.Models.UserProfile;
using UltimatePlaylist.Services.Common.Interfaces.Profile;
using UltimatePlaylist.Services.Common.Models.Profile;
using static UltimatePlaylist.Common.Mvc.Consts.Consts;

#endregion

namespace UltimatePlaylist.MobileApi.Area.User
{
    [Area("Profile")]
    [Route("[controller]")]
    [AuthorizeRole(UserRole.User)]
    [ApiExplorerSettings(GroupName = MobileApiGroups.User)]
    public class UserProfileController : BaseControllerWithAuthentication
    {
        #region Private Members

        private readonly Lazy<IMapper> MapperProvider;
        private readonly Lazy<IUserProfileService> UserProfileServiceProvider;
        private readonly FilesConfig FilesConfig;

        #endregion

        #region Constructor(s)

        public UserProfileController(
            Lazy<IMapper> mapperProvider,
            Lazy<IUserProfileService> userProfileServiceProvider,
            IOptions<FilesConfig> filesConfig)
        {
            MapperProvider = mapperProvider;
            UserProfileServiceProvider = userProfileServiceProvider;
            FilesConfig = filesConfig.Value;
        }

        #endregion

        #region Private Properites

        private IMapper Mapper => MapperProvider.Value;

        private IUserProfileService UserProfileService => UserProfileServiceProvider.Value;

        #endregion

        #region GET

        [HttpGet("info")]
        [ProducesEnvelope(typeof(UserProfileInfoResponseModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserInfoAsync()
        {
            return await UserProfileService.GetUserInfoAsync(XUserExternalId)
               .Map(userProfileInfo => Mapper.Map<UserProfileInfoResponseModel>(userProfileInfo))
               .Finally(BuildEnvelopeResult);
        }

        #endregion

        #region POST

        [HttpPost("add-update-pin")]
        [ProducesEmptyEnvelope(StatusCodes.Status200OK)]
        public async Task<IActionResult> AddUpdatePinAsync(UserPinRequestModel userPinRequestModel)
        {
            var writeServiceModel = Mapper.Map<UserPinWriteServiceModel>(userPinRequestModel);

            return await UserProfileService.AddOrUpdatePinAsync(XUserExternalId, writeServiceModel)
               .Finally(BuildEnvelopeResult);
        }

        [HttpPost("deactivate")]
        [ProducesEmptyEnvelope(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeactivateUser()
        {
            return await UserProfileService.DeactivateUserAsync(XUserExternalId)
               .Finally(BuildEnvelopeResult);
        }

        [HttpPost("check-pin")]
        [ProducesEmptyEnvelope(StatusCodes.Status200OK)]
        public async Task<IActionResult> CheckIfPinCorrectAsync(UserPinRequestModel userPinRequestModel)
        {
            var writeServiceModel = Mapper.Map<UserPinWriteServiceModel>(userPinRequestModel);

            return await UserProfileService.IsPinCorrectAsync(XUserExternalId, writeServiceModel)
               .Finally(BuildEnvelopeResult);
        }

        [HttpPost("avatar", Name = "Upload avatar image file")]
        [ProducesEnvelope(typeof(UserAvatarFileResponseModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> UploadAudioFileAsync(IFormFile formfile)
        {
            if (formfile is null)
            {
                return BuildEnvelopeResult(Result.Failure(ErrorMessages.FileNotFound));
            }

            if (formfile.Length > FilesConfig.Avatar.MaxSizeBytes)
            {
                return BuildEnvelopeResult(Result.Failure(ErrorMessages.FileIsTooLarge));
            }

            if (!FilesConfig.Avatar.Formats.Contains(formfile.ContentType))
            {
                return BuildEnvelopeResult(Result.Failure(ErrorMessages.WrongFileFormat));
            }

            return await Result.Success()
                .Bind(async () => await UserProfileService.SetOrUpdateAvatarAsync(XUserExternalId, formfile.OpenReadStream(), formfile.FileName))
                .Map(fileServiceModel => Mapper.Map<UserAvatarFileResponseModel>(fileServiceModel))
                .Finally(BuildEnvelopeResult);
        }

        #endregion

        #region PUT

        [HttpPut("edit")]
        [ProducesEnvelope(typeof(UserProfileInfoResponseModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> EditUserProfileAsync(UserProfileEditRequestModel userProfileEditRequestModel)
        {
            var writeServiceModel = Mapper.Map<EditUserProfileWriteServiceModel>(userProfileEditRequestModel);

            return await UserProfileService.UpdateUserProfileAsync(XUserExternalId, writeServiceModel)
               .Map(userProfileInfo => Mapper.Map<UserProfileInfoResponseModel>(userProfileInfo))
               .Finally(BuildEnvelopeResult);
        }

        #endregion

        #region DELETE

        [HttpDelete("remove-pin")]
        [ProducesEmptyEnvelope(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeletePinAsync()
        {
            return await UserProfileService.RemovePinAsync(XUserExternalId)
               .Finally(BuildEnvelopeResult);
        }

        [HttpDelete("avatar")]
        [ProducesEmptyEnvelope(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteAUserAvatarAsync()
        {
            return await UserProfileService.DeleteUserAvatarAsync(XUserExternalId)
               .Finally(BuildEnvelopeResult);
        }

        #endregion
    }
}
