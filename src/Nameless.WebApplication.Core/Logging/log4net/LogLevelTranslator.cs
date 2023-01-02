using log4net.Core;
using MS_LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Nameless.WebApplication.Logging.log4net {

    internal static class LogLevelTranslator {

        #region Internal Static Methods

        internal static Level Translate(MS_LogLevel level, bool overrideCriticalLevel = false) {
            return level switch {
                MS_LogLevel.Debug => Level.Debug,
                MS_LogLevel.Information => Level.Info,
                MS_LogLevel.Warning => Level.Warn,
                MS_LogLevel.Error => Level.Error,
                MS_LogLevel.Critical => overrideCriticalLevel ? Level.Fatal : Level.Critical,
                _ => Level.Off
            };
        }

        #endregion
    }
}
