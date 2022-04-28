#region Usings

using AutoMapper;
using FluentAssertions;
using Moq;
using UltimatePlaylist.Common.Const;
using UltimatePlaylist.Common.Mvc.Interface;
using UltimatePlaylist.Database.Infrastructure.Entities.Identity;
using UltimatePlaylist.Database.Infrastructure.Entities.Identity.Specifications;
using UltimatePlaylist.Database.Infrastructure.Repositories.Interfaces;
using UltimatePlaylist.Services.Common.Interfaces.User;
using UltimatePlaylist.Services.UserManagement;
using Xunit;

#endregion

namespace UltimatePlaylist.Tests.UnitTests.Users.Services.UserManagement
{
    public class UserManagementServiceTest
    {
        private readonly Mock<IRepository<User>> UserRepositoryMock;
        private readonly Mock<IMapper> MapperMock;
        private readonly Mock<IUserActiveService> UserActiveServiceMock;
        private readonly IUserManagementService UserManagementService;
        private readonly Mock<IUserManagementProcedureRepository> UserManagementProcedureRepositoryMock;

        public UserManagementServiceTest()
        {
            UserRepositoryMock = new Mock<IRepository<User>>();
            MapperMock = new Mock<IMapper>();
            UserActiveServiceMock = new Mock<IUserActiveService>();
            UserManagementProcedureRepositoryMock = new Mock<IUserManagementProcedureRepository>();

            UserManagementService = new UserManagementService(
                new Lazy<IRepository<User>>(UserRepositoryMock.Object),
                new Lazy<IMapper>(MapperMock.Object),
                new Lazy<IUserActiveService>(UserActiveServiceMock.Object),
                new Lazy<IUserManagementProcedureRepository>(UserManagementProcedureRepositoryMock.Object));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Should_Change_User_Is_Active_Status(bool isActive)
        {
            // Mock
            var user = new User()
            {
                ExternalId = Guid.NewGuid(),
                IsActive = true,
            };

            UserRepositoryMock.Setup(x => x.FirstOrDefaultAsync(It.IsAny<UserSpecification>())).Returns(Task.FromResult(user));

            // Act
            var result = await UserManagementService.ChangeUserStatus(user.ExternalId, isActive);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(isActive);
            UserRepositoryMock.Verify(x => x.FirstOrDefaultAsync(It.IsAny<UserSpecification>()), Times.Once);
            UserRepositoryMock.Verify(x => x.UpdateAndSaveAsync(user, true), Times.Once);
            UserActiveServiceMock.Verify(x => x.UpdateActiveStatusInStore(user.ExternalId, isActive), Times.Once);
        }

        [Fact]
        public async Task Should_Return_Error_While_User_Exists()
        {
            // Mock
            UserRepositoryMock.Setup(x => x.FirstOrDefaultAsync(It.IsAny<UserSpecification>())).Returns(Task.FromResult((User)null));

            // Act
            var result = await UserManagementService.ChangeUserStatus(Guid.NewGuid(), isActive: true);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(ErrorMessages.UserDoesNotExist);
            UserRepositoryMock.Verify(x => x.FirstOrDefaultAsync(It.IsAny<UserSpecification>()), Times.Once);
        }
    }
}
