#region Usings

using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using UltimatePlaylist.Database.Infrastructure.Entities.UserSongHistory;

#endregion

namespace UltimatePlaylist.Services.Common.Interfaces.UserSong
{
    public interface IUserSongHistoryService
    {
        public Task<Result<UserSongHistoryEntity>> GetOrAddUserSongHistoryAsync(Guid songExternalId, Guid userExternalId);
    }
}
