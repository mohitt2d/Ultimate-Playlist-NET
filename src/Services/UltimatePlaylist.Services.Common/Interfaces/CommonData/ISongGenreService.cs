#region Usings

using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using UltimatePlaylist.Services.Common.Models.CommonData;

#endregion

namespace UltimatePlaylist.Services.Common.Interfaces.Song
{
    public interface ISongGenreService
    {
        Task<Result<IList<SongGenresReadServiceModel>>> Genres();
    }
}
