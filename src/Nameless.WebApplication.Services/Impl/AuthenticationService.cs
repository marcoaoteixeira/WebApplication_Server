using Microsoft.AspNetCore.Identity;
using Nameless.WebApplication.Domain;
using Nameless.WebApplication.Entities;

namespace Nameless.WebApplication.Services.Impl {

    public sealed class AuthenticationService : IAuthenticationService {

        #region Private Read-Only Fields

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IJsonWebTokenService _jsonWebTokenService;

        #endregion

        #region Public Constructors

        public AuthenticationService(UserManager<User> userManager, SignInManager<User> signInManager, IJsonWebTokenService jsonWebTokenService) {
            Prevent.Null(userManager, nameof(userManager));
            Prevent.Null(signInManager, nameof(signInManager));
            Prevent.Null(jsonWebTokenService, nameof(jsonWebTokenService)); ;

            _userManager = userManager;
            _signInManager = signInManager;
            _jsonWebTokenService = jsonWebTokenService;
        }

        #endregion

        #region IAuthenticationService Members

        public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request, CancellationToken cancellationToken = default) {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null) { return new AuthenticationResponse { Reason = "Username or password incorrect." }; }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, isPersistent: false, lockoutOnFailure: true);
            if (result.IsLockedOut) { return new AuthenticationResponse { Reason = "User locked out." }; }
            if (!result.Succeeded) { return new AuthenticationResponse { Reason = "Username or password incorrect." }; }

            var jsonWebToken = await _jsonWebTokenService.GenerateTokenAsync(user.Id.ToString(), cancellationToken);
            //var refreshToken = await _userManager.GenerateRefreshTokenAsync(user.Id, cancellationToken);

            return new AuthenticationResponse {
                Token = jsonWebToken,
                RefreshToken = string.Empty
            };
        }

        public Task<bool> RevokeTokenAsync(string token, CancellationToken cancellationToken = default) {
            throw new NotImplementedException();
        }

        #endregion
    }
}
