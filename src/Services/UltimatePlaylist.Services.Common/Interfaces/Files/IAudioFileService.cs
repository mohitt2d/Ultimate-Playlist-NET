#region Usings

using System.IO;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Services.Common.Models.Files;
using UltimatePlaylist.Services.Common.Models.Media;

#endregion

namespace UltimatePlaylist.Services.Common.Interfaces.Files
{
    public interface IAudioFileService
    {
        Task<Result<AudioFileReadServiceModel>> MarkAsCanceledAsync(string jobName);

        Task<Result<AudioFileReadServiceModel>> MarkAsErrorAsync(
            string jobName,
            MediaServicesJobErrorCode errorCode,
            string errorMessage);

        Task<Result<AudioFileReadServiceModel>> MarkAsFinishedAsync(
            string jobName,
            AudioAssetPublishedReadServiceModel audioAssetPublishedReadServiceModel);

        Task<Result<AudioFileReadServiceModel>> MarkAsProcessingAsync(string jobName);

        Task<Result<AudioFileReadServiceModel>> MarkAsQueuedAsync(AudioFileQueuedWriteServiceModel writeServiceModel);

        Task<Result<AudioFileReadServiceModel>> MarkAsScheduledAsync(string jobName);

        Task<FileReadServiceModel> SaveNewAudioFileAsync(
            string audioAssetName,
            string containerName,
            Stream fileStream,
            string fileName);

        Task RemoveAudioFileAsync(string fileName);

        Task<FileReadServiceModel> SaveWithoutMediaServiceAudioFileAsync(
            Stream fileStream,
            string fileName);
    }
}
