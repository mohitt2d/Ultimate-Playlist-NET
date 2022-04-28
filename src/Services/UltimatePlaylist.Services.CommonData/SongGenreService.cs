#region Usings

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CSharpFunctionalExtensions;
using UltimatePlaylist.Database.Infrastructure.Entities.Song;
using UltimatePlaylist.Database.Infrastructure.Entities.Specifications;
using UltimatePlaylist.Database.Infrastructure.Repositories.Interfaces;
using UltimatePlaylist.Services.Common.Interfaces.Song;
using UltimatePlaylist.Services.Common.Models.CommonData;

#endregion

namespace UltimatePlaylist.Services.CommonData
{
    public class SongGenreService : ISongGenreService
    {
        #region Private members

        private readonly Lazy<IMapper> MapperProvider;
        private readonly Lazy<IRepository<GenreEntity>> GenreRepositoryProvider;

        #endregion

        #region Constructor(s)

        public SongGenreService(
            Lazy<IMapper> mapperProvider,
            Lazy<IRepository<GenreEntity>> genreRepositoryProvider)
        {
            MapperProvider = mapperProvider;
            GenreRepositoryProvider = genreRepositoryProvider;
        }

        #endregion

        #region Properties

        private IMapper Mapper => MapperProvider.Value;

        private IRepository<GenreEntity> GenreRepository => GenreRepositoryProvider.Value;

        #endregion

        #region Public Methods

        public async Task<Result<IList<SongGenresReadServiceModel>>> Genres()
        {
            var genres = await GenreRepository.ListAsync(new GenreSpecification());

            return Result.Success()
                .Map(() => Mapper.Map<IList<SongGenresReadServiceModel>>(genres));
        }

        #endregion
    }
}
