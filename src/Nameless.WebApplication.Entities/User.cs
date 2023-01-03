namespace Nameless.WebApplication.Entities {

    public sealed class User : EntityBase {

        #region Public Properties

        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool Locked { get; set; }
        public List<Claim> Claims { get; set; } = new();
        public List<RefreshToken> RefreshTokens { get; set; } = new();

        #endregion
    }
}
