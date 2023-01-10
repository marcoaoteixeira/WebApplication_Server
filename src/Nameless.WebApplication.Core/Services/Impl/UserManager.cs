using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nameless.WebApplication.Collections.Generic;
using Nameless.WebApplication.Entities;
using Nameless.WebApplication.Settings;

namespace Nameless.WebApplication.Services.Impl {

    public sealed class UserManager : IUserManager {

        #region Private Constants

        private const int REFRESH_TOKEN_BYTE_COUNT = 256;

        #endregion

        #region Private Read-Only Fields

        private readonly WebApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<UserManager> _logger;
        private readonly RefreshTokenSettings _refreshTokenSettings;
        private readonly IClock _clock;


        #endregion

        #region Public Constructors

        public UserManager(
            WebApplicationDbContext dbContext,
            IHttpContextAccessor httpContextAccessor,
            ILogger<UserManager> logger,
            IOptions<RefreshTokenSettings> refreshTokenSettings,
            IClock? clock = null
        ) {
            Prevent.Null(dbContext, nameof(dbContext));
            Prevent.Null(httpContextAccessor, nameof(httpContextAccessor));
            Prevent.Null(logger, nameof(logger));

            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _refreshTokenSettings = refreshTokenSettings.Value ?? new();
            _clock = clock ?? SystemClock.Instance;
        }

        #endregion

        #region IUserManager Members

        public Task<bool> AnyAsync(Expression<Func<User, bool>> predicate, CancellationToken cancellationToken = default) {
            return _dbContext.Users.AnyAsync(predicate, cancellationToken);
        }

        public Task CreateAsync(User user, CancellationToken cancellationToken = default) {
            Prevent.Null(user, nameof(user));

            var now = _clock.UtcNow;

            user.ID = Guid.NewGuid();
            user.Password = HashUtil.Hash(user.Password);
            user.CreationDate = now;

            _dbContext.Users.Add(user);

            return _dbContext.SaveChangesAsync(cancellationToken);
        }

        public Task DeleteAsync(Guid userID, CancellationToken cancellationToken = default) {
            // Can't delete user if not logged in.
            if (_httpContextAccessor.HttpContext?.User == null) {
                _logger.LogInformation("User not logged.");

                return Task.CompletedTask;
            }

            var claim = _httpContextAccessor.HttpContext
                .User
                .Claims
                .SingleOrDefault(_ => _.Type == JwtRegisteredClaimNames.Sub);

            // User without JWT Sub claim.
            if (claim == null) {
                _logger.LogInformation("JWT Sub claim not present.");

                return Task.CompletedTask;
            }

            var currentUserID = Guid.Parse(claim.Value);

            // Can't delete own user.
            if (currentUserID == userID) {
                _logger.LogInformation("Can't delete own user.");

                return Task.CompletedTask;
            }

            return _dbContext.Users.Where(_ => _.ID == userID).ExecuteDeleteAsync(cancellationToken);
        }

        public async Task UpdateAsync(Guid userID, User user, CancellationToken cancellationToken = default) {
            Prevent.Null(user, nameof(user));

            var currentUser = await _dbContext.Users.SingleOrDefaultAsync(_ => _.ID == userID, cancellationToken);
            if (currentUser == null) {
                _logger.LogInformation($"User not found with ID: {userID}.");

                return;
            }

            currentUser.Username = user.Username;

            if (!string.IsNullOrWhiteSpace(user.Email)) {
                var userWithSameEmail = await _dbContext.Users.AnyAsync(_ => _.Email == user.Email, cancellationToken);
                if (userWithSameEmail) {
                    _logger.LogInformation("User with same email already exists.");

                    return;
                }
                currentUser.Email = user.Email;
            }

            currentUser.Role = user.Role;
            currentUser.Locked = user.Locked;
            currentUser.ModificationDate = _clock.UtcNow;

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task ChangePasswordAsync(Guid userID, string previousPassword, string newPassword, CancellationToken cancellationToken = default) {
            Prevent.NullOrWhitespaces(previousPassword, nameof(previousPassword));
            Prevent.NullOrWhitespaces(newPassword, nameof(newPassword));

            var user = await _dbContext.Users.SingleOrDefaultAsync(_ => _.ID == userID, cancellationToken);
            if (user == null) {
                _logger.LogInformation($"User not found with ID: {userID}.");

                return;
            }

            if (!HashUtil.Validate(previousPassword, user.Password)) {
                _logger.LogInformation("Previous password didn't match.");

                return;
            }

            user.Password = HashUtil.Hash(newPassword);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public Task<User?> GetByIDAsync(Guid userID, CancellationToken cancellationToken = default) {
            return _dbContext
                .Users
                .Include(_ => _.Claims)
                .Include(_ => _.RefreshTokens)
                .SingleOrDefaultAsync(_ => _.ID == userID, cancellationToken);
        }

        public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default) {
            return _dbContext
                .Users
                .Include(_ => _.Claims)
                .Include(_ => _.RefreshTokens)
                .SingleOrDefaultAsync(_ => _.Email == email, cancellationToken);
        }

        public Task<IPage<User>> PaginateAsync(int index, int size, Expression<Func<User, bool>> where, Expression<Func<User, object>>? orderBy = null, OrderDirection orderDirection = OrderDirection.Ascending, CancellationToken cancellationToken = default) {
            Prevent.Null(where, nameof(where));

            var users = _dbContext.Users.Where(where);

            if (orderBy != null) {
                users = orderDirection == OrderDirection.Ascending
                    ? users.OrderBy(orderBy)
                    : users.OrderByDescending(orderBy);
            }

            return Task.FromResult(users.AsPage(index, size));
        }

        public async Task AddClaimsAsync(Guid userID, IEnumerable<Claim> claims, CancellationToken cancellationToken = default) {
            Prevent.Null(claims, nameof(claims));

            var user = await _dbContext.Users.SingleOrDefaultAsync(_ => _.ID == userID, cancellationToken);
            if (user == null) {
                _logger.LogInformation($"User not found with ID: {userID}.");

                return;
            }

            var now = _clock.UtcNow;
            foreach (var claim in claims) {
                claim.ID = Guid.NewGuid();
                claim.CreationDate = now;
                claim.Owner = user;
            }
            user.Claims.AddRange(claims);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task RemoveClaimsAsync(Guid userID, IEnumerable<Claim> claims, CancellationToken cancellationToken = default) {
            Prevent.Null(claims, nameof(claims));

            var user = await _dbContext.Users.SingleOrDefaultAsync(_ => _.ID == userID, cancellationToken);
            if (user == null) {
                _logger.LogInformation($"User not found with ID: {userID}.");

                return;
            }

            foreach (var claim in claims) {
                user.Claims.RemoveAll(_ => _.Name == claim.Name);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<string?> GenerateRefreshTokenAsync(Guid userID, CancellationToken cancellationToken = default) {
            var user = await _dbContext.Users.SingleOrDefaultAsync(_ => _.ID == userID, cancellationToken);
            if (user == null) {
                _logger.LogInformation($"User not found with ID: {userID}.");

                return null;
            }

            var now = _clock.UtcNow;
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(REFRESH_TOKEN_BYTE_COUNT));

            user.RefreshTokens.Add(new RefreshToken {
                Token = token,
                ExpiresIn = now.AddDays(_refreshTokenSettings.Ttl),
                CreatedIn = now,
                CreatedByIp = _httpContextAccessor.HttpContext.GetIpAddress(),
                CreationDate = now,
                Owner = user
            });

            user.RefreshTokens.RemoveAll(_ =>
                !_.IsActive(now) &
                _.CreatedIn.AddDays(_refreshTokenSettings.Ttl) <= now
            );

            await _dbContext.SaveChangesAsync(cancellationToken);

            return token;
        }

        public async Task RevokeRefreshToken(Guid userID, string token, string? reason = default, CancellationToken cancellationToken = default) {
            Prevent.NullOrWhitespaces(token, nameof(token));

            var refreshToken = await _dbContext.RefreshTokens
                .Include(_ => _.Owner)
                .SingleOrDefaultAsync(_ => _.Owner.ID == userID && _.Token == token, cancellationToken);

            if (refreshToken == null || !refreshToken.IsActive(_clock.UtcNow)) {
                _logger.LogInformation("Can't revoke token. Not found or not active.");

                return;
            }

            var now = _clock.UtcNow;
            refreshToken.RevokedIn = now;
            refreshToken.RevokedByIp = _httpContextAccessor.HttpContext.GetIpAddress();
            refreshToken.RevokeReason = reason;
            refreshToken.ReplacedByToken = null;
            refreshToken.ModificationDate = now;

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        #endregion
    }
}
