#region Usings

using System;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using UltimatePlaylist.Common.Const;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Database.Infrastructure.Entities.Identity;
using UltimatePlaylist.Database.Infrastructure.Repositories.Interfaces;
using UltimatePlaylist.Database.Infrastructure.Specifications.Interfaces;
using UltimatePlaylist.Services.Common.Interfaces.Identity;
using UltimatePlaylist.Services.Identity.Services.Users;
using UltimatePlaylist.Tests.UnitTests.Users.Services.Builders;
using UltimatePlaylist.Tests.UnitTests.Users.Services.Helpers;
using Xunit;

#endregion
namespace UltimatePlaylist.Tests.UnitTests.Users.Services.Identity
{
    public class UserIdentityServiceTests
    {
        [Fact]
        public async Task UserRegistration_Success()
        {
            // Mock
            var user = new UserEntityBuilder()
                .WithExternalId(Guid.NewGuid())
                .WithUserDsps(isSpotifyAdded: true, isAppleMusicAdded: true)
                .WithUserGender()
                .WithAvatar()
                .Build();

            var registrationServicesMock = TestsHelper.GetRegistrationMocks(user);
            registrationServicesMock.UserRegistrationWriteServiceMockModel.Object.Username = "Testowy";

            var userIdentityServiceMock = new UserIdentityService(
                new Lazy<ILogger<UserIdentityService>>(registrationServicesMock.LoggerMock.Object),
                new Lazy<UserManager<User>>(registrationServicesMock.UserManagerMock.Object),
                new Lazy<IMapper>(registrationServicesMock.MapperMock.Object),
                new Lazy<IBackgroundJobClient>(registrationServicesMock.BackgroundJobClientMock.Object),
                new Lazy<IUserRetrieverService>(registrationServicesMock.UserRetrieverServiceMock.Object),
                new Lazy<IRepository<User>>(registrationServicesMock.UserRepositoryMock.Object),
                registrationServicesMock.AuthConfigMock.Object,
                registrationServicesMock.EamilConfigMock.Object,
                new Lazy<IReadOnlyRepository<GenderEntity>>(registrationServicesMock.GenderRepositoryMock.Object));

            // Act
            var result = await userIdentityServiceMock.RegisterAsync(registrationServicesMock.UserRegistrationWriteServiceMockModel.Object);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task UserRegistration_Failure_UserEmailExist()
        {
            // Mock
            var user = new UserEntityBuilder()
                .WithExternalId(Guid.NewGuid())
                .WithUserDsps(isSpotifyAdded: true, isAppleMusicAdded: true)
                .WithUserGender()
                .WithAvatar()
                .Build();

            var registrationServicesMock = TestsHelper.GetRegistrationMocks(user);

            registrationServicesMock.UserManagerMock
                .Setup(userManager =>
                userManager.FindByEmailAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(registrationServicesMock.UserMock));

            var userIdentityServiceMock = new UserIdentityService(
               new Lazy<ILogger<UserIdentityService>>(registrationServicesMock.LoggerMock.Object),
               new Lazy<UserManager<User>>(registrationServicesMock.UserManagerMock.Object),
               new Lazy<IMapper>(registrationServicesMock.MapperMock.Object),
               new Lazy<IBackgroundJobClient>(registrationServicesMock.BackgroundJobClientMock.Object),
               new Lazy<IUserRetrieverService>(registrationServicesMock.UserRetrieverServiceMock.Object),
               new Lazy<IRepository<User>>(registrationServicesMock.UserRepositoryMock.Object),
               registrationServicesMock.AuthConfigMock.Object,
               registrationServicesMock.EamilConfigMock.Object,
               new Lazy<IReadOnlyRepository<GenderEntity>>(registrationServicesMock.GenderRepositoryMock.Object));

            // Act
            var result = await userIdentityServiceMock.RegisterAsync(registrationServicesMock.UserRegistrationWriteServiceMockModel.Object);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(ErrorMessages.EmailTaken);
        }

        [Fact]
        public async Task UserRegistration_Failure_UsernameIsNotUnique()
        {
            // Mock
            var user = new UserEntityBuilder()
                .WithExternalId(Guid.NewGuid())
                .WithUserDsps(isSpotifyAdded: true, isAppleMusicAdded: true)
                .WithUserGender()
                .WithAvatar()
                .Build();

            var registrationServicesMock = TestsHelper.GetRegistrationMocks(user);

            registrationServicesMock.UserManagerMock
                .Setup(userManager =>
                userManager.FindByNameAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(registrationServicesMock.UserMock));

            var userIdentityServiceMock = new UserIdentityService(
               new Lazy<ILogger<UserIdentityService>>(registrationServicesMock.LoggerMock.Object),
               new Lazy<UserManager<User>>(registrationServicesMock.UserManagerMock.Object),
               new Lazy<IMapper>(registrationServicesMock.MapperMock.Object),
               new Lazy<IBackgroundJobClient>(registrationServicesMock.BackgroundJobClientMock.Object),
               new Lazy<IUserRetrieverService>(registrationServicesMock.UserRetrieverServiceMock.Object),
               new Lazy<IRepository<User>>(registrationServicesMock.UserRepositoryMock.Object),
               registrationServicesMock.AuthConfigMock.Object,
               registrationServicesMock.EamilConfigMock.Object,
               new Lazy<IReadOnlyRepository<GenderEntity>>(registrationServicesMock.GenderRepositoryMock.Object));

            // Act
            var result = await userIdentityServiceMock.RegisterAsync(registrationServicesMock.UserRegistrationWriteServiceMockModel.Object);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(ErrorMessages.UsernameTaken);
        }

        [Fact]
        public async Task UserRegistration_Failure_CannotFindGender()
        {
            // Mock
            var user = new UserEntityBuilder()
                .WithExternalId(Guid.NewGuid())
                .WithUserDsps(isSpotifyAdded: true, isAppleMusicAdded: true)
                .WithUserGender()
                .WithAvatar()
                .Build();

            var registrationServicesMock = TestsHelper.GetRegistrationMocks(user);

            registrationServicesMock.GenderRepositoryMock
                .Setup(genderRepository =>
                genderRepository.FirstOrDefaultAsync(It.IsAny<ISpecification<GenderEntity>>()))
                .Returns(Task.FromResult(It.IsAny<GenderEntity>()));

            var userIdentityServiceMock = new UserIdentityService(
               new Lazy<ILogger<UserIdentityService>>(registrationServicesMock.LoggerMock.Object),
               new Lazy<UserManager<User>>(registrationServicesMock.UserManagerMock.Object),
               new Lazy<IMapper>(registrationServicesMock.MapperMock.Object),
               new Lazy<IBackgroundJobClient>(registrationServicesMock.BackgroundJobClientMock.Object),
               new Lazy<IUserRetrieverService>(registrationServicesMock.UserRetrieverServiceMock.Object),
               new Lazy<IRepository<User>>(registrationServicesMock.UserRepositoryMock.Object),
               registrationServicesMock.AuthConfigMock.Object,
               registrationServicesMock.EamilConfigMock.Object,
               new Lazy<IReadOnlyRepository<GenderEntity>>(registrationServicesMock.GenderRepositoryMock.Object));

            // Act
            var result = await userIdentityServiceMock.RegisterAsync(registrationServicesMock.UserRegistrationWriteServiceMockModel.Object);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(ErrorType.GenderDoesNotExist.ToString());
        }

        [Fact]
        public async Task UserRegistration_Failure_CannotCreateUser()
        {
            // Mock
            var user = new UserEntityBuilder()
                .WithExternalId(Guid.NewGuid())
                .WithUserDsps(isSpotifyAdded: true, isAppleMusicAdded: true)
                .WithUserGender()
                .WithAvatar()
                .Build();

            var registrationServicesMock = TestsHelper.GetRegistrationMocks(user);
            registrationServicesMock.UserRegistrationWriteServiceMockModel.Object.Username = "Testowy";

            registrationServicesMock.UserManagerMock
                .Setup(userManager =>
                userManager.CreateAsync(registrationServicesMock.UserMock))
                .Returns(Task.FromResult(IdentityResult.Failed(new IdentityError())));

            var userIdentityServiceMock = new UserIdentityService(
               new Lazy<ILogger<UserIdentityService>>(registrationServicesMock.LoggerMock.Object),
               new Lazy<UserManager<User>>(registrationServicesMock.UserManagerMock.Object),
               new Lazy<IMapper>(registrationServicesMock.MapperMock.Object),
               new Lazy<IBackgroundJobClient>(registrationServicesMock.BackgroundJobClientMock.Object),
               new Lazy<IUserRetrieverService>(registrationServicesMock.UserRetrieverServiceMock.Object),
               new Lazy<IRepository<User>>(registrationServicesMock.UserRepositoryMock.Object),
               registrationServicesMock.AuthConfigMock.Object,
               registrationServicesMock.EamilConfigMock.Object,
               new Lazy<IReadOnlyRepository<GenderEntity>>(registrationServicesMock.GenderRepositoryMock.Object));

            // Act
            var result = await userIdentityServiceMock.RegisterAsync(registrationServicesMock.UserRegistrationWriteServiceMockModel.Object);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(ErrorType.UserNotCreated.ToString());
        }

        [Fact]
        public async Task UserRegistration_Failure_CannotAddUserToRole()
        {
            // Mock
            var user = new UserEntityBuilder()
                .WithExternalId(Guid.NewGuid())
                .WithUserDsps(isSpotifyAdded: true, isAppleMusicAdded: true)
                .WithUserGender()
                .WithAvatar()
                .Build();

            var registrationServicesMock = TestsHelper.GetRegistrationMocks(user);
            registrationServicesMock.UserRegistrationWriteServiceMockModel.Object.Username = "Testowy";

            registrationServicesMock.UserManagerMock
                .Setup(userManager =>
                userManager.AddToRoleAsync(registrationServicesMock.UserMock, It.IsAny<string>()))
                .Returns(Task.FromResult(IdentityResult.Failed(new IdentityError())));

            var userIdentityServiceMock = new UserIdentityService(
               new Lazy<ILogger<UserIdentityService>>(registrationServicesMock.LoggerMock.Object),
               new Lazy<UserManager<User>>(registrationServicesMock.UserManagerMock.Object),
               new Lazy<IMapper>(registrationServicesMock.MapperMock.Object),
               new Lazy<IBackgroundJobClient>(registrationServicesMock.BackgroundJobClientMock.Object),
               new Lazy<IUserRetrieverService>(registrationServicesMock.UserRetrieverServiceMock.Object),
               new Lazy<IRepository<User>>(registrationServicesMock.UserRepositoryMock.Object),
               registrationServicesMock.AuthConfigMock.Object,
               registrationServicesMock.EamilConfigMock.Object,
               new Lazy<IReadOnlyRepository<GenderEntity>>(registrationServicesMock.GenderRepositoryMock.Object));

            // Act
            var result = await userIdentityServiceMock.RegisterAsync(registrationServicesMock.UserRegistrationWriteServiceMockModel.Object);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(ErrorType.UserCantBeAddedToRole.ToString());
        }
    }
}
