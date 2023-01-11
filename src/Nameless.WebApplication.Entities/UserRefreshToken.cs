using System.ComponentModel.DataAnnotations;

namespace Nameless.WebApplication.Entities {

    public sealed class UserRefreshToken : EntityBase<Guid> {

        #region Public Properties

        [MaxLength(4096)]
        public string Token { get; set; } = default!;
        public DateTime ExpiresIn { get; set; }
        public DateTime CreatedIn { get; set; }
        [MaxLength(64)]
        public string CreatedByIp { get; set; } = default!;
        public DateTime? RevokedIn { get; set; }
        [MaxLength(64)]
        public string? RevokedByIp { get; set; }
        [MaxLength(4096)]
        public string? ReplacedByToken { get; set; }
        [MaxLength(1024)]
        public string? RevokeReason { get; set; }
        public Guid UserId { get; set; } = default!;

        #endregion

        #region Public Methods

        public bool IsRevoked() => RevokedIn != null;

        public bool IsExpired(DateTime date = default) => (date == DateTime.MinValue ? DateTime.UtcNow : date) >= ExpiresIn;

        public bool IsActive(DateTime date = default) => !IsRevoked() && !IsExpired(date);

        #endregion
    }
}
