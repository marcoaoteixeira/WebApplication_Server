namespace Nameless.WebApplication.Domain.v1.Users.Models.Input {

    public sealed class UpdateUserInput {

        #region Public Properties

        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;
        public bool Locked { get; set; }


        #endregion
    }
}
