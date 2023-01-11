﻿using Nameless.WebApplication.Entities;

namespace Nameless.WebApplication {

    public partial class StartUp {

        #region Private Static Methods

        private static void ConfigureIdentity(IServiceCollection services) {
            services
                .AddIdentity<User, Role>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
        }

        #endregion
    }
}
