namespace Nameless.WebApplication.Domain.Dtos.v1.Users {

    public sealed class CreateUserOutput {

        #region Public Properties

        public Guid ID { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool Locked { get; set; }
        public DateTime CreationDate { get; set; }

        #endregion
    }
}
