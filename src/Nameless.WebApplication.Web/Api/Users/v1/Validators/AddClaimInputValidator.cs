using FluentValidation;
using Nameless.WebApplication.Api.Users.v1.Models.Input;

namespace Nameless.WebApplication.Api.Users.v1.Validators {

    public sealed class AddClaimInputValidator : AbstractValidator<AddClaimInput> {

        #region Public Constructors

        public AddClaimInputValidator() {
            RuleFor(_ => _.Name).NotEmpty();

            RuleFor(_ => _.Value)
                .NotEmpty()
                .MaximumLength(2048);
        }

        #endregion
    }
}
