using Microsoft.EntityFrameworkCore;

namespace Nameless.WebApplication.Entities {

    public sealed class WebApplicationDbContext : DbContext {

        #region Public Properties

        public DbSet<User> Users => Set<User>();
        public DbSet<Claim> Claims => Set<Entities.Claim>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        #endregion

        #region Public Constructors

        public WebApplicationDbContext(DbContextOptions<WebApplicationDbContext> opts)
            : base(opts) { }

        #endregion

        #region Protected Override Methods

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
        }

        #endregion
    }
}