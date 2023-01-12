namespace Nameless.WebApplication.Domain.v1.Users.Models.Output {

    public sealed class AddClaimOutput {

        #region Public Properties

        public string Type { get; set; } = default!;
        public string Value { get; set; } = default!;

        #endregion
    }
}
