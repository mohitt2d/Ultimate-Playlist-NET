#region Usings

using AutoMapper;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using UltimatePlaylist.Common.Config;
using UltimatePlaylist.Database.Infrastructure.Entities.Identity;
using UltimatePlaylist.Database.Infrastructure.Repositories.Interfaces;
using UltimatePlaylist.Services.Common.Interfaces.Identity;
using UltimatePlaylist.Services.Common.Models.Identity;
using UltimatePlaylist.Services.Identity.Services.Users;

#endregion

namespace UltimatePlaylist.Tests.UnitTests.Users.Services.MockModels
{
    public class RegistrationMockModel
    {
        public Mock<UserRegistrationWriteServiceModel> UserRegistrationWriteServiceMockModel { get; set; }

        public User UserMock { get; set; }

        public GenderEntity GenderEntityMock { get; set; }

        public Mock<ILogger<UserIdentityService>> LoggerMock { get; set; }

        public Mock<UserManager<User>> UserManagerMock { get; set; }

        public Mock<IMapper> MapperMock { get; set; }

        public Mock<IBackgroundJobClient> BackgroundJobClientMock { get; set; }

        public Mock<IUserRetrieverService> UserRetrieverServiceMock { get; set; }

        public Mock<IRepository<User>> UserRepositoryMock { get; set; }

        public Mock<IReadOnlyRepository<GenderEntity>> GenderRepositoryMock { get; set; }

        public Mock<IOptions<AuthConfig>> AuthConfigMock { get; set; }

        public Mock<IOptions<EmailConfig>> EamilConfigMock { get; set; }
    }
}
