namespace Nameless.WebApplication.Api.Users.v1.Models.Input {

    public sealed class UpdateUserInput {

        #region Public Properties

        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;
        public bool Locked { get; set; }


        #endregion
    }
}
