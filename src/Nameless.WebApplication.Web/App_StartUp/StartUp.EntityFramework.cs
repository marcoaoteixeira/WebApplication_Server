using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nameless.WebApplication.Entities;
using Nameless.WebApplication.Services;

namespace Nameless.WebApplication {

    public partial class StartUp {

        #region Private Static Methods

        private static void ConfigureEntityFramework(IServiceCollection services, IHostEnvironment hostEnvironment, IConfiguration configuration) {
            services.AddDbContext<ApplicationDbContext>(opts => {
                // Get the environment variable telling that we're running on Docker
                var isDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER");

                var connectionStringName = string.Equals(isDocker, bool.TrueString, StringComparison.OrdinalIgnoreCase)
                    ? $"{nameof(ApplicationDbContext)}_Docker"
                    : $"{nameof(ApplicationDbContext)}";
                
                var connectionString = configuration.GetConnectionString(connectionStringName);

                opts.UseSqlServer(connectionString);
            });
        }

        private static void UseEntityFramework(IApplicationBuilder applicationBuilder) {

            // Force migration.
            using var dbContext = applicationBuilder.ApplicationServices.GetService<ApplicationDbContext>();
            dbContext?.Database.Migrate();

            using var userManager = applicationBuilder.ApplicationServices.GetService<UserManager<User>>();
            if (userManager != null) {
                const string adminUsername = "administrator@webapplication.com";
                var hasAdminUser = userManager.Users.Any(_ => _.UserName == adminUsername);
                if (!hasAdminUser) {
                    var adminUser = new User {
                        Id = Guid.Parse("7a32afaa-1252-4b78-a40e-c0543d9f24d5"),
                        UserName = adminUsername,
                        NormalizedUserName = adminUsername.ToUpper(),
                        Email = adminUsername,
                        NormalizedEmail = adminUsername.ToUpper(),
                        EmailConfirmed = true
                    };

                    userManager.CreateAsync(adminUser).Wait();
                    userManager.AddPasswordAsync(adminUser, "123456abc@").Wait();
                }
            }
        }

        #endregion
    }
}
