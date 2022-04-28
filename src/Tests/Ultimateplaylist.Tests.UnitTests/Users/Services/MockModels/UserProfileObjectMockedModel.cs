#region Usings

using Moq;
using UltimatePlaylist.Database.Infrastructure.Entities.Dsp;
using UltimatePlaylist.Database.Infrastructure.Entities.File;
using UltimatePlaylist.Database.Infrastructure.Entities.Identity;
using UltimatePlaylist.Services.Common.Models.Files;

#endregion

namespace UltimatePlaylist.Tests.UnitTests.Users.Services.MockModels
{
    public class UserProfileObjectMockedModel
    {
        public Mock<User> MockedUser { get; set; }

        public Mock<UserDspEntity> MockedUserDsps { get; set; }

        public Mock<GenderEntity> MockedGender { get; set; }

        public Mock<FileReadServiceModel> MockedAvatar { get; set; }

        public Mock<AvatarFileEntity> MockedAvatarEntity { get; set; }
    }
}
