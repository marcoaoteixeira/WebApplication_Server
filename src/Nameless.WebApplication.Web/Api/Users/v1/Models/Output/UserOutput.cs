namespace Nameless.WebApplication.Api.Users.v1.Models.Output {

    public sealed class UserOutput {

        #region Public Properties

        public Guid ID { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;
        public bool Locked { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }

        #endregion
    }
}
