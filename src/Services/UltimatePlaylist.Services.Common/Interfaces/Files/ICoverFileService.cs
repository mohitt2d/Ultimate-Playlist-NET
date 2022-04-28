#region Usings

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltimatePlaylist.Services.Common.Models.Files;

#endregion

namespace UltimatePlaylist.Services.Common.Interfaces.Files
{
    public interface ICoverFileService
    {
        Task<FileReadServiceModel> SaveNewFileAsync(Stream fileStream, string fileName);

        Task RemoveCoverAsync(Guid externalId);
    }
}
