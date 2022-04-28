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
using UltimatePlaylist.Common.Const;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Common.Mvc.Interface;
using UltimatePlaylist.Database.Infrastructure.Entities.Identity;
using UltimatePlaylist.Database.Infrastructure.Entities.Identity.Specifications;
using UltimatePlaylist.Database.Infrastructure.Repositories.Interfaces;
using UltimatePlaylist.Services.Common.Interfaces.Identity;
using UltimatePlaylist.Services.Common.Models.Identity;
using UserRole = UltimatePlaylist.Common.Enums.UserRole;

#endregion

namespace UltimatePlaylist.Services.Identity.Services.Users
{
    public class UserIdentityService : BaseIdentityService, IUserIdentityService
    {
        #region Private members

        private readonly Lazy<ILogger<UserIdentityService>> LoggerProvider;
        private readonly Lazy<IReadOnlyRepository<GenderEntity>> GenderRepositoryProvider;

        #endregion

        #region Constructor(s)

        public UserIdentityService(
            Lazy<ILogger<UserIdentityService>> loggerProvider,
            Lazy<UserManager<User>> userManagerProvider,
            Lazy<IMapper> mapperProvider,
            Lazy<IBackgroundJobClient> backgroundJobClientProvider,
            Lazy<IUserRetrieverService> userRetrieverServiceProvider,
            Lazy<IRepository<User>> userRepositoryProvider,
            IOptions<AuthConfig> authOptions,
            IOptions<EmailConfig> emailOptions,
            Lazy<IReadOnlyRepository<GenderEntity>> genderRepositoryProvider)
            : base(userManagerProvider, mapperProvider, backgroundJobClientProvider, userRetrieverServiceProvider, userRepositoryProvider, authOptions, emailOptions)
        {
            LoggerProvider = loggerProvider;
            GenderRepositoryProvider = genderRepositoryProvider;
        }

        #endregion

        #region Properties

        private ILogger<UserIdentityService> Logger => LoggerProvider.Value;

        private IReadOnlyRepository<GenderEntity> GenderRepository => GenderRepositoryProvider.Value;

        #endregion

        #region Traditional Login

        public async Task<Result<AuthenticationReadServiceModel>> LoginAsync(UserLoginWriteServiceModel userLoginWriteServiceModel)
        {
            var user = await UserManager
                .Users
                .Include(x => x.Roles)
                .ThenInclude(x => x.Role)
                .Where(p => p.Roles.Any(x => x.Role.Name == nameof(UserRole.User)))
                .FirstOrDefaultAsync(u => u.NormalizedEmail.Equals(userLoginWriteServiceModel.Email.ToUpper()));

            if (user is null)
            {
                return Result.Failure<AuthenticationReadServiceModel>(ErrorMessages.InvalidEmailOrPassword);
            }

            if (!user.EmailConfirmed)
            {
                await SendConfirmationRequestEmail(user);
                return Result.Failure<AuthenticationReadServiceModel>(ErrorMessages.EmailNotConfirmed);
            }

            if (!user.IsActive)
            {
                return Result.Failure<AuthenticationReadServiceModel>(ErrorMessages.UserInActive);
            }

            var userHasValidPassword = await UserManager.CheckPasswordAsync(user, userLoginWriteServiceModel.Password);
            if (!userHasValidPassword)
            {
                return Result.Failure<AuthenticationReadServiceModel>(ErrorMessages.InvalidEmailOrPassword);
            }

            user.IsEmailChangeConfirmedFromWeb = false;
            await UserRepository.UpdateAndSaveAsync(user);

            return await GenerateAuthenticationResult(user);
        }

        public async Task<Result> RegisterAsync(UserRegistrationWriteServiceModel request)
        {
            var existingUser = await UserManager.FindByEmailAsync(request.Email);
            if (existingUser is not null)
            {
                return Result.Failure(ErrorMessages.EmailTaken);
            }

            var existingUserWithUniqueUsername = await UserManager.FindByNameAsync(request.Username);
            if (existingUserWithUniqueUsername is not null)
            {
                return Result.Failure(ErrorMessages.UsernameTaken);
            }

            var gender = await GenderRepository.FirstOrDefaultAsync(new GenderSpecification()
                .ByExternalId(request.GenderExternalId));
            if (gender is null)
            {
                return Result.Failure(ErrorType.GenderDoesNotExist.ToString());
            }

            request.Username = request.Username.Trim();
            var newUser = Mapper.Map<User>(request);
            newUser.Gender = gender;
            newUser.IsActive = true;
            newUser.ShouldNotificationBeEnabled = true;

            var createdUser = await UserManager.CreateAsync(newUser);
            if (!createdUser.Succeeded)
            {
                return Result.Failure(ErrorType.UserNotCreated.ToString());
            }

            await UserManager.AddPasswordAsync(newUser, request.Password);
            await UserManager.UpdateAsync(newUser);

            var roleResult = await UserManager.AddToRoleAsync(newUser, UserRole.User.ToString());
            if (!roleResult.Succeeded)
            {
                return Result.Failure(ErrorType.UserCantBeAddedToRole.ToString());
            }

            await SendConfirmationRequestEmail(newUser);
            return Result.Success();
        }

        #endregion

    }
}