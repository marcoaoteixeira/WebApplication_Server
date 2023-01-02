using Microsoft.Extensions.DependencyInjection;
using MS_IExternalScopeProvider = Microsoft.Extensions.Logging.IExternalScopeProvider;
using MS_ILoggerProvider = Microsoft.Extensions.Logging.ILoggerProvider;
using MS_ILoggingBuilder = Microsoft.Extensions.Logging.ILoggingBuilder;

namespace Nameless.WebApplication.Logging.log4net {

    public static class LoggerExtension {

        #region Public Static Methods

        public static MS_ILoggingBuilder AddLog4net(this MS_ILoggingBuilder builder) {
            builder.Services.AddSingleton(NullExternalScopeProvider.Instance);
            builder.Services.AddSingleton<ILoggerEventFactory, LoggerEventFactory>();
            builder.Services.AddSingleton<MS_ILoggerProvider, LoggerProvider>();

            return builder;
        }

        public static MS_ILoggingBuilder AddLog4net<TExternalScopeProvider>(this MS_ILoggingBuilder builder)
            where TExternalScopeProvider : class, MS_IExternalScopeProvider {
            return AddLog4net<TExternalScopeProvider, LoggerEventFactory>(builder);
        }

        public static MS_ILoggingBuilder AddLog4net<TExternalScopeProvider, TLoggerEventFactory>(this MS_ILoggingBuilder builder)
            where TExternalScopeProvider : class, MS_IExternalScopeProvider
            where TLoggerEventFactory : class, ILoggerEventFactory {
            builder.Services.AddSingleton<MS_IExternalScopeProvider, TExternalScopeProvider>();
            builder.Services.AddSingleton<ILoggerEventFactory, TLoggerEventFactory>();
            builder.Services.AddSingleton<MS_ILoggerProvider, LoggerProvider>();

            return builder;
        }

        #endregion
    }
}
