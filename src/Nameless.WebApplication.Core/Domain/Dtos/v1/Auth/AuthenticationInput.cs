namespace Nameless.WebApplication.Domain.Dtos.v1.Auth {

    public sealed class AuthenticationInput {

        #region Public Properties

        public string? Email { get; set; }
        public string? Password { get; set; }

        #endregion
    }
}
