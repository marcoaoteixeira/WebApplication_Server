using System.Linq.Expressions;
using Nameless.WebApplication.Collections.Generic;
using Nameless.WebApplication.Entities;

namespace Nameless.WebApplication.Services {

    public interface IUserManager {

        #region Methods

        Task<bool> AnyAsync(Expression<Func<User, bool>> predicate, CancellationToken cancellationToken = default);
        Task CreateAsync(User user, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid userID, CancellationToken cancellationToken = default);
        Task UpdateAsync(Guid userID, User user, CancellationToken cancellationToken = default);
        Task ChangePasswordAsync(Guid userID, string previousPassword, string newPassword, CancellationToken cancellationToken = default);
        Task<User?> GetByIDAsync(Guid userID, CancellationToken cancellationToken = default);
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<IPage<User>> PaginateAsync(int index, int size, Expression<Func<User, bool>> where, Expression<Func<User, object>>? orderBy = null, OrderDirection orderDirection = OrderDirection.Ascending, CancellationToken cancellationToken = default);
        Task AddClaimsAsync(Guid userID, IEnumerable<Claim> claims, CancellationToken cancellationToken = default);
        Task RemoveClaimsAsync(Guid userID, IEnumerable<Claim> claims, CancellationToken cancellationToken = default);
        Task<string?> GenerateRefreshTokenAsync(Guid userID, CancellationToken cancellationToken = default);
        Task RevokeRefreshToken(Guid userID, string token, string? reason = default, CancellationToken cancellationToken = default);

        #endregion
    }
}
