namespace Nameless.WebApplication.Domain.v1.Users.Models.Input {

    public sealed class CreateUserInput {

        #region Public Properties

        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string ConfirmPassword { get; set; } = null!;
        public bool Locked { get; set; }
        public string? Role { get; set; }

        #endregion
    }
}
