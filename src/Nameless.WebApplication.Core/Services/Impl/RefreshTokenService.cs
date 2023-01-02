using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Nameless.WebApplication.Domain;
using Nameless.WebApplication.Domain.Entities;
using Nameless.WebApplication.Settings;

namespace Nameless.WebApplication.Services.Impl {

    public sealed class RefreshTokenService : IRefreshTokenService {

        #region Private Read-Only Fields

        private readonly WebApplicationDbContext _dbContext;
        private readonly IClock _clock;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly WebApplicationSettings _settings;

        #endregion

        #region Public Constructors

        public RefreshTokenService(WebApplicationDbContext dbContext, IClock clock, IHttpContextAccessor httpContextAccessor, IOptions<WebApplicationSettings> settings) {
            Prevent.Null(dbContext, nameof(dbContext));
            Prevent.Null(httpContextAccessor, nameof(httpContextAccessor));
            Prevent.Null(settings, nameof(settings));

            _dbContext = dbContext;
            _clock = clock ?? SystemClock.Instance;
            _httpContextAccessor = httpContextAccessor;
            _settings = settings.Value;
        }

        #endregion

        #region IRefreshTokenService Members

        public Task<RefreshToken> GenerateAsync(CancellationToken cancellationToken = default) {
            var now = _clock.UtcNow;
            var result = new RefreshToken {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(256)),
                ExpiresIn = now.AddDays(_settings.Jwt.RefreshTokenTtl),
                CreatedIn = now,
                CreatedByIp = _httpContextAccessor.HttpContext.GetIpAddress(),
                CreationDate = now,
                ModificationDate = now
            };

            return Task.FromResult(result);
        }

        public async Task RevokeAsync(string refreshToken, string? revokeReason, CancellationToken cancellationToken = default) {
            var currentRefreshToken = await _dbContext.RefreshTokens
                .SingleOrDefaultAsync(_ => _.Token == refreshToken, cancellationToken);
            
            if (currentRefreshToken == null) {
                throw new InvalidRefreshTokenException();
            }

            if (!currentRefreshToken.IsActive(_clock)) {
                return;
            }

            var now = _clock.UtcNow;
            currentRefreshToken.RevokedIn = now;
            currentRefreshToken.RevokedByIp = _httpContextAccessor.HttpContext.GetIpAddress();
            currentRefreshToken.RevokeReason = revokeReason;
            currentRefreshToken.ReplacedByToken = null;
            currentRefreshToken.ModificationDate = now;

            _dbContext.Update(currentRefreshToken);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        #endregion
    }
}
