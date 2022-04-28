#region Usings

using System;
using System.Collections.Generic;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Database.Infrastructure.Entities.Dsp;
using UltimatePlaylist.Database.Infrastructure.Entities.File;
using UltimatePlaylist.Database.Infrastructure.Entities.Identity;
using UltimatePlaylist.Services.Common.Models.Profile;

#endregion

namespace UltimatePlaylist.Tests.UnitTests.Users.Services.Builders
{
    public class UserEntityBuilder
    {
        private readonly User UserEntity;

        private readonly User UserUpdatedEntity;

        public UserEntityBuilder()
        {
            UserEntity = new User();
            UserUpdatedEntity = new User();
        }

        public User Build() => UserEntity;

        public User BuildExpectedResponse() => UserUpdatedEntity;

        public UserEntityBuilder WithExternalId(Guid externalId)
        {
            UserEntity.ExternalId = externalId;
            UserEntity.Name = "TestUser";

            return this;
        }

        public UserEntityBuilder WithUserDsps(
            bool isSpotifyAdded = false,
            bool isAppleMusicAdded = false)
        {
            UserEntity.Dsps = new List<UserDspEntity>();

            if (isSpotifyAdded)
            {
                UserEntity.Dsps.Add(new UserDspEntity()
                {
                    ExternalId = Guid.NewGuid(),
                    SpotifyAccessToken = "TestAccessToken",
                    SpotifyRefreshToken = "TestRefreshToken",
                    Type = DspType.Spotify,
                    SpotifyScopes = "TestScopes",
                    SpotifyTokenType = "Bearer",
                    UserSpotifyIdentity = "TestUser",
                    AccessTokenExpirationDate = DateTime.UtcNow.AddHours(1),
                    IsActive = true,
                });
            }

            if (isAppleMusicAdded)
            {
                UserEntity.Dsps.Add(new UserDspEntity()
                {
                    ExternalId = Guid.NewGuid(),
                    Type = DspType.AppleMusic,
                    AppleUserToken = "TestAppleMusicToken",
                    IsActive = true,
                });
            }

            return this;
        }

        public UserEntityBuilder WithUserGender()
        {
            UserEntity.Gender = new GenderEntity()
            {
                ExternalId = Guid.NewGuid(),
                Name = "TestGender",
            };

            return this;
        }

        public UserEntityBuilder WithAvatar()
        {
            UserEntity.AvatarFile = new AvatarFileEntity()
            {
                ExternalId = Guid.NewGuid(),
                Url = "TestAvatarUrl",
            };

            return this;
        }

        public UserEntityBuilder ExpectedVauleForUpdateUserProfile(Guid userExternalId, EditUserProfileWriteServiceModel editUserProfileWriteServiceModel)
        {
            UserUpdatedEntity.ExternalId = userExternalId;
            UserUpdatedEntity.Name = editUserProfileWriteServiceModel.FirstName;
            UserUpdatedEntity.LastName = editUserProfileWriteServiceModel.LastName;
            UserUpdatedEntity.Gender = new GenderEntity()
            {
                ExternalId = editUserProfileWriteServiceModel.GenderExternalId,
                Name = "TestUpdatedGender",
            };
            UserUpdatedEntity.AvatarFile = editUserProfileWriteServiceModel.AvatarExternalId.HasValue
                ? new AvatarFileEntity()
                {
                    ExternalId = editUserProfileWriteServiceModel.AvatarExternalId.Value,
                    Url = "TestUpdatedAvatarUrl",
                }
                : null;
            UserUpdatedEntity.Email = editUserProfileWriteServiceModel.Email;
            UserUpdatedEntity.PhoneNumber = editUserProfileWriteServiceModel.PhoneNumber;
            UserUpdatedEntity.UserName = editUserProfileWriteServiceModel.Username;
            UserUpdatedEntity.ZipCode = editUserProfileWriteServiceModel.ZipCode;
            UserUpdatedEntity.BirthDate = editUserProfileWriteServiceModel.BirthDate;
            UserUpdatedEntity.IsSmsPromotionalNotificationEnabled = editUserProfileWriteServiceModel.IsSmsPromotionalNotificationEnabled;

            return this;
        }
    }
}
