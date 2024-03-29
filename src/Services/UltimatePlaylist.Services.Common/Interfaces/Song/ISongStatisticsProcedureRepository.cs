﻿#region Usings

using CSharpFunctionalExtensions;
using UltimatePlaylist.Common.Models;
using UltimatePlaylist.Database.Infrastructure.Views;
using UltimatePlaylist.Services.Common.Models;
using UltimatePlaylist.Services.Common.Models.Song;

#endregion

namespace UltimatePlaylist.Services.Common.Interfaces.Song
{
    public interface ISongStatisticsProcedureRepository
    {
        Task<int> GeneralSongsCount(SongsAnalyticsFilterServiceModel filter);

        Task<Result<List<GeneralSongDataProcedureView>>> GetGeneralSongsData(Pagination pagination, SongsAnalyticsFilterServiceModel filterServiceModel);

        Task<Result<List<GeneralSongsAnalyticsFileInformationView>>> GetFileSongsData(Pagination pagination, SongsAnalyticsFilterServiceModel filter);
    }
}
