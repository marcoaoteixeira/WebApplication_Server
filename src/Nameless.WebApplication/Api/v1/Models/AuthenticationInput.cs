using System.ComponentModel.DataAnnotations;

namespace Nameless.WebApplication.Api.v1.Models {

    public sealed class AuthenticationInput {

        #region Public Properties

        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        #endregion
    }
}
