#region Usings

using System.IO;
using System.Threading.Tasks;
using UltimatePlaylist.Services.Common.Models.Files;

#endregion

namespace UltimatePlaylist.Services.Common.Interfaces.Files
{
    public interface ICloudStorageService
    {
        Task DeleteFileIfExistsAsync(string containerName, string fileName);

        Task<FileUploadInfoReadServiceModel> UploadFileAsync(Stream fileStream, string containerName, string fileName);

        Task<string> GetSecurityLinkAsync(string containerName, string fileName);
    }
}
