#region Usings

using System.IO;
using System.Threading.Tasks;
using UltimatePlaylist.Services.Common.Models.Media;

#endregion

namespace UltimatePlaylist.Services.Common.Interfaces.Media
{
    public interface IMediaService
    {
        Task<AudioAssetCreatedReadServiceModel> CreateAudioAssetAsync(
            Stream fileStream,
            string assetName);

        Task DeleteAudioAssetAsync(string assetName);

        Task<AudioAssetPublishedReadServiceModel> PublishAudioAssetAsync(string assetName);
    }
}
