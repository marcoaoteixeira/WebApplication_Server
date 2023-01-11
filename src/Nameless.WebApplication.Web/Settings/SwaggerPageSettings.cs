namespace Nameless.WebApplication.Settings {

    public sealed class SwaggerPageSettings {

        #region Public Properties

        public string? Description { get; set; }
        public Contact? Contact { get; set; }
        public License? License { get; set; }

        #endregion
    }

    public sealed class Contact {

        #region Public Properties

        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Url { get; set; }

        #endregion
    }

    public sealed class License {

        #region Public Properties

        public string? Name { get; set; }
        public string? Url { get; set; }

        #endregion
    }
}
