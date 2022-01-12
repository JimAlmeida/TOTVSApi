using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace TOTVS.Application
{
    public static class JWTFactory
    {
        public static string BuildToken(string issuer, string subject, string audience, DateTime expirationTime, string jwtKey, string jwtId = null, DateTime? issueTime = null)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(jwtKey);

            var _jti = jwtId ?? Guid.NewGuid().ToString();
            var _issTime = issueTime ?? DateTime.Now;

            var jwtClaims = new Dictionary<string, object>
            {
                { JwtRegisteredClaimNames.Sub, subject },
                { JwtRegisteredClaimNames.Jti, _jti },
                { JwtRegisteredClaimNames.Aud, audience },
                { JwtRegisteredClaimNames.Iss, issuer }
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Claims = jwtClaims,
                Expires = expirationTime,
                IssuedAt = _issTime,
                NotBefore = _issTime,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
