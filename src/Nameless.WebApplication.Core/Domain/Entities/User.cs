namespace Nameless.WebApplication.Domain.Entities {

    public class User : EntityBase {

        #region Public Virtual Properties

        public virtual string? Username { get; set; }
        public virtual string? Email { get; set; }
        public virtual string? Password { get; set; }
        public virtual bool Locked { get; set; }
        public virtual List<Claim> Claims { get; set; } = new();
        public virtual List<RefreshToken> RefreshTokens { get; set; } = new();

        #endregion
    }
}
