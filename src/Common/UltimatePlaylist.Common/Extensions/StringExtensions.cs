#region Usings

#endregion

namespace UltimatePlaylist.Common.Extensions
{
    public static class StringExtensions
    {
        #region FirstToLower(camelCase)

        public static string FirstToLower(this string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return source;
            }

            source = source.Trim();
            var firstLetter = source.Substring(0, 1).ToLower();
            return $"{firstLetter}{source.Substring(1)}";
        }

        #endregion

        #region FirstToUpper(PascalCase)

        public static string FirstToUpper(this string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return source;
            }

            source = source.Trim();
            var firstLetter = source.Substring(0, 1).ToUpper();
            return $"{firstLetter}{source.Substring(1)}";
        }

        #endregion

        #region LowerCase

        public static string NullableLowerCase(this string source)
        {
            return string.IsNullOrEmpty(source) ? string.Empty : source.ToLower();
        }

        #endregion
    }
}
