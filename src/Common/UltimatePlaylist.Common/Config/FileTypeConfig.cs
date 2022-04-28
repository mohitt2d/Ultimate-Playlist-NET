#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace UltimatePlaylist.Common.Config
{
    public class FileTypeConfig
    {
        public List<string> Formats { get; set; }

        public long MaxSizeBytes { get; set; }
    }
}
