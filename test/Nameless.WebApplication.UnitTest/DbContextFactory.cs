using Microsoft.EntityFrameworkCore;
using Nameless.WebApplication.Entities;

namespace Nameless.WebApplication.UnitTest {

    public sealed class DbContextFactory {

        public static ApplicationDbContext CreateInMemory() {
            var opts = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"database_{Guid.NewGuid():N}")
                .Options;

            var context = new ApplicationDbContext(opts);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            return context;
        }
    }
}
