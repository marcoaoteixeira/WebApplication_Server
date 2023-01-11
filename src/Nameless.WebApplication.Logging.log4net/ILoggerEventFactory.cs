using log4net.Core;
using MS_IExternalScopeProvider = Microsoft.Extensions.Logging.IExternalScopeProvider;

namespace Nameless.WebApplication.Logging.log4net {

    public interface ILoggerEventFactory {

        #region Methods

        LoggingEvent? CreateLoggingEvent<TState>(in LogMessage<TState> message, ILogger logger, MS_IExternalScopeProvider externalScopeProvider, Log4netSettings settings);

        #endregion
    }
}
