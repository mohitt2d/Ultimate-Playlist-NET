#region Usings

using System;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using UltimatePlaylist.Common.Config;
using UltimatePlaylist.Common.Const;
using UltimatePlaylist.Common.Mvc.Interface;
using UltimatePlaylist.Database.Infrastructure.Entities.Dsp;
using UltimatePlaylist.Database.Infrastructure.Entities.File;
using UltimatePlaylist.Database.Infrastructure.Entities.Identity;
using UltimatePlaylist.Database.Infrastructure.Repositories.Interfaces;
using UltimatePlaylist.Database.Infrastructure.Specifications.Interfaces;
using UltimatePlaylist.Services.Common.Interfaces.Files;
using UltimatePlaylist.Services.Common.Models.Profile;
using UltimatePlaylist.Services.Personalization;
using UltimatePlaylist.Tests.UnitTests.Users.Services.Builders;
using UltimatePlaylist.Tests.UnitTests.Users.Services.Helpers;
using Xunit;

#endregion

namespace UltimatePlaylist.Tests.UnitTests.Users.Services.UserProfile
{
    public class UserProfileServiceTests
    {
        [Fact]
        public async Task Update_User_Profile_Success()
        {
            // Mock
            var writeServiceModel = new EditUserProfileWriteServiceModel()
            {
                FirstName = "Name1",
                LastName = "LastName1",
                GenderExternalId = Guid.NewGuid(),
                AvatarExternalId = Guid.NewGuid(),
                Email = "test@test",
                PhoneNumber = "1234567891",
                Username = "Name1LastName1",
                ZipCode = "12345",
                BirthDate = DateTime.UtcNow.AddYears(-18),
                IsSmsPromotionalNotificationEnabled = true,
            };

            var user = new UserEntityBuilder()
                .WithExternalId(Guid.NewGuid())
                .WithUserDsps(isSpotifyAdded: true, isAppleMusicAdded: true)
                .WithUserGender()
                .WithAvatar()
                .Build();

            var userUpdated = new UserEntityBuilder()
                .ExpectedVauleForUpdateUserProfile(user.ExternalId, writeServiceModel)
                .BuildExpectedResponse();

            var userProfileServicesMock = TestsHelper.GetUserProfileMocks(user, userUpdated, writeServiceModel);

            var userProfileServiceMock = new UserProfileService(
                new Lazy<UserManager<User>>(userProfileServicesMock.UserManagerMock.Object),
                new Lazy<IRepository<User>>(userProfileServicesMock.UserRepositoryMock.Object),
                new Lazy<IReadOnlyRepository<UserDspEntity>>(userProfileServicesMock.UserDspRepositoryMock.Object),
                new Lazy<IAvatarFileService>(userProfileServicesMock.AvatarFileServiceMock.Object),
                new Lazy<IReadOnlyRepository<AvatarFileEntity>>(userProfileServicesMock.AvatarRepositoryMock.Object),
                new Lazy<IReadOnlyRepository<GenderEntity>>(userProfileServicesMock.GenderRepositoryMock.Object),
                new Lazy<IMapper>(userProfileServicesMock.MapperMock.Object),
                userProfileServicesMock.EmailConfig,
                new Lazy<IBackgroundJobClient>(userProfileServicesMock.BackgroundJobClientMock.Object),
                new Lazy<IUserBlacklistTokenStore>(userProfileServicesMock.UserBlacklistTokenStoreMock.Object));

            // Act
            var result = await userProfileServiceMock.UpdateUserProfileAsync(user.ExternalId, userProfileServicesMock.EditUserProfileWriteServiceMockModel);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.UserName.Should().Be(writeServiceModel.Username);
            result.Value.FirstName.Should().Be(writeServiceModel.FirstName);
            result.Value.LastName.Should().Be(writeServiceModel.LastName);
            result.Value.Email.Should().Be(writeServiceModel.Email);
            result.Value.PhoneNumber.Should().Be(writeServiceModel.PhoneNumber);
            result.Value.Gender.ExternalId.Should().Be(writeServiceModel.GenderExternalId);
            result.Value.Avatar.ExternalId.Should().Be(writeServiceModel.AvatarExternalId.Value);
            result.Value.BirthDate.Should().Be(writeServiceModel.BirthDate);
            result.Value.ZipCode.Should().Be(writeServiceModel.ZipCode);
            result.Value.IsSmsPromotionalNotificationEnabled.Should().Be(writeServiceModel.IsSmsPromotionalNotificationEnabled);
        }

        [Fact]
        public async Task Update_User_Profile_Failed_UserName_Occupied()
        {
            // Mock
            var writeServiceModel = new EditUserProfileWriteServiceModel()
            {
                FirstName = "Name1",
                LastName = "LastName1",
                GenderExternalId = Guid.NewGuid(),
                AvatarExternalId = Guid.NewGuid(),
                Email = "test@test",
                PhoneNumber = "1234567891",
                Username = "Name1LastName1",
                ZipCode = "12345",
                BirthDate = DateTime.UtcNow.AddYears(-18),
                IsSmsPromotionalNotificationEnabled = true,
            };

            var user = new UserEntityBuilder()
                .WithExternalId(Guid.NewGuid())
                .WithUserDsps(isSpotifyAdded: true, isAppleMusicAdded: true)
                .WithUserGender()
                .WithAvatar()
                .Build();

            var userUpdated = new UserEntityBuilder()
                .ExpectedVauleForUpdateUserProfile(user.ExternalId, writeServiceModel)
                .BuildExpectedResponse();

            var userProfileServicesMock = TestsHelper.GetUserProfileMocks(user, userUpdated, writeServiceModel);

            // Change default mock action
            userProfileServicesMock.UserManagerMock
                .Setup(userManager =>
                userManager.FindByNameAsync(writeServiceModel.Username))
                .Returns(Task.FromResult(new User() { ExternalId = Guid.NewGuid() }));

            var userProfileServiceMock = new UserProfileService(
                new Lazy<UserManager<User>>(userProfileServicesMock.UserManagerMock.Object),
                new Lazy<IRepository<User>>(userProfileServicesMock.UserRepositoryMock.Object),
                new Lazy<IReadOnlyRepository<UserDspEntity>>(userProfileServicesMock.UserDspRepositoryMock.Object),
                new Lazy<IAvatarFileService>(userProfileServicesMock.AvatarFileServiceMock.Object),
                new Lazy<IReadOnlyRepository<AvatarFileEntity>>(userProfileServicesMock.AvatarRepositoryMock.Object),
                new Lazy<IReadOnlyRepository<GenderEntity>>(userProfileServicesMock.GenderRepositoryMock.Object),
                new Lazy<IMapper>(userProfileServicesMock.MapperMock.Object),
                userProfileServicesMock.EmailConfig,
                new Lazy<IBackgroundJobClient>(userProfileServicesMock.BackgroundJobClientMock.Object),
                new Lazy<IUserBlacklistTokenStore>(userProfileServicesMock.UserBlacklistTokenStoreMock.Object));

            // Act
            var result = await userProfileServiceMock.UpdateUserProfileAsync(user.ExternalId, userProfileServicesMock.EditUserProfileWriteServiceMockModel);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(ErrorMessages.UsernameTaken);
        }

        [Fact]
        public async Task Update_User_Profile_Failed_Avatar_File_Not_Found()
        {
            // Mock
            var writeServiceModel = new EditUserProfileWriteServiceModel()
            {
                FirstName = "Name1",
                LastName = "LastName1",
                GenderExternalId = Guid.NewGuid(),
                AvatarExternalId = Guid.NewGuid(),
                Email = "test@test",
                PhoneNumber = "1234567891",
                Username = "Name1LastName1",
                ZipCode = "12345",
                BirthDate = DateTime.UtcNow.AddYears(-18),
                IsSmsPromotionalNotificationEnabled = true,
            };

            var user = new UserEntityBuilder()
                .WithExternalId(Guid.NewGuid())
                .WithUserDsps(isSpotifyAdded: true, isAppleMusicAdded: true)
                .WithUserGender()
                .Build();

            var userUpdated = new UserEntityBuilder()
                .ExpectedVauleForUpdateUserProfile(user.ExternalId, writeServiceModel)
                .BuildExpectedResponse();

            var userProfileServicesMock = TestsHelper.GetUserProfileMocks(user, userUpdated, writeServiceModel);

            // Change default mock action
            userProfileServicesMock.AvatarRepositoryMock.Reset();

            var userProfileServiceMock = new UserProfileService(
                new Lazy<UserManager<User>>(userProfileServicesMock.UserManagerMock.Object),
                new Lazy<IRepository<User>>(userProfileServicesMock.UserRepositoryMock.Object),
                new Lazy<IReadOnlyRepository<UserDspEntity>>(userProfileServicesMock.UserDspRepositoryMock.Object),
                new Lazy<IAvatarFileService>(userProfileServicesMock.AvatarFileServiceMock.Object),
                new Lazy<IReadOnlyRepository<AvatarFileEntity>>(userProfileServicesMock.AvatarRepositoryMock.Object),
                new Lazy<IReadOnlyRepository<GenderEntity>>(userProfileServicesMock.GenderRepositoryMock.Object),
                new Lazy<IMapper>(userProfileServicesMock.MapperMock.Object),
                userProfileServicesMock.EmailConfig,
                new Lazy<IBackgroundJobClient>(userProfileServicesMock.BackgroundJobClientMock.Object),
                new Lazy<IUserBlacklistTokenStore>(userProfileServicesMock.UserBlacklistTokenStoreMock.Object));

            // Act
            var result = await userProfileServiceMock.UpdateUserProfileAsync(user.ExternalId, userProfileServicesMock.EditUserProfileWriteServiceMockModel);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(ErrorMessages.CannotSetUserAvatar);
        }

        [Fact]
        public async Task Update_User_Profile_Failed_Save_Updated_User_Data()
        {
            // Mock
            var writeServiceModel = new EditUserProfileWriteServiceModel()
            {
                FirstName = "Name1",
                LastName = "LastName1",
                GenderExternalId = Guid.NewGuid(),
                AvatarExternalId = Guid.NewGuid(),
                Email = "test@test",
                PhoneNumber = "1234567891",
                Username = "Name1LastName1",
                ZipCode = "12345",
                BirthDate = DateTime.UtcNow.AddYears(-18),
                IsSmsPromotionalNotificationEnabled = true,
            };

            var user = new UserEntityBuilder()
                .WithExternalId(Guid.NewGuid())
                .WithUserDsps(isSpotifyAdded: true, isAppleMusicAdded: true)
                .WithUserGender()
                .WithAvatar()
                .Build();

            var userUpdated = new UserEntityBuilder()
                .ExpectedVauleForUpdateUserProfile(user.ExternalId, writeServiceModel)
                .BuildExpectedResponse();

            var userProfileServicesMock = TestsHelper.GetUserProfileMocks(user, userUpdated, writeServiceModel);

            // Change default mock action
            userProfileServicesMock.UserRepositoryMock.Reset();
            userProfileServicesMock.UserRepositoryMock.Setup(userRepository =>
                userRepository.FirstOrDefaultAsync(It.IsAny<ISpecification<User>>()))
                .Returns(Task.FromResult(user));

            var userProfileServiceMock = new UserProfileService(
                new Lazy<UserManager<User>>(userProfileServicesMock.UserManagerMock.Object),
                new Lazy<IRepository<User>>(userProfileServicesMock.UserRepositoryMock.Object),
                new Lazy<IReadOnlyRepository<UserDspEntity>>(userProfileServicesMock.UserDspRepositoryMock.Object),
                new Lazy<IAvatarFileService>(userProfileServicesMock.AvatarFileServiceMock.Object),
                new Lazy<IReadOnlyRepository<AvatarFileEntity>>(userProfileServicesMock.AvatarRepositoryMock.Object),
                new Lazy<IReadOnlyRepository<GenderEntity>>(userProfileServicesMock.GenderRepositoryMock.Object),
                new Lazy<IMapper>(userProfileServicesMock.MapperMock.Object),
                userProfileServicesMock.EmailConfig,
                new Lazy<IBackgroundJobClient>(userProfileServicesMock.BackgroundJobClientMock.Object),
                new Lazy<IUserBlacklistTokenStore>(userProfileServicesMock.UserBlacklistTokenStoreMock.Object));

            // Act
            var result = await userProfileServiceMock.UpdateUserProfileAsync(user.ExternalId, userProfileServicesMock.EditUserProfileWriteServiceMockModel);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(ErrorMessages.CannotUpdateUserProfile);
        }
    }
}
