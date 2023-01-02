using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Nameless.WebApplication.Domain;
using Nameless.WebApplication.Domain.Entities;
using Nameless.WebApplication.Services;

namespace Nameless.WebApplication {

    public partial class StartUp {

        #region Private Static Methods

        private static void ConfigureEntityFramework(IServiceCollection services, IHostEnvironment hostEnvironment, IConfiguration configuration) {
            services.AddDbContext<WebApplicationDbContext>(opts => {
                var connectionString = configuration.GetConnectionString($"{nameof(WebApplicationDbContext)}_{hostEnvironment.EnvironmentName}");

                if (hostEnvironment.IsDevelopment()) {
                    opts.UseSqlite(connectionString);
                } else {
                    opts.UseSqlServer(connectionString);
                }
            });
        }

        private static void UseEntityFramework(IApplicationBuilder applicationBuilder) {
            // seeding database
            using var dbContext = applicationBuilder.ApplicationServices.GetService<WebApplicationDbContext>();
            if (dbContext != null) {
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
                        Claims = new List<Domain.Entities.Claim> {
                            new Domain.Entities.Claim {
                                ID = Guid.Parse("c34752b2-2c82-4385-a8b3-ec88b3924aef"),
                                Name = ClaimTypes.Role,
                                Value = Roles.System.GetDescription(),
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
