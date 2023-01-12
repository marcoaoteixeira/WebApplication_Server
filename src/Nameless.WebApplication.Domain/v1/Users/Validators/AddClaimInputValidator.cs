using FluentValidation;
using Nameless.WebApplication.Domain.v1.Users.Models.Input;

namespace Nameless.WebApplication.Domain.v1.Users.Validators {

    public sealed class AddClaimInputValidator : AbstractValidator<AddClaimInput> {

        #region Public Constructors

        public AddClaimInputValidator() {
            RuleFor(_ => _.Type)
                .NotEmpty();

            RuleFor(_ => _.Value)
                .NotEmpty()
                .MaximumLength(2048);
        }

        #endregion
    }
}
