using Nameless.WebApplication.Logging.log4net;
using Nameless.WebApplication.Settings;

namespace Nameless.WebApplication {

    public partial class StartUp {

        #region Private Static Methods

        private static void ConfigureOptions(IServiceCollection services, IConfiguration configuration) {
            services.AddOptions();

            services
                .Configure<Log4netSettings>(configuration.GetSection(GetSectionKey<Log4netSettings>()))
                .Configure<SwaggerPageSettings>(configuration.GetSection(GetSectionKey<SwaggerPageSettings>()))
                .Configure<JsonWebTokenSettings>(configuration.GetSection(GetSectionKey<JsonWebTokenSettings>()));
        }

        #endregion

        #region Private Static Methods

        private static string GetSectionKey<TNode>() {
            return typeof(TNode).Name
                .Replace("Options", string.Empty)
                .Replace("Settings", string.Empty);
        }

        #endregion
    }
}
