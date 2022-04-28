#region Usings

using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using UltimatePlaylist.Services.Common.Models.Analytics;

#endregion

namespace UltimatePlaylist.Services.Common.Interfaces.Song
{
    public interface IAnalyticsService
    {
        Task<Result<AnalyticsLastEarnedTicketsReadServiceModel>> SaveAnalitycsDataAsync(
            Guid userExternalId,
            SaveAnalyticsDataWriteServiceModel saveAnalyticsDataWriteServiceModel);
    }
}
