#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Database.Infrastructure.Entities.Dsp;
using UltimatePlaylist.Database.Infrastructure.Entities.Dsp.Specifications;
using UltimatePlaylist.Database.Infrastructure.Entities.Identity;
using UltimatePlaylist.Database.Infrastructure.Entities.Identity.Specifications;
using UltimatePlaylist.Database.Infrastructure.Repositories.Interfaces;
using UltimatePlaylist.Services.Common.Interfaces.Dsp;
using UltimatePlaylist.Services.Common.Models.Dsp;

#endregion

namespace UltimatePlaylist.Services.Dsp
{
    public class DspService : IDspService
    {
        #region Private members

        private readonly Lazy<IRepository<UserDspEntity>> UserDspsRepositoryProvider;

        private readonly Lazy<IReadOnlyRepository<User>> UserRepositoryProvider;

        private readonly Lazy<IMapper> MapperProvider;

        #endregion

        #region Constructor(s)

        public DspService(
            Lazy<IRepository<UserDspEntity>> userDspsRepositoryProvider,
            Lazy<IReadOnlyRepository<User>> userRepositoryProvider,
            Lazy<ILogger<DspService>> loggerProvider,
            Lazy<IMapper> mapperProvider)
        {
            UserDspsRepositoryProvider = userDspsRepositoryProvider;
            UserRepositoryProvider = userRepositoryProvider;
            MapperProvider = mapperProvider;
        }

        #endregion

        #region Properties

        private IRepository<UserDspEntity> UserDspsRepository => UserDspsRepositoryProvider.Value;

        private IReadOnlyRepository<User> UserRepository => UserRepositoryProvider.Value;

        private IMapper Mapper => MapperProvider.Value;

        #endregion

        #region Public Method(s)

        public async Task<Result<IReadOnlyList<UserDspReadServiceModel>>> UserConnectedDsps(Guid userExternalId)
        {
            return await GetUser(userExternalId)
                .Bind(async user => await GetUserConnectedDsps(user.ExternalId));
        }

        #endregion

        #region Private Method(s)

        private async Task<Result<User>> GetUser(Guid userExternalId)
        {
            var user = await UserRepository.FirstOrDefaultAsync(new UserSpecification()
                .ByExternalId(userExternalId));

            return Result.SuccessIf(user != null, ErrorType.CannotFindUser.ToString())
                .Map(() => user);
        }

        private async Task<Result<IReadOnlyList<UserDspReadServiceModel>>> GetUserConnectedDsps(Guid userExternalId)
        {
            var userDsps = await UserDspsRepository.ListAsync(new UserDspSpecification()
                .ByActive()
                .ByUserExternalId(userExternalId));

            return Result.Success()
                .Map(() => Mapper.Map<IReadOnlyList<UserDspReadServiceModel>>(userDsps.GroupBy(p => p.Type).Select(g => g.First())));
        }

        private DspType CheckDspType(string type) => type switch
        {
            "spotify" => DspType.Spotify,
            "applemusic" => DspType.AppleMusic,
            _ => DspType.Undefined,
        };

        #endregion
    }
}
