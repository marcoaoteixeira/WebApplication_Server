using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Nameless.WebApplication.Settings;

namespace Nameless.WebApplication {

    public partial class StartUp {

        #region Private Static Methods

        private static void ConfigureAuth(IServiceCollection services, IHostEnvironment hostEnvironment, IConfiguration configuration) {
            var jwtSettings = configuration
                .GetSection(GetSectionKey<JsonWebTokenSettings>())
                .Get<JsonWebTokenSettings>() ?? new();

            services
                .AddAuthentication(opts => {
                    opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(opts => {
                    var secret = Encoding.UTF8.GetBytes(jwtSettings.Secret ?? JsonWebTokenSettings.DEFAULT_SECRET);

                    opts.RequireHttpsMetadata = !hostEnvironment.IsDevelopment();
                    opts.SaveToken = true;
                    opts.TokenValidationParameters = new() {
                        ValidateIssuer = !hostEnvironment.IsDevelopment(),
                        ValidateAudience = !hostEnvironment.IsDevelopment(),
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(secret),
                        ClockSkew = TimeSpan.Zero
                    };
                    opts.Events = new JwtBearerEvents {
                        OnAuthenticationFailed = ctx => {
                            if (ctx.Exception is SecurityTokenExpiredException) {
                                ctx.Response.Headers.Add("IS-TOKEN-EXPIRED", "true");
                            }
                            return Task.CompletedTask;
                        }
                    };
                });
        }

        private static void UseAuth(IApplicationBuilder applicationBuilder) {
            applicationBuilder.UseAuthentication();
            applicationBuilder.UseAuthorization();
        }

        #endregion
    }
}
