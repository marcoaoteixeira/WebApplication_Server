using Microsoft.EntityFrameworkCore;
using Nameless.WebApplication.Domain;
using Nameless.WebApplication.Domain.Entities;

namespace Nameless.WebApplication.Services.Impl {

    public sealed class UserService : IUserService {

        #region Private Read-Only Fields

        private readonly WebApplicationDbContext _dbContext;
        private readonly IClock _clock;

        #endregion

        #region Public Constructors

        public UserService(WebApplicationDbContext dbContext, IClock clock) {
            Prevent.Null(dbContext, nameof(dbContext));
            Prevent.Null(clock, nameof(clock));

            _dbContext = dbContext;
            _clock = clock;
        }

        #endregion

        #region IUserService Members

        public Task<User?> GetByIDAsync(Guid id, CancellationToken cancellationToken = default) {
            return _dbContext.Users
                .Include(_ => _.Claims)
                .Include(_ => _.RefreshTokens)
                .SingleOrDefaultAsync(_ => _.ID == id, cancellationToken);
        }

        public Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default) {
            return _dbContext.Users
                .Include(_ => _.Claims)
                .Include(_ => _.RefreshTokens)
                .SingleOrDefaultAsync(_ => _.Username == username, cancellationToken);
        }

        public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default) {
            return _dbContext.Users
                .Include(_ => _.Claims)
                .Include(_ => _.RefreshTokens)
                .SingleOrDefaultAsync(_ => _.Email == email, cancellationToken);
        }

        public Task UpdateAsync(User user, CancellationToken cancellation = default) {
            Prevent.Null(user, nameof(user));

            if (!_dbContext.Users.Any(_ => _.ID == user.ID)) {
                return Task.CompletedTask;
            }

            _dbContext.Update(user);

            return _dbContext.SaveChangesAsync(cancellation);
        }

        #endregion
    }
}
