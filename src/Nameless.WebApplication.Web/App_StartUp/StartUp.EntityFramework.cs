using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Nameless.WebApplication.Entities;
using Nameless.WebApplication.Services;

namespace Nameless.WebApplication {

    public partial class StartUp {

        #region Private Static Methods

        private static void ConfigureEntityFramework(IServiceCollection services, IHostEnvironment hostEnvironment, IConfiguration configuration) {
            services.AddDbContext<WebApplicationDbContext>(opts => {
                // Get the environment variable telling that we're running on Docker
                var isDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER");

                var connectionStringName = string.Equals(isDocker, bool.TrueString, StringComparison.OrdinalIgnoreCase)
                    ? $"{nameof(WebApplicationDbContext)}_Docker"
                    : $"{nameof(WebApplicationDbContext)}";
                
                var connectionString = configuration.GetConnectionString(connectionStringName);

                opts.UseSqlServer(connectionString);
            });
        }

        private static void UseEntityFramework(IApplicationBuilder applicationBuilder) {
            using var dbContext = applicationBuilder.ApplicationServices.GetService<WebApplicationDbContext>();
            if (dbContext != null) {
                // ensure db migrations run
                dbContext.Database.Migrate();

                // seeding database
                var clock = applicationBuilder.ApplicationServices.GetService<IClock>();
                var now = clock?.UtcNow ?? DateTime.UtcNow;
                // Add admin user
                const string adminUsername = "administrator@webapplication.com";
                var adminUser = dbContext.Users.SingleOrDefault(_ => _.Username == adminUsername);
                if (adminUser == null) {
                    adminUser = new() {
                        ID = Guid.Parse("7a32afaa-1252-4b78-a40e-c0543d9f24d5"),
                        Username = adminUsername,
                        Email = adminUsername,
                        Password = HashUtil.Hash("123456abc@"),
                        Locked = false,
                        CreationDate = now,
                        ModificationDate = now,
                        Claims = new List<Entities.Claim> {
                            new Entities.Claim {
                                ID = Guid.Parse("c34752b2-2c82-4385-a8b3-ec88b3924aef"),
                                Name = ClaimTypes.Role,
                                Value = Roles.SystemAdministrator.GetDescription(),
                                CreationDate = now,
                                ModificationDate = now,
                            }
                        }
                    };

                    dbContext.Users.Add(adminUser);
                    dbContext.SaveChanges();
                }
            }
        }

        #endregion
    }
}
