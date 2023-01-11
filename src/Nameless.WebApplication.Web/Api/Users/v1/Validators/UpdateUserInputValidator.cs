using FluentValidation;
using Nameless.WebApplication.Api.Users.v1.Models.Input;

namespace Nameless.WebApplication.Api.Users.v1.Validators {

    public sealed class UpdateUserInputValidator : AbstractValidator<UpdateUserInput> {

        #region Public Constructors

        public UpdateUserInputValidator() {
            RuleFor(_ => _.Username)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(256);

            RuleFor(_ => _.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(256);

            RuleFor(_ => _.Role)
                .NotEmpty();
        }

        #endregion
    }
}
