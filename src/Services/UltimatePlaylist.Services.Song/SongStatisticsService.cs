﻿#region Usings

using System.ComponentModel;
using AutoMapper;
using CSharpFunctionalExtensions;
using OfficeOpenXml;
using UltimatePlaylist.Common.Models;
using UltimatePlaylist.Common.Mvc.Attributes;
using UltimatePlaylist.Database.Infrastructure.Views;
using UltimatePlaylist.Services.Common.Interfaces.Song;
using UltimatePlaylist.Services.Common.Models;
using UltimatePlaylist.Services.Common.Models.Song;

#endregion

namespace UltimatePlaylist.Services.Song
{
    public class SongStatisticsService : ISongStatisticsService
    {
        #region Private field(s)

        private readonly Lazy<IMapper> MapperProvider;
        private readonly Lazy<ISongStatisticsProcedureRepository> SongStatisticsProcedureRepositoryProvider;

        #endregion

        #region Constructor(s)

        public SongStatisticsService(
            Lazy<IMapper> mapperProvider,
            Lazy<ISongStatisticsProcedureRepository> songStatisticsProcedureRepositoryProvider)
        {
            MapperProvider = mapperProvider;
            SongStatisticsProcedureRepositoryProvider = songStatisticsProcedureRepositoryProvider;
        }

        #endregion

        #region Private properties

        private IMapper Mapper => MapperProvider.Value;

        private ISongStatisticsProcedureRepository SongStatisticsProcedureRepository => SongStatisticsProcedureRepositoryProvider.Value;

        #endregion

        #region Public method(s)

        public async Task<Result<PaginatedReadServiceModel<GeneralSongDataListItemReadServiceModel>>> SongsListAsync(Pagination pagination, SongsAnalyticsFilterServiceModel filterServiceModel)
        {
            var count = await SongStatisticsProcedureRepository.GeneralSongsCount(filterServiceModel);

            return await SongStatisticsProcedureRepository.GetGeneralSongsData(pagination, filterServiceModel)
                   .Map(songs => Mapper.Map<IReadOnlyList<GeneralSongDataListItemReadServiceModel>>(songs))
                   .Map(songs => new PaginatedReadServiceModel<GeneralSongDataListItemReadServiceModel>(songs, pagination, count));
        }

        public async Task<Result<IReadOnlyList<SongsAnalyticsFileServiceReadModel>>> GetDataForFile(Pagination pagination, SongsAnalyticsFilterServiceModel filterServiceModel)
        {
            return await SongStatisticsProcedureRepository.GetFileSongsData(pagination, filterServiceModel)
                   .Map(songs => Mapper.Map<IReadOnlyList<SongsAnalyticsFileServiceReadModel>>(songs));
        }

        #endregion
    }
}
