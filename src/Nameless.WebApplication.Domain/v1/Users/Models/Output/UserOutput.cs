namespace Nameless.WebApplication.Domain.v1.Users.Models.Output {

    public sealed class UserOutput {

        #region Public Properties

        public Guid Id { get; set; }
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;
        public bool Locked { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }

        #endregion
    }
}
