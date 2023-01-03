namespace Nameless.WebApplication.Entities {

    public sealed class Claim : EntityBase {

        #region Public Properties

        public string Name { get; set; } = null!;
        public string Value { get; set; } = null!;
        public User Owner { get; set; } = null!;

        #endregion
    }
}
