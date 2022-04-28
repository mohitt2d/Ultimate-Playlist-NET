#region Usings

using CSharpFunctionalExtensions;
using UltimatePlaylist.Services.Common.Models.AppleMusic;
using UltimatePlaylist.Services.Common.Models.Ticket;

#endregion

namespace UltimatePlaylist.Services.Common.Interfaces.AppleMusic
{
    public interface IAppleMusicSongService
    {
        Task<Result<EarnedTicketsReadServiceModel>> AddSongToAppleMusicWithTicketAsync(Guid userExternalId, AddSongToAppleMusicWriteServiceModel addSongToAppleMusicWriteServiceModel);

        Task<Result> AddSongToAppleMusicWithoutTicketAsync(Guid userExternalId, AddSongToAppleMusicWriteServiceModel addSongToAppleMusicWriteServiceModel);
    }
}
