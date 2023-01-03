namespace Nameless.WebApplication.Domain.Dtos.Common {

    public sealed class AuthenticationRequest {

        #region Public Properties

        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;

        #endregion
    }
}
