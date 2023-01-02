using Nameless.WebApplication.Logging.log4net;
using Nameless.WebApplication.Settings;

namespace Nameless.WebApplication {

    public partial class StartUp {

        #region Private Static Methods

        private static void ConfigureOptions(IServiceCollection services, IConfiguration configuration) {
            services.AddOptions();

            services
                .Configure<SwaggerPageSettings>(configuration.GetSection(GetOptionName<SwaggerPageSettings>()))
                .Configure<WebApplicationSettings>(configuration.GetSection(GetOptionName<WebApplicationSettings>()))
                .Configure<Log4netSettings>(configuration.GetSection(GetOptionName<Log4netSettings>()));
        }

        #endregion

        #region Private Static Methods

        private static string GetOptionName<TOptions>() {
            return typeof(TOptions).Name
                .Replace("Options", string.Empty)
                .Replace("Settings", string.Empty);
        }

        #endregion
    }
}
