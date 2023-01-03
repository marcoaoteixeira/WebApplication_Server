namespace Nameless.WebApplication.Domain.Entities {

    public class User : EntityBase {

        #region Public Virtual Properties

        public virtual string Username { get; set; } = null!;
        public virtual string Email { get; set; } = null!;
        public virtual string Password { get; set; } = null!;
        public virtual bool Locked { get; set; }
        public virtual List<Claim> Claims { get; set; } = new();
        public virtual List<RefreshToken> RefreshTokens { get; set; } = new();

        #endregion
    }
}
