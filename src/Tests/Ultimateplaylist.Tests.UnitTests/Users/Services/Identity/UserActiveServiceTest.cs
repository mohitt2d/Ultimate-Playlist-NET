#region Usings

using AutoFixture;
using Moq;
using UltimatePlaylist.Common.Mvc.Interface;
using UltimatePlaylist.Database.Infrastructure.Entities.Identity;
using UltimatePlaylist.Database.Infrastructure.Entities.Identity.Specifications;
using UltimatePlaylist.Database.Infrastructure.Repositories.Interfaces;
using UltimatePlaylist.Services.Common.Interfaces.Identity;
using UltimatePlaylist.Services.Identity.Services.Users;
using Xunit;

#endregion

namespace UltimatePlaylist.Tests.UnitTests.Users.Services.Identity
{
    public class UserActiveServiceTest
    {
        private Mock<IReadOnlyRepository<User>> UserReadRepositoryMock;
        private Mock<IUserBlacklistTokenStore> UserBlacklistTokenStoreMock;
        private Mock<IUserActiveStore> UserActiveStoreMock;
        private IUserActiveService UserActiveService;

        public UserActiveServiceTest()
        {
            UserReadRepositoryMock = new Mock<IReadOnlyRepository<User>>();
            UserBlacklistTokenStoreMock = new Mock<IUserBlacklistTokenStore>();
            UserActiveStoreMock = new Mock<IUserActiveStore>();
            UserActiveService = new UserActiveService(
                new Lazy<IReadOnlyRepository<User>>(UserReadRepositoryMock.Object),
                new Lazy<IUserBlacklistTokenStore>(UserBlacklistTokenStoreMock.Object),
                new Lazy<IUserActiveStore>(UserActiveStoreMock.Object));
        }

        [Fact]
        public async Task Should_Set_Token_In_BlackList_If_Exist_In_Store()
        {
            // Arrange
            var userExternalId = Guid.NewGuid();
            var token = new Fixture().Build<string>().Create();
            var isActive = false;
            UserActiveStoreMock.Setup(x => x.TryGet(userExternalId, out isActive)).Returns(true);
            UserBlacklistTokenStoreMock.Setup(x => x.Set(userExternalId, token));

            // Act
            await UserActiveService.CheckActiveStatusAndSetTokenOnBlacklist(userExternalId, token);

            // Assert
            UserActiveStoreMock.Verify(x => x.TryGet(userExternalId, out isActive), Times.Once);
            UserBlacklistTokenStoreMock.Verify(x => x.Set(userExternalId, token), Times.Once);
        }

        [Fact]
        public async Task Should_Set_Token_In_BlackList_If_Exist_In_Repository()
        {
            // Arrange
            var userExternalId = Guid.NewGuid();
            var token = new Fixture().Build<string>().Create();
            var isActive = false;
            var user = new User() { IsActive = isActive, ExternalId = userExternalId };
            UserActiveStoreMock.Setup(x => x.TryGet(userExternalId, out isActive)).Returns(false);
            UserBlacklistTokenStoreMock.Setup(x => x.Set(userExternalId, token));
            UserReadRepositoryMock.Setup(x => x.FirstOrDefaultAsync(It.IsAny<UserSpecification>())).Returns(Task.FromResult(user));

            // Act
            await UserActiveService.CheckActiveStatusAndSetTokenOnBlacklist(userExternalId, token);

            // Assert
            UserActiveStoreMock.Verify(x => x.TryGet(userExternalId, out isActive), Times.Once);
            UserActiveStoreMock.Verify(x => x.Set(userExternalId, isActive), Times.Once);
            UserBlacklistTokenStoreMock.Verify(x => x.Set(userExternalId, token), Times.Once);
            UserReadRepositoryMock.Verify(x => x.FirstOrDefaultAsync(It.IsAny<UserSpecification>()), Times.Once);
        }

        [Fact]
        public void Should_Set_Token_In_Store()
        {
            // Arrange
            var userExternalId = Guid.NewGuid();
            var isActive = false;

            // Act
            UserActiveService.UpdateActiveStatusInStore(userExternalId, isActive);

            // Assert
            UserActiveStoreMock.Verify(x => x.Set(userExternalId, isActive), Times.Once);
        }
    }
}
