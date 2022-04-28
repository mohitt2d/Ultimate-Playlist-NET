#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UltimatePlaylist.Common.Config;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Database.Infrastructure.Context;
using UltimatePlaylist.Database.Infrastructure.Entities.Identity;
using UltimatePlaylist.Database.Infrastructure.Entities.Playlist;
using UltimatePlaylist.Database.Infrastructure.Entities.Song;
using UserRole = UltimatePlaylist.Common.Enums.UserRole;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Seeding
{
    public class DatabaseInitializer
    {
        #region Private fields

        private readonly Lazy<EFContext> ContextProvider;
        private readonly Lazy<UserManager<User>> UserManagerProvider;
        private readonly Lazy<RoleManager<Role>> RoleManagerProvider;
        private readonly Lazy<IOptions<DatabaseSeedConfig>> SeedConfigProvider;
        private readonly Lazy<ILogger<DatabaseInitializer>> LoggerProvider;

        #endregion

        #region Constructor(s)

        public DatabaseInitializer(
            Lazy<EFContext> contextProvider,
            Lazy<UserManager<User>> userManagerProvider,
            Lazy<RoleManager<Role>> roleManagerProvider,
            Lazy<IOptions<DatabaseSeedConfig>> seedConfigProvider,
            Lazy<ILogger<DatabaseInitializer>> loggerProvider)
        {
            ContextProvider = contextProvider;
            UserManagerProvider = userManagerProvider;
            RoleManagerProvider = roleManagerProvider;
            SeedConfigProvider = seedConfigProvider;
            LoggerProvider = loggerProvider;
        }

        #endregion

        #region Properties

        private EFContext Context => ContextProvider.Value;

        private UserManager<User> UserManager => UserManagerProvider.Value;

        private RoleManager<Role> RoleManager => RoleManagerProvider.Value;

        private ILogger<DatabaseInitializer> Logger => LoggerProvider.Value;

        private IOptions<DatabaseSeedConfig> SeedConfig => SeedConfigProvider.Value;

        #endregion

        #region Public methods

        public async Task InitializeAsync()
        {
            Context.Database.Migrate();

            await SeedRolesAsync();
            var genders = await SeedGendersAsync(SeedConfig.Value.Genders);
            await SeedUserAsync(SeedConfig.Value.Admins, genders, UserRole.Administrator);
            await SeedGenresAsync(SeedConfig.Value.MusicGenres);
            await SeedFixedSongs(SeedConfig.Value.Songs);
            Context.SaveChanges();
        }

        #endregion

        #region Private methods

        private async Task SeedRolesAsync()
        {
            if (!(await RoleManager.RoleExistsAsync(UserRole.Administrator.ToString())))
            {
                await RoleManager.CreateAsync(new Role { Name = UserRole.Administrator.ToString() });
            }

            if (!(await RoleManager.RoleExistsAsync(UserRole.User.ToString())))
            {
                await RoleManager.CreateAsync(new Role { Name = UserRole.User.ToString() });
            }
        }

        private async Task<List<GenderEntity>> SeedGendersAsync(IList<DatabaseGenderSeedConfig> genders)
        {
            if (genders is null || genders.Count == 0)
            {
                throw new ArgumentNullException(nameof(genders));
            }

            var genderList = await Context.Genders.ToListAsync();

            if (genderList.Any())
            {
                return genderList;
            }

            Logger.LogDebug("Adding genders");

            foreach (var gender in genders)
            {
                genderList.Add(new GenderEntity()
                {
                    ExternalId = gender.ExternalId,
                    Name = gender.Name,
                });
            }

            Context.Genders.AddRange(genderList);

            return genderList;
        }

        private async Task SeedUserAsync(DatabaseUserSeedConfig userSeedConfig, List<GenderEntity> genders, UserRole userRole)
        {
            if (userSeedConfig is null)
            {
                return;
            }

            foreach (var fixedUser in userSeedConfig.FixedAccounts)
            {
                await SeedFixedUserAsync(fixedUser, genders, userRole);
            }
        }

        private async Task SeedFixedUserAsync(DatabaseSeedUserFixedAccountConfig fixedAccount, List<GenderEntity> genders, UserRole userRole)
        {
            var user = await UserManager
                .Users
                .FirstOrDefaultAsync(u => u.ExternalId == fixedAccount.ExternalId || u.Email == fixedAccount.Email);

            if (user is not null)
            {
                return;
            }

            Logger.LogDebug("Adding fixed {0} with email {1}.", userRole, fixedAccount.Email);

            var gender = genders.Single(s => s.ExternalId == fixedAccount.GenderExternalId);

            user = new User
            {
                Email = fixedAccount.Email,
                ExternalId = fixedAccount.ExternalId,
                UserName = fixedAccount.Email,
                Name = fixedAccount.FirstName,
                LastName = fixedAccount.LastName,
                EmailConfirmed = true,
                PhoneNumber = fixedAccount.PhoneNumber,
                Pin = fixedAccount.Pin,
                BirthDate = DateTime.UtcNow.AddYears(-25),
                Gender = gender,
                IsActive = true,
            };

            await UserManager.CreateAsync(user);

            if (fixedAccount.Password is not null)
            {
                await UserManager.AddPasswordAsync(user, fixedAccount.Password);
            }

            await UserManager.AddToRoleAsync(user, userRole.ToString());
        }

        private async Task SeedGenresAsync(IList<string> genres)
        {
            if (genres is null || genres.Count == 0)
            {
                return;
            }

            var genresList = await Context.Genres.ToListAsync();

            if (genresList.Any())
            {
                return;
            }

            Logger.LogDebug("Adding genres");

            foreach (var genre in genres)
            {
                genresList.Add(new GenreEntity()
                {
                    ExternalId = Guid.NewGuid(),
                    Name = genre,
                });
            }

            Context.Genres.AddRange(genresList);
        }

        private async Task SeedFixedSongs(IList<DatabaseSongSeedConfig> songs)
        {
            var fallbackPlaylist = await Context.Playlists.FirstOrDefaultAsync(p => p.IsFallback);
            if (fallbackPlaylist is not null)
            {
                return;
            }

            Logger.LogDebug("Adding songs");

            var songsList = new List<SongEntity>();
            var playlist = new PlaylistEntity() { IsFallback = true, StartDate = DateTime.UtcNow };

            await Context.Playlists.AddAsync(playlist);
            await Context.SaveChangesAsync();

            foreach (var song in songs)
            {
                songsList.Add(new SongEntity()
                {
                    ExternalId = song.ExternalId,
                    Album = song.Title,
                    Artist = song.Artist,
                    Title = song.Title,
                    FirstReleaseDate = song.FirstRelaseDate,
                    FeaturedArtist = song.Artist,
                    AudioFile = new Entities.File.AudioFileEntity()
                    {
                        FileName = $"Audio-{song.Title}-{Guid.NewGuid()}",
                        StreamingUrl = song.StreamingUrl,
                        Container = "audiofiles",
                        Extension = ".wav",
                        Url = song.StreamingUrl,
                    },
                    CoverFile = new Entities.File.CoverFileEntity()
                    {
                        FileName = $"Cover-{song.Title}-{Guid.NewGuid()}",
                        Url = song.CoverUrl,
                        Container = "covers",
                        Extension = ".jpg",
                    },
                    SongDSPs = new List<SongDSPEntity>()
                    {
                        new SongDSPEntity() { DspType = DspType.AppleMusic, Url = song.AppleMusicUrl, SongDspId = song.AppleMusicId },
                        new SongDSPEntity() { DspType = DspType.Spotify, Url = song.SpotifyUrl, SongDspId = song.SpotifyId },
                    },
                    PlaylistSongs = new List<PlaylistSongEntity>()
                    {
                        new PlaylistSongEntity() { PlaylistId = playlist.Id },
                    },
                    Duration = song.Duration,
                });
            }

            Context.Songs.AddRange(songsList);

            return;
        }

        #endregion
    }
}
