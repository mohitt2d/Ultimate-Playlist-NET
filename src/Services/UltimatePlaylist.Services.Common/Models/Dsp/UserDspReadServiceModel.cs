#region Usings

using UltimatePlaylist.Common.Enums;

#endregion

namespace UltimatePlaylist.Services.Common.Models.Dsp
{
    public class UserDspReadServiceModel : BaseReadServiceModel
    {
        public DspType Type { get; set; }
    }
}
