#region Usings

using CSharpFunctionalExtensions;
using UltimatePlaylist.Common.Models;
using UltimatePlaylist.Services.Common.Models;
using UltimatePlaylist.Services.Common.Models.Song;

#endregion

namespace UltimatePlaylist.Services.Common.Interfaces.Song
{
    public interface ISongStatisticsService
    {
        Task<Result<PaginatedReadServiceModel<GeneralSongDataListItemReadServiceModel>>> SongsListAsync(Pagination pagination, SongsAnalyticsFilterServiceModel filterServiceModel);

        Task<Result<IReadOnlyList<SongsAnalyticsFileServiceReadModel>>> GetDataForFile(Pagination pagination, SongsAnalyticsFilterServiceModel filterServiceModel);
    }
}
