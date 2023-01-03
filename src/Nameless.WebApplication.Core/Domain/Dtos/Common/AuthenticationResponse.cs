namespace Nameless.WebApplication.Domain.Dtos.Common {

    public sealed class AuthenticationResponse {

        #region Public Properties

        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public string? Reason { get; set; }
        public bool Success => Reason == null;

        #endregion
    }
}
