using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Nameless.WebApplication.Settings;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Nameless.WebApplication.Versioning {

    /// <summary>
    /// Configures the Swagger generation options.
    /// </summary>
    /// <remarks>This allows API versioning to define a Swagger document per API version after the
    /// <see cref="IApiVersionDescriptionProvider"/> service has been resolved from the service container.</remarks>
    public sealed class SwaggerGenConfigureOptions : IConfigureOptions<SwaggerGenOptions> {

        #region Private Read-Only Fields

        private readonly IApiVersionDescriptionProvider _provider;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly SwaggerPageSettings _settings;

        #endregion

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureSwaggerOptions"/> class.
        /// </summary>
        /// <param name="provider">The <see cref="IApiVersionDescriptionProvider">provider</see> used to generate Swagger documents.</param>
        /// <param name="hostEnvironment">The <see cref="IHostEnvironment"/>hostEnvironment.</param>
        /// <param name="settings">The <see cref="SwaggerPageSettings"/>applicationOptions.</param>
        public SwaggerGenConfigureOptions(IApiVersionDescriptionProvider provider, IHostEnvironment hostEnvironment, IOptions<SwaggerPageSettings> settings) {
            Prevent.Null(provider, nameof(provider));
            Prevent.Null(hostEnvironment, nameof(hostEnvironment));
            Prevent.Null(settings, nameof(settings));

            _provider = provider;
            _hostEnvironment = hostEnvironment;
            _settings = settings.Value;
        }

        #endregion

        #region Private Static Methods

        private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description, IHostEnvironment hostEnvironment, SwaggerPageSettings settings) {
            return new OpenApiInfo {
                Title = hostEnvironment.ApplicationName,
                Version = description.ApiVersion.ToString(),
                Description = $"{(description.IsDeprecated ? "[DEPRECATED] " : string.Empty)}{settings.Description}",
                Contact = new() {
                    Name = settings.Contact?.Name,
                    Email = settings.Contact?.Email,
                    Url = settings.Contact?.Url != null ? new Uri(settings.Contact.Url) : null
                },
                License = new() {
                    Name = settings.License?.Name,
                    Url = settings.License?.Url != null ? new Uri(settings.License.Url) : null
                }
            };
        }

        #endregion

        #region IConfigureOptions<SwaggerGenOptions>

        /// <inheritdoc />
        public void Configure(SwaggerGenOptions options) {
            // add a swagger document for each discovered API version
            // note: you might choose to skip or document deprecated API versions differently
            foreach (var description in _provider.ApiVersionDescriptions) {
                options.SwaggerDoc(
                    name: description.GroupName,
                    info: CreateInfoForApiVersion(
                        description: description,
                        hostEnvironment: _hostEnvironment,
                        settings: _settings
                    )
                );
            }
        }

        #endregion
    }
}
