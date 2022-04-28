#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace UltimatePlaylist.Common.Config
{
    public class FilesConfig
    {
        public FileTypeConfig Audio { get; set; }

        public FileTypeConfig Cover { get; set; }

        public FileTypeConfig Avatar { get; set; }
    }
}
