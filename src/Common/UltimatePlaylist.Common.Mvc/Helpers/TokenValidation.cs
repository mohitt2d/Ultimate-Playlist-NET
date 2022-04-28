#region Usings

using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using UltimatePlaylist.Common.Config;

#endregion

namespace UltimatePlaylist.Common.Mvc.Helpers
{
    public static class TokenValidation
    {
        #region GetTokenValidator

        public static TokenValidationParameters GetTokenValidation(AuthConfig authConfig)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authConfig.JWT.Key));

            var tokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = key,
                ValidIssuer = authConfig.JWT.Issuer,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                RequireExpirationTime = true,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero,
            };
            return tokenValidationParameters;
        }

        #endregion
    }
}