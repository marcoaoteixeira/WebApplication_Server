using FluentValidation;
using Nameless.WebApplication.Domain.v1.Users.Models.Input;

namespace Nameless.WebApplication.Domain.v1.Users.Validators {

    public sealed class RemoveClaimInputValidator : AbstractValidator<RemoveClaimInput> {

        #region Public Constructors

        public RemoveClaimInputValidator() {
            RuleFor(_ => _.Type).NotEmpty();
        }

        #endregion
    }
}
