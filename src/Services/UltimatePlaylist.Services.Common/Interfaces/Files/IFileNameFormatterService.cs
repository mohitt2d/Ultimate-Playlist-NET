#region Usings

using System;

#endregion

namespace UltimatePlaylist.Services.Common.Interfaces.Files
{
    public interface IFileNameFormatterService
    {
        string Format(string fileName);

        string Format(string fileName, DateTime timestamp);

        string Format(string fileName, DateTime timestamp, Guid guid);
    }
}
