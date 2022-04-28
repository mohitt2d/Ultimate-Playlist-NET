#region Usings

using System;
using System.IO;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using UltimatePlaylist.Services.Common.Models.Files;

#endregion

namespace UltimatePlaylist.Services.Common.Interfaces.Files
{
    public interface IAvatarFileService
    {
        Task<FileReadServiceModel> SaveNewAvatarFileAsync(Stream fileStream, string fileName);

        Task RemoveAvatarAsync(Guid externalId);
    }
}
