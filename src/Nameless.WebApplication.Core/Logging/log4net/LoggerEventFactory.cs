using System.Collections;
using System.Globalization;
using log4net.Core;
using MS_IExternalScopeProvider = Microsoft.Extensions.Logging.IExternalScopeProvider;
using MS_LoggerExtensions = Microsoft.Extensions.Logging.LoggerExtensions;

namespace Nameless.WebApplication.Logging.log4net {

    public sealed class LoggerEventFactory : ILoggerEventFactory {

        #region Private Constants

        private const string DEFAULT_SCOPE_PROPERTY = "scope";

        #endregion

        #region Private Static Methods

        private static void Enrich(LoggingEvent loggingEvent, MS_IExternalScopeProvider externalScopeProvider) {
            static string? join(string? previous, string? actual) {
                return string.IsNullOrEmpty(previous)
                    ? actual
                    : string.Concat(previous, " ", actual);
            }

            externalScopeProvider.ForEachScope((scope, @event) => {
                // This function will add the scopes in the legacy way they were added before the IExternalScopeProvider was introduced,
                // to maintain backwards compatibility.
                // This pretty much means that we are emulating a LogicalThreadContextStack, which is a stack, that allows pushing
                // strings on to it, which will be concatenated with space as a separator.
                // See: https://github.com/apache/logging-log4net/blob/47aaf46d5f031ea29d781bac4617bd1bb9446215/src/log4net/Util/LogicalThreadContextStack.cs#L343

                // Because string implements IEnumerable we first need to check for string.
                if (scope is string) {
                    var previousValue = @event.Properties[DEFAULT_SCOPE_PROPERTY] as string;
                    @event.Properties[DEFAULT_SCOPE_PROPERTY] = join(previousValue, scope as string);
                    return;
                }

                if (scope is IEnumerable collection) {
                    foreach (var item in collection) {
                        switch (item) {
                            case KeyValuePair<string, string>: {
                                    var keyValuePair = (KeyValuePair<string, string>)item;
                                    var previousValue = @event.Properties[keyValuePair.Key] as string;
                                    @event.Properties[keyValuePair.Key] = join(previousValue, keyValuePair.Value);
                                    continue;
                                }

                            case KeyValuePair<string, object>: {
                                    var keyValuePair = (KeyValuePair<string, object>)item;
                                    var previousValue = @event.Properties[keyValuePair.Key] as string;

                                    // The current culture should not influence how integers/floats/... are displayed in logging,
                                    // so we are using Convert.ToString which will convert IConvertible and IFormattable with
                                    // the specified IFormatProvider.
                                    var additionalValue = Convert.ToString(keyValuePair.Value, CultureInfo.InvariantCulture);
                                    @event.Properties[keyValuePair.Key] = join(previousValue, additionalValue);
                                    continue;
                                }
                        }
                    }
                    return;
                }

                if (scope is not null) {
                    var previousValue = @event.Properties[DEFAULT_SCOPE_PROPERTY] as string;
                    var additionalValue = Convert.ToString(scope, CultureInfo.InvariantCulture);
                    @event.Properties[DEFAULT_SCOPE_PROPERTY] = join(previousValue, additionalValue);
                    return;
                }

            }, loggingEvent);
        }

        #endregion

        #region ILoggerEventFactory Members

        public LoggingEvent? CreateLoggingEvent<TState>(in LogMessage<TState> message, ILogger logger, MS_IExternalScopeProvider externalScopeProvider, Log4netSettings settings) {
            Prevent.Null(message, nameof(message));
            Prevent.Null(logger, nameof(logger));

            var level = LogLevelTranslator.Translate(message.LogLevel, settings.OverrideCriticalLevel);
            if (level == Level.Off) {
                return null;
            }

            var outputMessage = message.Formatter(message.State, message.Exception);
            if (string.IsNullOrWhiteSpace(outputMessage) && message.Exception == null) {
                return null;
            }

            var result = new LoggingEvent(
                callerStackBoundaryDeclaringType: typeof(MS_LoggerExtensions),
                repository: logger.Repository,
                loggerName: logger.Name,
                level: level,
                message: outputMessage,
                exception: message.Exception
            );

            Enrich(result, externalScopeProvider);

            return result;
        }

        #endregion
    }
}
