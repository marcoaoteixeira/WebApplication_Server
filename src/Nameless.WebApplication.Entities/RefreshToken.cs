namespace Nameless.WebApplication.Entities {

    public sealed class RefreshToken : EntityBase {

        #region Public Properties

        public string Token { get; set; } = null!;
        public DateTime ExpiresIn { get; set; }
        public DateTime CreatedIn { get; set; }
        public string CreatedByIp { get; set; } = null!;
        public DateTime? RevokedIn { get; set; }
        public string? RevokedByIp { get; set; }
        public string? ReplacedByToken { get; set; }
        public string? RevokeReason { get; set; }
        public User Owner { get; set; } = null!;

        #endregion

        #region Public Methods

        public bool IsRevoked() => RevokedIn != null;

        public bool IsExpired(DateTime date = default) => (date == DateTime.MinValue ? DateTime.UtcNow : date) >= ExpiresIn;

        public bool IsActive(DateTime date = default) => !IsRevoked() && !IsExpired(date);

        #endregion
    }
}
