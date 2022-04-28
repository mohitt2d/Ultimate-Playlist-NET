#region Usings

using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using UltimatePlaylist.Services.Common.Models.Analytics;
using UltimatePlaylist.Services.Common.Models.Song;

#endregion

namespace UltimatePlaylist.Services.Common.Interfaces.Song
{
    public interface ISongSkippingService
    {
        Task<Result<SkipSongReadServiceModel>> SkipSongAsync(SkipSongWriteServiceModel skipSongWriteServiceModel);
    }
}
