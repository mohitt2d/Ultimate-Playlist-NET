#region Usings

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CSharpFunctionalExtensions;
using UltimatePlaylist.Database.Infrastructure.Entities.Identity;
using UltimatePlaylist.Database.Infrastructure.Entities.Identity.Specifications;
using UltimatePlaylist.Database.Infrastructure.Repositories.Interfaces;
using UltimatePlaylist.Services.Common.Interfaces.CommonData;
using UltimatePlaylist.Services.Common.Models.CommonData;

#endregion

namespace UltimatePlaylist.Services.CommonData
{
    public class GenderService : IGenderService
    {
        #region Private members

        private readonly Lazy<IMapper> MapperProvider;
        private readonly Lazy<IRepository<GenderEntity>> GenderRepositoryProvider;

        #endregion

        #region Constructor(s)

        public GenderService(
            Lazy<IMapper> mapperProvider,
            Lazy<IRepository<GenderEntity>> genderRepositoryProvider)
        {
            MapperProvider = mapperProvider;
            GenderRepositoryProvider = genderRepositoryProvider;
        }

        #endregion

        #region Properties

        private IMapper Mapper => MapperProvider.Value;

        private IRepository<GenderEntity> GenderRepository => GenderRepositoryProvider.Value;

        #endregion

        #region Public Methods

        public async Task<Result<IList<GenderReadServiceModel>>> GetGenders()
        {
            var genders = await GenderRepository.ListAsync(new GenderSpecification());

            return Result.Success()
                .Map(() => Mapper.Map<IList<GenderReadServiceModel>>(genders));
        }

        #endregion
    }
}
