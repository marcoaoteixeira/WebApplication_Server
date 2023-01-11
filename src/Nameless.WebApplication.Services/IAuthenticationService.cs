using Nameless.WebApplication.Domain;

namespace Nameless.WebApplication.Services {

    public interface IAuthenticationService {

        #region Methods

        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request, CancellationToken cancellationToken = default);

        Task<bool> RevokeTokenAsync(string token, CancellationToken cancellationToken = default);

        #endregion
    }
}
