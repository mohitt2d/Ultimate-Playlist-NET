#region Usings

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using UltimatePlaylist.Services.Common.Models.Dsp;
using UltimatePlaylist.Services.Common.Models.Spotify;

#endregion

namespace UltimatePlaylist.Services.Common.Interfaces.Dsp
{
    public interface IDspService
    {
        Task<Result<IReadOnlyList<UserDspReadServiceModel>>> UserConnectedDsps(Guid userExternalId);
    }
}
