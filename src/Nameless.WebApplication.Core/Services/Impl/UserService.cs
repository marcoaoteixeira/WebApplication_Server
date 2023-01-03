using Microsoft.EntityFrameworkCore;
using Nameless.WebApplication.Domain;
using Nameless.WebApplication.Entities;

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

        public async Task<User> CreateAsync(User user, CancellationToken cancellationToken = default) {
            Prevent.Null(user, nameof(user));

            var now = _clock.UtcNow;

            user.ID = Guid.NewGuid();

            user.Password = HashUtil.Hash(user.Password);

            user.CreationDate = now;
            user.ModificationDate = now;

            _dbContext.Users.Add(user);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return user;
        }

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

        public async Task UpdateAsync(User user, CancellationToken cancellationToken = default) {
            Prevent.Null(user, nameof(user));

            var currentUser = await _dbContext.Users.SingleOrDefaultAsync(_ => _.ID == user.ID, cancellationToken);

            if (currentUser == null) {
                return;
            }

            currentUser.Username = user.Username;
            currentUser.ModificationDate = _clock.UtcNow;

            _dbContext.Update(user);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public Task<User[]> SearchAsync(int index, int size, string? searchTerm = default, string? orderBy = default, CancellationToken cancellationToken = default) {
            if (index < 0) { index = 0; }
            if (size < 1) { size = 10; }

            searchTerm ??= string.Empty;

            return _dbContext.Users
                .Where(_ => _.Username.Contains(searchTerm))
                .Skip(index * size)
                .Take(size)
                .ToArrayAsync(cancellationToken);
        }

        #endregion
    }
}
