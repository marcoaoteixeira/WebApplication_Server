using FluentValidation;

namespace Nameless.WebApplication {
    
    public partial class StartUp {

        #region Private Static Methods

        private static void ConfigureFluentValidation(IServiceCollection services) {
            services
                .AddValidatorsFromAssemblies(
                    assemblies: new[] {
                        typeof(StartUp).Assembly,
                        typeof(ApiControllerBase).Assembly
                    }
                );
        }

        #endregion
    }
}
