#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace UltimatePlaylist.Common.Mvc.Attributes
{
    public class UltimateColumnAttribute : Attribute
    {
        public UltimateColumnAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
