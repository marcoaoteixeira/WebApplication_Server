using Microsoft.EntityFrameworkCore;

namespace Nameless.WebApplication.Entities {

    public sealed class WebApplicationDbContext : DbContext {

        #region Public Properties

        public DbSet<User> Users => Set<User>();
        public DbSet<Claim> Claims => Set<Claim>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        #endregion

        #region Public Constructors

        public WebApplicationDbContext(DbContextOptions<WebApplicationDbContext> opts)
            : base(opts) { }

        #endregion

        #region Protected Override Methods

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<User>()
                .Property(_ => _.Role)
                .HasConversion(
                    convertToProviderExpression: _ => _.ToString(),
                    convertFromProviderExpression: _ => Enum.Parse<Roles>(_)
                );
        }

        #endregion
    }
}