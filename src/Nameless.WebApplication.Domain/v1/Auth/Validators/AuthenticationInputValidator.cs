using FluentValidation;
using Nameless.WebApplication.Domain.v1.Auth.Models.Input;

namespace Nameless.WebApplication.Domain.v1.Auth.Validators {

    public sealed class AuthenticationInputValidator : AbstractValidator<AuthenticationInput> {

        #region Public Constructors

        public AuthenticationInputValidator() {
            RuleFor(_ => _.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(_ => _.Password)
                .NotEmpty();
        }

        #endregion
    }
}