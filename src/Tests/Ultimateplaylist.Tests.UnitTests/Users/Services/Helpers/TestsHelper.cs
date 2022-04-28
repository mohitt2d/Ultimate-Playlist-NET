#region Usings

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using UltimatePlaylist.Common.Config;
using UltimatePlaylist.Common.Mvc.Interface;
using UltimatePlaylist.Database.Infrastructure.Entities.Dsp;
using UltimatePlaylist.Database.Infrastructure.Entities.File;
using UltimatePlaylist.Database.Infrastructure.Entities.Identity;
using UltimatePlaylist.Database.Infrastructure.Repositories.Interfaces;
using UltimatePlaylist.Database.Infrastructure.Specifications.Interfaces;
using UltimatePlaylist.Services.Common.Interfaces.Files;
using UltimatePlaylist.Services.Common.Interfaces.Identity;
using UltimatePlaylist.Services.Common.Models.CommonData;
using UltimatePlaylist.Services.Common.Models.Files;
using UltimatePlaylist.Services.Common.Models.Identity;
using UltimatePlaylist.Services.Common.Models.Profile;
using UltimatePlaylist.Services.Identity.Services.Users;
using UltimatePlaylist.Tests.UnitTests.Users.Services.MockModels;
using UltimatePlaylist.Tests.UnitTests.Users.Services.MockModels;

#endregion

namespace UltimatePlaylist.Tests.UnitTests.Users.Services.Helpers
{
    public static class TestsHelper
    {
        #region Public Method(s)

        public static RegistrationMockModel GetRegistrationMocks(User mockedUser)
        {
            return new RegistrationMockModel()
            {
                UserRegistrationWriteServiceMockModel = new Mock<UserRegistrationWriteServiceModel>(),
                UserMock = mockedUser,
                GenderEntityMock = mockedUser.Gender,
                LoggerMock = new Mock<ILogger<UserIdentityService>>(),
                UserManagerMock = GetMockedUserManager(mockedUser),
                MapperMock = GetMockedMapper(mockedUser),
                BackgroundJobClientMock = new Mock<IBackgroundJobClient>(),
                UserRetrieverServiceMock = new Mock<IUserRetrieverService>(),
                UserRepositoryMock = new Mock<IRepository<User>>(),
                GenderRepositoryMock = GetMockedGenderRepository(mockedUser.Gender),
                AuthConfigMock = GetMockedAuthConfigOptions(),
                EamilConfigMock = GetMockedEmailConfigOptions(),
            };
        }

        public static UserProfileMockModel GetUserProfileMocks(
            User mockedUser,
            User mockedExpectedUpdatedUser,
            EditUserProfileWriteServiceModel editUserProfileWriteServiceModel)
        {
            return new UserProfileMockModel()
            {
                EditUserProfileWriteServiceMockModel = editUserProfileWriteServiceModel,
                UserManagerMock = GetMockedUserManager(mockedUser),
                UserRepositoryMock = GetMockedUserRepository(mockedUser),
                UserDspRepositoryMock = GetMockedUserDspRepository(mockedUser.Dsps),
                AvatarFileServiceMock = GetMockedUserAvatarService(mockedUser.AvatarFile),
                GenderRepositoryMock = GetMockedGenderRepository(mockedUser.Gender),
                AvatarRepositoryMock = GetMockedUserAvatarRepository(mockedUser.AvatarFile),
                MapperMock = GetMockedMapper(mockedExpectedUpdatedUser),
                EmailConfig = GetEmailConfig(),
                BackgroundJobClientMock = GetBackgroundJobClient(),
                UserBlacklistTokenStoreMock = new Mock<IUserBlacklistTokenStore>(),
            };
        }

        #endregion

        #region Private Method(s)

        private static Mock<UserManager<User>> GetMockedUserManager(User userModel)
        {
            var store = new Mock<IUserStore<User>>();
            var userManagerMock = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            userManagerMock
                .Setup(userManager =>
                userManager.FindByEmailAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(It.IsAny<User>()));
            userManagerMock
                .Setup(userManager =>
                userManager.FindByNameAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(It.IsAny<User>()));
            userManagerMock
                .Setup(userManager =>
                userManager.CreateAsync(userModel))
                .Returns(Task.FromResult(IdentityResult.Success));
            userManagerMock
               .Setup(userManager =>
               userManager.AddPasswordAsync(It.IsAny<User>(), It.IsAny<string>()))
               .Returns(Task.FromResult(It.IsAny<IdentityResult>()));
            userManagerMock
               .Setup(userManager =>
               userManager.UpdateAsync(It.IsAny<User>()))
               .Returns(Task.FromResult(It.IsAny<IdentityResult>()));
            userManagerMock
               .Setup(userManager =>
               userManager.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
               .Returns(Task.FromResult(IdentityResult.Success));

            return userManagerMock;
        }

        private static Mock<IMapper> GetMockedMapper(User userModel)
        {
            var mapperMock = new Mock<IMapper>();
            mapperMock
            .Setup(mapper =>
            mapper.Map<User>(It.IsAny<UserRegistrationWriteServiceModel>()))
            .Returns(userModel);

            mapperMock
            .Setup(mapper =>
            mapper.Map<UserProfileInfoReadServiceModel>(It.IsAny<User>()))
            .Returns(new UserProfileInfoReadServiceModel()
            {
                FirstName = userModel.Name,
                LastName = userModel.LastName,
                UserName = userModel.UserName,
                Email = userModel.Email,
                PhoneNumber = userModel.PhoneNumber,
                Avatar = new FileReadServiceModel()
                {
                    ExternalId = userModel.AvatarFile.ExternalId,
                    Url = userModel.AvatarFile.Url,
                },
                Gender = new GenderReadServiceModel()
                {
                    ExternalId = userModel.Gender.ExternalId,
                    Name = userModel.Gender.Name,
                },
                ZipCode = userModel.ZipCode,
                BirthDate = userModel.BirthDate,
                IsNotificationEnabled = userModel.IsNotificationEnabled,
                IsSmsPromotionalNotificationEnabled = userModel.IsSmsPromotionalNotificationEnabled,
                IsPinEnabled = !string.IsNullOrEmpty(userModel.Pin),
            });

            return mapperMock;
        }

        private static Mock<IReadOnlyRepository<GenderEntity>> GetMockedGenderRepository(GenderEntity genderModel)
        {
            var genderRepositoryMock = new Mock<IReadOnlyRepository<GenderEntity>>();
            genderRepositoryMock
                .Setup(genderRepository =>
                genderRepository.FirstOrDefaultAsync(It.IsAny<ISpecification<GenderEntity>>()))
                .Returns(Task.FromResult(genderModel));

            return genderRepositoryMock;
        }

        private static AuthConfig GetAuthConfig()
        {
            return new AuthConfig()
            {
                AppleWellKnown = string.Empty,
                JWT = new JWTConfig()
                {
                    Audience = "https://localhost:5001",
                    Issuer = "https://localhost:5001",
                    TokenExpirationTime = new TimeSpan(0, 5, 0),
                    Key = Guid.NewGuid().ToString(),
                },
            };
        }

        private static Mock<IOptions<AuthConfig>> GetMockedAuthConfigOptions()
        {
            var authOptions = new Mock<IOptions<AuthConfig>>();
            authOptions.Setup(ap => ap.Value).Returns(GetAuthConfig());

            return authOptions;
        }

        private static IOptions<EmailConfig> GetEmailConfig()
        {
            return Options.Create<EmailConfig>(new EmailConfig()
            {
                ConfirmationAction = string.Empty,
                SendGridClientKey = string.Empty,
                TemplateRegistrationConfirm = string.Empty,
                TemplateUpdateProfileConfirm = string.Empty,
                FrontendUrl = string.Empty,
                SenderEmail = string.Empty,
                SenderName = string.Empty,
                ResetPasswordAction = string.Empty,
                TemplateResetPassword = string.Empty,
            });
        }

        private static Mock<IOptions<EmailConfig>> GetMockedEmailConfigOptions()
        {
            var authOptions = new Mock<IOptions<EmailConfig>>();
            authOptions.Setup(ap => ap.Value).Returns(GetEmailConfig().Value);

            return authOptions;
        }

        private static Mock<IRepository<User>> GetMockedUserRepository(User userModel)
        {
            var userRepositoryMock = new Mock<IRepository<User>>();
            userRepositoryMock
                .Setup(userRepository =>
                userRepository.FirstOrDefaultAsync(It.IsAny<ISpecification<User>>()))
                .Returns(Task.FromResult(userModel));

            userModel.Updated = DateTime.UtcNow;

            userRepositoryMock
                .Setup(userRepository =>
                userRepository.UpdateAndSaveAsync(It.IsAny<User>(), true))
                .Returns(Task.FromResult(userModel));

            return userRepositoryMock;
        }

        private static Mock<IReadOnlyRepository<UserDspEntity>> GetMockedUserDspRepository(ICollection<UserDspEntity> userDspModel)
        {
            var userDspRepositoryMock = new Mock<IReadOnlyRepository<UserDspEntity>>();
            userDspRepositoryMock
                .Setup(userDspRepository =>
                userDspRepository.FirstOrDefaultAsync(It.IsAny<ISpecification<UserDspEntity>>()))
                .Returns(Task.FromResult(userDspModel.First()));

            userDspRepositoryMock
                .Setup(userDspRepository =>
                userDspRepository.ListAsync(It.IsAny<ISpecification<UserDspEntity>>()))
                .Returns(Task.FromResult((IReadOnlyList<UserDspEntity>)userDspModel));

            return userDspRepositoryMock;
        }

        private static Mock<IAvatarFileService> GetMockedUserAvatarService(AvatarFileEntity avatarModel)
        {
            var fileReadServiceModel = new FileReadServiceModel();

            if (avatarModel != null)
            {
                fileReadServiceModel.ExternalId = avatarModel.ExternalId;
                fileReadServiceModel.Url = avatarModel.Url;
            }

            var userAvatarServiceMock = new Mock<IAvatarFileService>();
            userAvatarServiceMock
                .Setup(userAvatarService =>
                userAvatarService.SaveNewAvatarFileAsync(It.IsAny<Stream>(), It.IsAny<string>()))
                .Returns(Task.FromResult(fileReadServiceModel));

            userAvatarServiceMock
               .Setup(userAvatarService =>
               userAvatarService.RemoveAvatarAsync(It.IsAny<Guid>()));

            return userAvatarServiceMock;
        }

        private static Mock<IReadOnlyRepository<AvatarFileEntity>> GetMockedUserAvatarRepository(AvatarFileEntity avatarEntityModel)
        {
            var avatarFileRepositoryMock = new Mock<IReadOnlyRepository<AvatarFileEntity>>();
            avatarFileRepositoryMock
                .Setup(avatarFileRepository =>
                avatarFileRepository.FirstOrDefaultAsync(It.IsAny<ISpecification<AvatarFileEntity>>()))
                .Returns(Task.FromResult(avatarEntityModel));

            return avatarFileRepositoryMock;
        }

        private static Mock<IBackgroundJobClient> GetBackgroundJobClient()
        {
            var backgroundJobClientMock = new Mock<IBackgroundJobClient>();

            return backgroundJobClientMock;
        }
        #endregion
    }
}
