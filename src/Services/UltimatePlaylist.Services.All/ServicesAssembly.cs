#region Usings

using UltimatePlaylist.Service.Playlist;
using UltimatePlaylist.Services.Analytics;
using UltimatePlaylist.Services.AppleMusic;
using UltimatePlaylist.Services.CommonData;
using UltimatePlaylist.Services.Dsp;
using UltimatePlaylist.Services.Email;
using UltimatePlaylist.Services.File;
using UltimatePlaylist.Services.Games;
using UltimatePlaylist.Services.Identity.Services.Users;
using UltimatePlaylist.Services.Leaderboard;
using UltimatePlaylist.Services.Media;
using UltimatePlaylist.Services.Notification;
using UltimatePlaylist.Services.Personalization;
using UltimatePlaylist.Services.Schedule;
using UltimatePlaylist.Services.Song;
using UltimatePlaylist.Services.Spotify;
using UltimatePlaylist.Services.Ticket;
using UltimatePlaylist.Services.UserManagement;
using UltimatePlaylist.Services.UserSong;
using UltimatePlaylist.Services.Webhook;

#endregion

namespace UltimatePlaylist.Services.All
{
    public class ServicesAssembly
    {
        // this is needed so the assembly will load and register in Api Startup
        private UserIdentityService UserIdentityService { get; }

        private AdministratorIdentityService AdministratorIdentityService { get; }

        private EmailService EmailService { get; }

        private SongService SongService { get; }

        private EventGridService EventGridService { get; }

        private FileNameFormatterService FileNameFormatterService { get; }

        private AzureMediaService AzureMediaService { get; }

        private SongGenreService SongGenreService { get; }

        private SpotifyService SpotifyService { get; }

        private SpotifyAuthorizationService SpotifyAuthorizationService { get; }

        private AppleMusicConnectionService AppleMusicConnectionService { get; }

        private UserProfileService UserProfileService { get; }

        private TicketService TicketService { get; }

        private PlaylistService PlaylistService { get; }

        private UserSongService UserSongService { get; }

        private SpotifyApiService SpotifyApiService { get; }

        private DspService DspService { get; }

        private AnalyticsService AnalyticsService { get; }

        private ScheduleService ScheduleService { get; }

        private UserManagementService UserManagementService { get; }

        private UserEmailChangeConfirmedFromWebService UserEmailChangeConfirmedFromWebService { get; }

        private GamesInfoService GamesInfoService { get; }

        private LeaderboardService LeaderboardService { get; }

        private NotificationService NotificationService { get; }

        private NotificationsSettingsService NotificationsSettingsService { get; }
    }
}
