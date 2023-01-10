using Nameless.WebApplication.Services.Models;

namespace Nameless.WebApplication.Services.Impl {

    public sealed class AuthenticationService : IAuthenticationService {

        #region Private Read-Only Fields

        private readonly IUserManager _userManager;
        private readonly IJsonWebTokenService _jsonWebTokenService;

        #endregion

        #region Public Constructors

        public AuthenticationService(IUserManager userManager, IJsonWebTokenService jsonWebTokenService) {
            Prevent.Null(userManager, nameof(userManager));
            Prevent.Null(jsonWebTokenService, nameof(jsonWebTokenService)); ;

            _userManager = userManager;
            _jsonWebTokenService = jsonWebTokenService;
        }

        #endregion

        #region IAuthenticationService Members

        public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request, CancellationToken cancellationToken = default) {
            var user = await _userManager.GetByEmailAsync(request.Email, cancellationToken);
            if (user == null) { return new AuthenticationResponse { Reason = "Username or password incorrect." }; }

            if (!HashUtil.Validate(request.Password, user.Password)) {
                return new AuthenticationResponse { Reason = "Username or password incorrect." };
            }

            var jsonWebToken = await _jsonWebTokenService.GenerateTokenAsync(user.ID.ToString(), cancellationToken);
            var refreshToken = await _userManager.GenerateRefreshTokenAsync(user.ID, cancellationToken);

            return new AuthenticationResponse {
                Token = jsonWebToken,
                RefreshToken = refreshToken
            };
        }

        #endregion
    }
}
