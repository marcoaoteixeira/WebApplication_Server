using Nameless.WebApplication.Domain.Entities;

namespace Nameless.WebApplication.Services {

    public interface IUserService {

        #region Methods

        Task<User?> GetByIDAsync(Guid id, CancellationToken cancellationToken = default);
        Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task UpdateAsync(User user, CancellationToken cancellationToken = default);

        #endregion
    }
}
