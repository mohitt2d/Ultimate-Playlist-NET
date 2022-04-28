#region Usings

using System;
using System.Security.Cryptography;
using UltimatePlaylist.Common.Cryptography;

#endregion

namespace UltimatePlaylist.Common.Extensions
{
    public static class GuidExtensions
    {
        #region Methods
        public static Guid GetValueOrNew(this Guid? guid)
        {
            return guid.IsNullOrEmpty() ? Guid.NewGuid() : guid.Value;
        }

        public static bool IsNullOrEmpty(this Guid? guid)
        {
            return guid == null || guid == Guid.Empty;
        }

        public static bool IsEmpty(this Guid guid)
        {
            return guid == Guid.Empty;
        }

        public static bool IsNotEmpty(this Guid guid)
        {
            return guid != Guid.Empty;
        }

        public static Guid Encode(this Guid source, string key)
        {
            return new Guid(Crypto.AesEncrypt(source.ToByteArray(), key, null, 128, PaddingMode.None));
        }

        public static Guid Decode(this Guid source, string key)
        {
            return new Guid(Crypto.AesDecrypt(source.ToByteArray(), key, null, 128, PaddingMode.None));
        }

        #endregion
    }
}
