using Microsoft.EntityFrameworkCore;
using Nameless.WebApplication.Domain;

namespace Nameless.WebApplication.UnitTest {

    public sealed class DbContextFactory {

        public static WebApplicationDbContext CreateInMemory() {
            var opts = new DbContextOptionsBuilder<WebApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"database_{Guid.NewGuid():N}")
                .Options;

            var context = new WebApplicationDbContext(opts);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            return context;
        }
    }
}
