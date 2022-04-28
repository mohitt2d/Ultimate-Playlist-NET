#region Usings

using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CSharpFunctionalExtensions;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UltimatePlaylist.Common.Config;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Common.Mvc.Exceptions;
using UltimatePlaylist.Common.Mvc.Interface;
using UltimatePlaylist.Database.Infrastructure.Entities.Identity;
using UltimatePlaylist.Database.Infrastructure.Repositories.Interfaces;
using UltimatePlaylist.Services.Common.Interfaces.Identity;
using UltimatePlaylist.Services.Common.Models.Identity;
using UserRole = UltimatePlaylist.Common.Enums.UserRole;

#endregion

namespace UltimatePlaylist.Services.Identity.Services.Users
{
    public class AdministratorIdentityService : BaseIdentityService, IAdministratorIdentityService
    {
        #region Private members

        private readonly Lazy<ILogger<AdministratorIdentityService>> LoggerProvider;

        #endregion

        #region Constructor(s)

        public AdministratorIdentityService(
          Lazy<ILogger<AdministratorIdentityService>> loggerProvider,
          Lazy<UserManager<User>> userManagerProvider,
          Lazy<IMapper> mapperProvider,
          Lazy<IBackgroundJobClient> backgroundJobClientProvider,
          Lazy<IUserRetrieverService> userRetrieverServiceProvider,
          Lazy<IRepository<User>> userRepositoryProvider,
          IOptions<AuthConfig> authOptions,
          IOptions<EmailConfig> emailOptions)
          : base(userManagerProvider, mapperProvider, backgroundJobClientProvider, userRetrieverServiceProvider, userRepositoryProvider, authOptions, emailOptions)
        {
            LoggerProvider = loggerProvider;
        }

        #endregion

        #region Properties

        private ILogger<AdministratorIdentityService> Logger => LoggerProvider.Value;
        #endregion

        #region Traditional Login

        public async Task<Result<AuthenticationReadServiceModel>> LoginAsync(UserLoginWriteServiceModel userLoginWriteServiceModel)
        {
            var user = await UserManager
                .Users
                .Include(x => x.Roles)
                .ThenInclude(x => x.Role)
                .Where(p => p.Roles.Any(x => x.Role.Name == nameof(UserRole.Administrator)))
                .FirstOrDefaultAsync(u => u.NormalizedEmail.Equals(userLoginWriteServiceModel.Email.ToUpper()));

            if (user == null)
            {
                throw new LoginException(ErrorType.InvalidUsernameOrPassword);
            }

            if (!user.EmailConfirmed)
            {
                throw new LoginException(ErrorType.UserEmailNotConfirmed);
            }

            var userHasValidPassword = await UserManager.CheckPasswordAsync(user, userLoginWriteServiceModel.Password);
            if (!userHasValidPassword)
            {
                throw new LoginException(ErrorType.InvalidUsernameOrPassword);
            }

            return await GenerateAuthenticationResult(user);
        }

        #endregion
    }
}