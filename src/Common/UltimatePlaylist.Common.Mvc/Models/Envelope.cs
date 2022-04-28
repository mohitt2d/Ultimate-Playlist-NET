#region Usings

using System.Collections.Generic;
using UltimatePlaylist.Common.Mvc.Exceptions;

#endregion

namespace UltimatePlaylist.Common.Mvc.Models
{
    public class Envelope<T>
    {
        #region Constructor(s)

        public Envelope(T data)
        {
            Data = data;
        }

        public Envelope(
            T data,
            IList<Error> errors)
        {
            Data = data;
            Errors = errors;
        }

        public Envelope()
        {
        }

        #endregion

        #region Public Properties

        public T Data { get; set; }

        public IList<Error> Errors { get; set; } = new List<Error>();

        public bool HasErrors => Errors.Count > 0;

        #endregion
    }
}
