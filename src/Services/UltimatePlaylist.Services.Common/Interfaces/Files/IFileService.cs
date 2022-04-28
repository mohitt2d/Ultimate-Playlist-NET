#region Usings

using System.Threading.Tasks;
using UltimatePlaylist.Database.Infrastructure.Entities.File;
using UltimatePlaylist.Services.Common.Models.Files;

#endregion

namespace UltimatePlaylist.Services.Common.Interfaces.Files
{
    public interface IFileService<TFile>
        where TFile : BaseFileEntity
    {
        Task<string> GetSecurityLinkAsync(ContainerType containerType, string fileName);

        Task<string> GetSecurityLinkAsync(string containerName, string fileName);
    }
}
