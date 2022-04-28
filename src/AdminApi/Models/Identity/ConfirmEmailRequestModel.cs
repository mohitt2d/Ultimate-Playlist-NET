#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#endregion

namespace UltimatePlaylist.AdminApi.Models.Identity
{
    public class ConfirmEmailRequestModel
    {
        public string Token { get; set; }
    }
}
