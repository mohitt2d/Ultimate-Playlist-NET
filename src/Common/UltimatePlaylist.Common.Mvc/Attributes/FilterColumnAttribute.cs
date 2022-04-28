#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using UltimatePlaylist.Common.Mvc.Attributes.Enums;

#endregion

namespace UltimatePlaylist.Common.Mvc.Attributes
{
    public class FilterColumnAttribute : Attribute
    {
        #region Constructor(s)

        public FilterColumnAttribute(PropertyName name, params string[] pars)
        {
            Name = name.GetName();
            if (pars != null)
            {
                NamedValues = pars;
            }
        }

        public FilterColumnAttribute(PropertyName name, object[] overriden = null, string[] without = null)
        {
            Name = name.GetName();
            if (overriden != null)
            {
                OverridenBy = new KeyValuePair<string, Type>((string)overriden[0], (Type)overriden[1]);
            }

            if (without != null)
            {
                Without = without.ToList();
            }
        }

        #endregion

        #region Public Properties

        public string Name { get; }

        public string[] NamedValues { get; }

        public KeyValuePair<string, Type>? OverridenBy { get; }

        public List<string> Without { get; }

        #endregion
    }
}
