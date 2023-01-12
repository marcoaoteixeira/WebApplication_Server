namespace Nameless.WebApplication.Domain {

    public sealed class AuthenticationRequest {

        #region Public Properties

        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;

        #endregion
    }
}
