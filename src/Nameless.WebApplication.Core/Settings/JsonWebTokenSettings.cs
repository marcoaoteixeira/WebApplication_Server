namespace Nameless.WebApplication.Settings {

    public sealed class JsonWebTokenSettings {

        #region Public Constants

        public const string DEFAULT_SECRET = "fca5dc4e-e04f-4c48-90a8-12c27930fd2b";
        public const int DEFAULT_TTL = 60;
        public const int DEFAULT_REFRESH_TOKEN_TTL = 15;

        #endregion

        #region Public Properties

        public string Secret { get; set; } = DEFAULT_SECRET;
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public int TokenTtl { get; set; } = DEFAULT_TTL;
        public int RefreshTokenTtl { get; set; } = DEFAULT_REFRESH_TOKEN_TTL;

        #endregion
    }
}
