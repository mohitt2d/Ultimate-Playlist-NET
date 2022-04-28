#region usings

using System.IO;
using System.Security.Cryptography;

#endregion

namespace UltimatePlaylist.Common.Cryptography
{
    public class Crypto
    {
        private static readonly byte[] DefaultIV = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        private enum CryptMode
        {
            ENCODE,
            DECODE,
        }

        public static byte[] AesEncrypt(byte[] data, string key, byte[] iv = null, int? keySize = 128, PaddingMode? mode = null)
        {
            using var alg = new AesCryptoServiceProvider();
            if (keySize.HasValue)
            {
                alg.KeySize = (int)keySize;
            }

            if (mode.HasValue)
            {
                alg.Padding = (PaddingMode)mode;
            }

            return Crypt(CryptMode.ENCODE, alg, data, key, iv ?? DefaultIV);
        }

        public static byte[] AesDecrypt(byte[] data, string key, byte[] iv = null, int? keySize = 128, PaddingMode? mode = null)
        {
            using var alg = new AesCryptoServiceProvider();
            if (keySize.HasValue)
            {
                alg.KeySize = (int)keySize;
            }

            if (mode.HasValue)
            {
                alg.Padding = (PaddingMode)mode;
            }

            return Crypt(CryptMode.DECODE, alg, data, key, iv ?? DefaultIV);
        }

        private static byte[] Crypt(CryptMode mode, SymmetricAlgorithm alg, byte[] data, string key, byte[] iv)
        {
            var derivedKey = new Rfc2898DeriveBytes(key, iv).GetBytes(alg.KeySize / 8);

            using var ms = new MemoryStream();
            using var transformer = mode == CryptMode.ENCODE ? alg.CreateEncryptor(derivedKey, iv) : alg.CreateDecryptor(derivedKey, iv);
            using var cs = new CryptoStream(ms, transformer, CryptoStreamMode.Write);
            cs.Write(data, 0, data.Length);
            cs.Close();

            return ms.ToArray();
        }
    }
}