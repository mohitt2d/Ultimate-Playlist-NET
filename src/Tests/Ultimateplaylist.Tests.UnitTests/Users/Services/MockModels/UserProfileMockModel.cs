#region Usings

using AutoMapper;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using UltimatePlaylist.Common.Config;
using UltimatePlaylist.Common.Mvc.Interface;
using UltimatePlaylist.Database.Infrastructure.Entities.Dsp;
using UltimatePlaylist.Database.Infrastructure.Entities.File;
using UltimatePlaylist.Database.Infrastructure.Entities.Identity;
using UltimatePlaylist.Database.Infrastructure.Repositories.Interfaces;
using UltimatePlaylist.Services.Common.Interfaces.Files;
using UltimatePlaylist.Services.Common.Models.Profile;

#endregion

namespace UltimatePlaylist.Tests.UnitTests.Users.Services.MockModels
{
    public class UserProfileMockModel
    {
        public EditUserProfileWriteServiceModel EditUserProfileWriteServiceMockModel { get; set; }

        public Mock<UserManager<User>> UserManagerMock { get; set; }

        public Mock<IRepository<User>> UserRepositoryMock { get; set; }

        public Mock<IReadOnlyRepository<UserDspEntity>> UserDspRepositoryMock { get; set; }

        public Mock<IAvatarFileService> AvatarFileServiceMock { get; set; }

        public Mock<IReadOnlyRepository<AvatarFileEntity>> AvatarRepositoryMock { get; set; }

        public Mock<IReadOnlyRepository<GenderEntity>> GenderRepositoryMock { get; set; }

        public Mock<IMapper> MapperMock { get; set; }

        public IOptions<EmailConfig> EmailConfig { get; set; }

        public Mock<IBackgroundJobClient> BackgroundJobClientMock { get; set; }

        public Mock<IUserBlacklistTokenStore> UserBlacklistTokenStoreMock { get; set; }
    }
}
