namespace Nameless.WebApplication.Settings {

    public sealed class RefreshTokenSettings {

        #region Public Constants

        public const int DEFAULT_TTL = 15;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the refresh token time-to-live
        /// in days.
        /// </summary>
        public int Ttl { get; set; } = DEFAULT_TTL;

        #endregion
    }
}
