using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Actio.Common.Auth
{
    public class JwtHandler : IJwtHandler
    {
        private readonly JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        private readonly JwtOptions options;
        private readonly SecurityKey securityKey;
        private readonly SigningCredentials signingCredentials;
        private readonly JwtHeader jwtHeader;
        private readonly TokenValidationParameters tokenValidationParameters;

        public JwtHandler(IOptions<JwtOptions> options)
        {
            this.options = options.Value;
            this.securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.options.SecretKey));
            this.signingCredentials = new SigningCredentials(this.securityKey, SecurityAlgorithms.HmacSha256);
            this.jwtHeader = new JwtHeader(this.signingCredentials);
            this.tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidIssuer = this.options.Issuer,
                IssuerSigningKey = this.securityKey
            };
        }

        public JsonWebToken Create(Guid userId)
        {
            var nowUtc = DateTime.UtcNow;
            var expires = nowUtc.AddMinutes(this.options.ExpiryMinutes);
            var centuryBegin = new DateTime(1970, 1, 1).ToUniversalTime();
            var exp = (long)(new TimeSpan(expires.Ticks - centuryBegin.Ticks).TotalSeconds);
            var now = (long)(new TimeSpan(nowUtc.Ticks - centuryBegin.Ticks).TotalSeconds);
            var payload = new JwtPayload
            {
                {"sub", userId},
                {"iss", this.options.Issuer},
                {"iat", now},
                {"exp", exp},
                {"unique_name", userId}
            };
            var jwt = new JwtSecurityToken(this.jwtHeader, payload);
            var token = this.jwtSecurityTokenHandler.WriteToken(jwt);

            return new JsonWebToken
            {
                Token = token,
                Expires = exp
            };
        }
    }
}
