using MS_EventId = Microsoft.Extensions.Logging.EventId;
using MS_LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Nameless.WebApplication.Logging.log4net {

    public readonly record struct LogMessage<TState>(MS_LogLevel LogLevel, MS_EventId EventId, TState State, Exception? Exception, Func<TState, Exception?, string> Formatter);
}
