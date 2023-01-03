using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Nameless.WebApplication.Settings;
using SysClaim = System.Security.Claims.Claim;

namespace Nameless.WebApplication.Services.Impl {

    public sealed class JsonWebTokenService : IJsonWebTokenService {

        #region Private Read-Only Fields

        private readonly IClock _clock;
        private readonly JsonWebTokenSettings _jwtSettings;
        private readonly byte[] _secretBytes;

        #endregion

        #region Public Constructors

        public JsonWebTokenService(IClock clock, IOptions<JsonWebTokenSettings> jsonWebTokenSettings) {
            Prevent.Null(clock, nameof(clock));
            Prevent.Null(jsonWebTokenSettings, nameof(jsonWebTokenSettings));

            _clock = clock;
            _jwtSettings = jsonWebTokenSettings.Value;
            _secretBytes = Encoding.UTF8.GetBytes(_jwtSettings.Secret ?? JsonWebTokenSettings.DEFAULT_SECRET);
        }

        #endregion

        #region IJsonWebTokenService Members

        public Task<string> GenerateTokenAsync(string subject, CancellationToken cancellationToken = default) {
            var issuer = _jwtSettings.Issuer ?? string.Empty;
            var audience = _jwtSettings.Audience ?? string.Empty;
            var now = _clock.UtcNow;
            var expires = now.AddMinutes(_jwtSettings.TokenTtl);
            
            var securityToken = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: new[] {
                    new SysClaim(JwtRegisteredClaimNames.Sub, subject),
                    new SysClaim(JwtRegisteredClaimNames.Iss, issuer),
                    new SysClaim(JwtRegisteredClaimNames.Exp, expires.ToString()),
                    new SysClaim(JwtRegisteredClaimNames.Iat, now.ToString()),
                    new SysClaim(JwtRegisteredClaimNames.Aud, audience),
                    new SysClaim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                },
                expires: expires,
                signingCredentials: new SigningCredentials(
                    key: new SymmetricSecurityKey(_secretBytes),
                    algorithm: SecurityAlgorithms.HmacSha256Signature
                )
            );

            var result = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return Task.FromResult(result);
        }

        public Task<bool> ValidateTokenAsync(string token, [NotNullWhen(true)] out string? value, CancellationToken cancellationToken = default) {
            value = null;

            if (token == null) { return Task.FromResult(false); }

            var result = false;
            try {
                new JwtSecurityTokenHandler().ValidateToken(token, new() {

                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    IssuerSigningKey = new SymmetricSecurityKey(_secretBytes),

                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                    
                }, out var validatedToken);

                value = ((JwtSecurityToken)validatedToken)
                    .Claims
                    .Single(_ => _.Type == JwtRegisteredClaimNames.Sub)
                    .Value;
                result = true;
            } catch { }

            return Task.FromResult(result);
        }

        #endregion
    }
}
