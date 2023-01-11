using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using Nameless.WebApplication.Autofac;
using MS_IExternalScopeProvider = Microsoft.Extensions.Logging.IExternalScopeProvider;
using MS_ILogger = Microsoft.Extensions.Logging.ILogger;
using MS_ILoggerProvider = Microsoft.Extensions.Logging.ILoggerProvider;
using MS_NullLogger = Microsoft.Extensions.Logging.Abstractions.NullLogger;

namespace Nameless.WebApplication.Logging.log4net {

    public sealed class LoggingModule : Module {

        #region Public Properties

        public Type? ExternalScopeProviderImplementation { get; set; } = typeof(NullExternalScopeProvider);

        #endregion

        #region Protected Override Methods

        /// <inheritdoc/>
        protected override void Load(ContainerBuilder builder) {
            var externalScopeProvider = ExternalScopeProviderImplementation ?? typeof(NullExternalScopeProvider);
            if (externalScopeProvider.IsSingleton()) {
                builder.RegisterInstance(externalScopeProvider.GetSingletonInstance() ?? NullExternalScopeProvider.Instance);
            } else {
                builder.RegisterType(externalScopeProvider).As<MS_IExternalScopeProvider>().SingleInstance();
            }

            builder
                .RegisterType<LoggerEventFactory>()
                .As<ILoggerEventFactory>()
                .SingleInstance();

            builder
                .RegisterType<LoggerProvider>()
                .As<MS_ILoggerProvider>()
                .SingleInstance();
        }

        /// <inheritdoc/>
        protected override void AttachToComponentRegistration(IComponentRegistryBuilder componentRegistry, IComponentRegistration registration) {
            registration.PipelineBuilding += (sender, pipeline) => {
                pipeline.Use(new FactoryResolveMiddleware(
                    injectType: typeof(MS_ILogger),
                    factory: (member, ctx) => {
                        return member.DeclaringType != null
                            ? ctx.Resolve<MS_ILoggerProvider>().CreateLogger(member.DeclaringType.Name)
                            : MS_NullLogger.Instance;
                    }
                ));
            };

            base.AttachToComponentRegistration(componentRegistry, registration);
        }

        #endregion
    }
}
