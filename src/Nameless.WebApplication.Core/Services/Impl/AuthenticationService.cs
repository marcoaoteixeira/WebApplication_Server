using Microsoft.Extensions.Options;
using Nameless.WebApplication.Settings;

namespace Nameless.WebApplication.Services.Impl {

    public sealed class AuthenticationService : IAuthenticationService {

        #region Private Read-Only Fields

        private readonly IUserService _userService;
        private readonly IJsonWebTokenService _jsonWebTokenService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IClock _clock;
        private readonly WebApplicationSettings _settings;

        #endregion

        #region Public Constructors

        public AuthenticationService(IUserService userService, IJsonWebTokenService jsonWebTokenService, IRefreshTokenService refreshTokenService, IClock clock, IOptions<WebApplicationSettings> settings) {
            Prevent.Null(userService, nameof(userService));
            Prevent.Null(jsonWebTokenService, nameof(jsonWebTokenService));
            Prevent.Null(refreshTokenService, nameof(refreshTokenService));
            Prevent.Null(clock, nameof(clock));
            Prevent.Null(settings, nameof(settings));

            _userService = userService;
            _jsonWebTokenService = jsonWebTokenService;
            _refreshTokenService = refreshTokenService;
            _clock = clock;
            _settings = settings.Value;
        }

        #endregion

        #region IAuthenticationService Members

        public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request, CancellationToken cancellationToken = default) {
            var user = await _userService.GetByEmailAsync(request.Email, cancellationToken);
            if (user == null) { return new AuthenticationResponse { Reason = "Username or password incorrect." }; }

            if (!HashUtil.Validate(request.Password, user.Password)) {
                return new AuthenticationResponse { Reason = "Username or password incorrect." };
            }

            var jsonWebToken = await _jsonWebTokenService.GenerateTokenAsync(user.ID.ToString(), cancellationToken);
            //var refreshToken = await _refreshTokenService.GenerateAsync(cancellationToken);

            //user.RefreshTokens.Add(refreshToken);
            //user.RefreshTokens.RemoveAll(_ =>
            //    !_.IsActive(_clock) &
            //    _.CreatedIn.AddDays(_settings.Jwt.RefreshTokenTtl) <= _clock.UtcNow
            //);

            await _userService.UpdateAsync(user, cancellationToken);

            return new AuthenticationResponse {
                Token = jsonWebToken,
                //RefreshToken = refreshToken.Token
            };
        }

        #endregion
    }
}
