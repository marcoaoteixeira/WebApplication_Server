namespace Nameless.WebApplication.Logging.log4net {

    public sealed class NullScope : IDisposable {

        #region Private Static Read-Only Fields

        private static readonly NullScope _instance = new();

        #endregion

        #region Public Static Properties

        /// <summary>
        /// Gets the unique instance of <see cref="NullScope" />.
        /// </summary>
        public static NullScope Instance => _instance;

        #endregion

        #region Static Constructors

        // Explicit static constructor to tell the C# compiler
        // not to mark type as beforefieldinit
        static NullScope() { }

        #endregion

        #region Private Constructors

        private NullScope() { }

        #endregion

        #region IDisposable Members

        public void Dispose() { }

        #endregion
    }
}