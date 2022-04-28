#region Usings

using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Common.Mvc.Models;

#endregion

namespace UltimatePlaylist.MobileApi.Models.Dsp
{
    public class UserDspResponseModel : BaseResponseModel
    {
        public DspType Type { get; set; }
    }
}
