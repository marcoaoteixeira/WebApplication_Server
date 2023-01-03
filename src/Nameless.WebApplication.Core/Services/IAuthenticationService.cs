using Nameless.WebApplication.Domain.Dtos.Common;

namespace Nameless.WebApplication.Services {

    public interface IAuthenticationService {

        #region Methods

        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request, CancellationToken cancellationToken = default);

        #endregion
    }
}
