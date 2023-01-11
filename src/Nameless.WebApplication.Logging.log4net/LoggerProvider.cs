using System.Collections.Concurrent;
using System.Reflection;
using log4net;
using log4net.Config;
using log4net.Core;
using log4net.Repository;
using log4net.Repository.Hierarchy;
using Microsoft.Extensions.Options;
using MS_IExternalScopeProvider = Microsoft.Extensions.Logging.IExternalScopeProvider;
using MS_ILogger = Microsoft.Extensions.Logging.ILogger;
using MS_ILoggerProvider = Microsoft.Extensions.Logging.ILoggerProvider;

namespace Nameless.WebApplication.Logging.log4net {

    public sealed class LoggerProvider : MS_ILoggerProvider {

        #region Private Read-Only Static Fields

        private readonly static ConcurrentDictionary<string, ILogger> _cache = new();

        #endregion

        #region Private Read-Only Fields

        private readonly ILoggerEventFactory _loggerEventFactory;
        private readonly MS_IExternalScopeProvider _externalScopeProvider;
        private readonly Log4netSettings _settings;

        #endregion

        #region Private Fields

        private ILoggerRepository? _repository;
        private bool _disposed;

        #endregion

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="LoggerProvider"/>
        /// </summary>
        /// <param name="settings">The logger settings.</param>
        public LoggerProvider(ILoggerEventFactory loggerEventFactory, MS_IExternalScopeProvider externalScopeProvider, IOptions<Log4netSettings> settings) {
            Prevent.Null(loggerEventFactory, nameof(loggerEventFactory));
            Prevent.Null(externalScopeProvider, nameof(externalScopeProvider));

            _loggerEventFactory = loggerEventFactory;
            _externalScopeProvider = externalScopeProvider;
            _settings = settings?.Value ?? Log4netSettings.Default;

            Initialize();
        }

        #endregion

        #region Destructor

        ~LoggerProvider() {
            Dispose(disposing: false);
        }

        #endregion

        #region Private Static Methods

        private static FileInfo GetConfigurationFilePath(string configurationFileName) {
            return Path.IsPathRooted(configurationFileName)
                ? new FileInfo(configurationFileName)
                : new FileInfo(Path.Combine(typeof(LoggerProvider).Assembly.GetDirectoryPath(), configurationFileName));
        }

        #endregion

        #region Private Methods

        private void Initialize() {
            // Create logger repository
            var repositoryType = typeof(Hierarchy);
            if (!string.IsNullOrWhiteSpace(_settings.RepositoryName)) {
                try {
                    _repository = LogManager.CreateRepository(_settings.RepositoryName, repositoryType);
                } catch (LogException) {
                    _repository = null;
                }
            }
            _repository ??= LogManager.CreateRepository(Assembly.GetExecutingAssembly(), repositoryType);

            // Configure logger
            var configurationFilePath = GetConfigurationFilePath(_settings.ConfigurationFileName);
            if (_settings.ReloadOnChange) {
                XmlConfigurator.ConfigureAndWatch(_repository, configurationFilePath);
            } else {
                XmlConfigurator.Configure(_repository, configurationFilePath);
            }
        }

        private void Dispose(bool disposing) {
            if (_disposed) { return; }
            if (disposing) {
                if (_repository != null) {
                    _repository.Shutdown();
                    _cache.Clear();
                }
            }

            _repository = null;
            _disposed = true;
        }

        private void BlockAccessAfterDispose() {
            if (_disposed) {
                throw new ObjectDisposedException(nameof(LoggerProvider));
            }
        }

        #endregion

        #region MS_ILoggerProvider Members

        public MS_ILogger CreateLogger(string categoryName) {
            BlockAccessAfterDispose();

            return new Logger(
                logger: LogManager.GetLogger(_repository!.Name, categoryName).Logger,
                loggerEventFactory: _loggerEventFactory,
                externalScopeProvider: _externalScopeProvider,
                settings: _settings
            );
        }

        public void Dispose() {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
