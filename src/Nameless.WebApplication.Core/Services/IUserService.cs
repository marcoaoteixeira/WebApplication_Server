using Nameless.WebApplication.Entities;

namespace Nameless.WebApplication.Services {

    public interface IUserService {

        #region Methods

        Task<User> CreateAsync(User user, CancellationToken cancellationToken = default);
        Task<User?> GetByIDAsync(Guid id, CancellationToken cancellationToken = default);
        Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task UpdateAsync(User user, CancellationToken cancellationToken = default);
        Task<User[]> SearchAsync(int index, int size, string? searchTerm = default, string? orderBy = default, CancellationToken cancellationToken = default);

        #endregion
    }
}
