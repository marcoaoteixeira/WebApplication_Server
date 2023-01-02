using Nameless.WebApplication.Domain.Entities;

namespace Nameless.WebApplication.Services {

    public interface IRefreshTokenService {

        #region Methods

        Task<RefreshToken> GenerateAsync(CancellationToken cancellationToken = default);
        Task RevokeAsync(string refreshToken, string? revokeReason, CancellationToken cancellationToken = default);

        #endregion
    }
}