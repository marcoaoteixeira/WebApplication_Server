using Nameless.WebApplication.Services;
using Nameless.WebApplication.Services.Impl;

namespace Nameless.WebApplication.Domain.Entities {

    public class RefreshToken : EntityBase {

        #region Public Virtual Properties

        public virtual string? Token { get; set; }
        public virtual DateTime ExpiresIn { get; set; }
        public virtual DateTime CreatedIn { get; set; }
        public virtual string? CreatedByIp { get; set; }
        public virtual DateTime? RevokedIn { get; set; }
        public virtual string? RevokedByIp { get; set; }
        public virtual string? ReplacedByToken { get; set; }
        public virtual string? RevokeReason { get; set; }
        public virtual User Owner { get; set; } = null!;

        #endregion

        #region Public Virtual Methods

        public virtual bool IsRevoked() => RevokedIn != null;

        public virtual bool IsExpired(IClock? clock = null) => (clock ?? SystemClock.Instance).UtcNow >= ExpiresIn;

        public virtual bool IsActive(IClock? clock = null) => !IsRevoked() && !IsExpired(clock);

        #endregion
    }
}
