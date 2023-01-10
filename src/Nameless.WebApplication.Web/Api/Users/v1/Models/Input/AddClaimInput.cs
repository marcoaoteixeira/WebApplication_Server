namespace Nameless.WebApplication.Api.Users.v1.Models.Input {

    public sealed class AddClaimInput {

        #region Public Properties

        public string Name { get; set; } = null!;
        public string Value { get; set; } = null!;

        #endregion
    }
}
