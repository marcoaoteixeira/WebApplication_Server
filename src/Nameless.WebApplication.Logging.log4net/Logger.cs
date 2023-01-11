using log4net.Core;
using MS_EventId = Microsoft.Extensions.Logging.EventId;
using MS_ILogger = Microsoft.Extensions.Logging.ILogger;
using MS_LogLevel = Microsoft.Extensions.Logging.LogLevel;
using MS_IExternalScopeProvider = Microsoft.Extensions.Logging.IExternalScopeProvider;

namespace Nameless.WebApplication.Logging.log4net {

    public sealed class Logger : MS_ILogger {

        #region Private Read-Only Fields

        private readonly ILogger _logger;
        private readonly ILoggerEventFactory _loggerEventFactory;
        private readonly MS_IExternalScopeProvider _externalScopeProvider;
        private readonly Log4netSettings _settings;

        #endregion

        #region Public Constructors

        public Logger(ILogger logger, ILoggerEventFactory loggerEventFactory, MS_IExternalScopeProvider externalScopeProvider, Log4netSettings settings) {
            Prevent.Null(logger, nameof(logger));
            Prevent.Null(loggerEventFactory, nameof(loggerEventFactory));
            Prevent.Null(externalScopeProvider, nameof(externalScopeProvider));
            Prevent.Null(settings, nameof(settings));

            _logger = logger;
            _loggerEventFactory = loggerEventFactory;
            _externalScopeProvider = externalScopeProvider;
            _settings = settings;
        }

        #endregion

        #region MS_ILogger Members

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => _externalScopeProvider.Push(state);

        public bool IsEnabled(MS_LogLevel logLevel) {
            var level = LogLevelTranslator.Translate(logLevel, _settings.OverrideCriticalLevel);

            return level != Level.Off && _logger.IsEnabledFor(level);
        }

        public void Log<TState>(MS_LogLevel logLevel, MS_EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) {
            Prevent.Null(formatter, nameof(formatter));

            if (!IsEnabled(logLevel)) { return; }
            
            var message = new LogMessage<TState>(logLevel, eventId, state, exception, formatter);
            var loggingEvent = _loggerEventFactory.CreateLoggingEvent(
                in message,
                _logger,
                _externalScopeProvider,
                _settings
            );
            
            if (loggingEvent is null) { return; }

            _logger.Log(loggingEvent);
        }

        #endregion
    }
}
