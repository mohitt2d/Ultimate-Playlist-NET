#region Usings

using AutoFixture;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Services.Common.Models.Analytics;
using UltimatePlaylist.Services.Common.Models.Playlist;
using UltimatePlaylist.Services.Common.Models.Song;
using UltimatePlaylist.Services.Common.Models.Ticket;

#endregion

namespace UltimatePlaylist.Tests.UnitTests.Common
{
    public static class AnalyticsTestDataProvider
    {
        public static SaveAnalyticsDataWriteServiceModel GetSaveAnalyticsDataWriteServiceModel(
            AnalitycsEventType eventType = AnalitycsEventType.SkipSong,
            int? actualListeningSecond = null,
            Guid? playlistExternalId = null,
            Guid? songExternalId = null)
        {
            var fixture = new Fixture();

            var builder = fixture.Build<SaveAnalyticsDataWriteServiceModel>()
                .With(x => x.EventType, eventType)
                .With(x => x.ActualListeningSecond, actualListeningSecond)
                .With(x => x.PlaylistExternalId, playlistExternalId ?? Guid.NewGuid())
                .With(x => x.SongExternalId, songExternalId ?? Guid.NewGuid());

            return builder.Create();
        }

        public static PlaylistReadServiceModel GetPlaylistReadServiceModelWithRandomExternalIds(int numbersOfSongs = 5)
        {
            var fixture = new Fixture();

            var builder = fixture.Build<PlaylistReadServiceModel>()
                .With(x => x.Songs, GetMultipleSongs(numbersOfSongs));

            return builder.Create();
        }

        public static List<UserSongReadServiceModel> GetMultipleSongs(int numbersOfSongs)
        {
            var fixture = new Fixture();

            var builder = fixture.Build<UserSongReadServiceModel>()
                .With(x => x.IsSkipped, false);

            return builder.CreateMany(numbersOfSongs).ToList();
        }

        public static TicketsStatsReadServiceModel GetLastEarnedNotDisplayedTicketsReadServiceModel(int latestEarnedTickets = 1)
        {
            var fixture = new Fixture();

            var builder = fixture.Build<TicketsStatsReadServiceModel>()
                .With(x => x.TicketsAmountForTodayDrawing, latestEarnedTickets);

            return builder.Create();
        }

        public static AnalyticsLastEarnedTicketsReadServiceModel GetAnalyticsEarnedTicketsReadServiceModel(
            int latestEarnedTickets = 1,
            DateTime? expirationOfSkipLimitTimestamp = null,
            bool isSkipLimitReached = false)
        {
            var fixture = new Fixture();

            var builder = fixture.Build<AnalyticsLastEarnedTicketsReadServiceModel>()
                .With(x => x.ExpirationOfSkipLimitTimestamp, expirationOfSkipLimitTimestamp)
                .With(x => x.IsSkipLimitReached, isSkipLimitReached)
                .With(x => x.LatestEarnedTickets, latestEarnedTickets);

            return builder.Create();
        }
    }
}
