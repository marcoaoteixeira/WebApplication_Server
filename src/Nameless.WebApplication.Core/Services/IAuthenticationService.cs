namespace Nameless.WebApplication.Services {

    public interface IAuthenticationService {

        #region Methods

        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request, CancellationToken cancellationToken = default);

        #endregion
    }

    public sealed class AuthenticationRequest {

        #region Public Properties

        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;

        #endregion
    }

    public sealed class AuthenticationResponse {

        #region Public Properties

        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public string? Reason { get; set; }
        public bool Success => Reason == null;

        #endregion
    }
}
