#region Usings

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using UltimatePlaylist.Common.Const;
using UltimatePlaylist.Services.Common.Interfaces.AppleMusic;

#endregion

namespace UltimatePlaylist.Services.AppleMusic
{
    public class AppleMusicTokenService : IAppleMusicTokenService
    {
        #region Private members

        private readonly AppleMusicProviderConfig AppleConfig;

        #endregion

        #region Constructor(s)

        public AppleMusicTokenService(IOptions<AppleMusicProviderConfig> appleConfig)
        {
            AppleConfig = appleConfig.Value;
        }

        #endregion

        #region Public methods

        public string CreateAppleMusicToken()
        {
            var dateTimeNow = DateTime.UtcNow;
            var key = GetECDsaKey(AppleConfig.PrivateKey);
            var securityKey = new ECDsaSecurityKey(key) { KeyId = AppleConfig.KeyId };
            var descriptor = new SecurityTokenDescriptor
            {
                IssuedAt = dateTimeNow,
                Issuer = AppleConfig.TeamId,
                //Expires = dateTimeNow.AddMonths(6),
                SigningCredentials = new SigningCredentials(securityKey, "ES256"),
            };

            var handler = new JwtSecurityTokenHandler();
            var encodedToken = handler.CreateEncodedJwt(descriptor);
            return encodedToken;
        }

        #endregion

        #region Private methods

        private ECDsaCng GetECDsaKey(string privateKey) => new ECDsaCng(CngKey.Import(Convert.FromBase64String(privateKey), CngKeyBlobFormat.Pkcs8PrivateBlob));

        #endregion
    }
}
