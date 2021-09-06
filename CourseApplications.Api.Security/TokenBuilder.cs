using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace CourseApplications.Api.Security
{
    public class TokenBuilder : ITokenBuilder
    {
        public string Build(string name, string[] roles, DateTime expireDate)
        {
            var _tokenHandler = new JwtSecurityTokenHandler();

            var claims = new List<Claim>();

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var claimsIdentity = new ClaimsIdentity(new GenericIdentity(name, "Bearer"), claims);

            var token = _tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = TokenAuthOptions.Issuer,
                Audience = TokenAuthOptions.Audience,
                SigningCredentials = TokenAuthOptions.SigningCredentials,
                Subject = claimsIdentity,
                Expires = expireDate
            });

            return _tokenHandler.WriteToken(token);
        }
    }

    public class TokenAuthOptions
    {
        public static string Audience { get; } = "CourseAppAudience";
        public static string Issuer { get; } = "CourseAppIssuer";
        public static RsaSecurityKey Key { get; } = new RsaSecurityKey(RSAKeyHelper.GenerateKey());
        public static SigningCredentials SigningCredentials { get; } = new SigningCredentials(Key, SecurityAlgorithms.RsaSha256Signature);
        public static TimeSpan ExpiresSpan { get; } = TimeSpan.FromMinutes(30);
        public static string TokenType { get; } = "Bearer";
    }

    public class RSAKeyHelper
    {
        public static RSAParameters GenerateKey()
        {
            using (var key = new RSACryptoServiceProvider(2048))
            {
                return key.ExportParameters(true);
            }
        }
    }
}
