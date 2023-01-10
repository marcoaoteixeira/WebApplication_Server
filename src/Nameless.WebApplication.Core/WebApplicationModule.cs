using Autofac;
using Nameless.WebApplication.Services;
using Nameless.WebApplication.Services.Impl;

namespace Nameless.WebApplication {
    public sealed class WebApplicationModule : Module {

        #region Protected Override Methods

        protected override void Load(ContainerBuilder builder) {
            builder
                .RegisterInstance(SystemClock.Instance)
                .As<IClock>()
                .SingleInstance();

            builder
                .RegisterType<JsonWebTokenService>()
                .As<IJsonWebTokenService>()
                .InstancePerDependency();

            builder
                .RegisterType<AuthenticationService>()
                .As<IAuthenticationService>()
                .InstancePerDependency();

            builder
                .RegisterType<UserManager>()
                .As<IUserManager>()
                .InstancePerDependency();

            base.Load(builder);
        }

        #endregion
    }
}
