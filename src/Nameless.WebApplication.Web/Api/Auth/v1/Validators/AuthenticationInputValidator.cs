using FluentValidation;
using Nameless.WebApplication.Api.Auth.v1.Models.Input;

namespace Nameless.WebApplication.Api.Auth.v1.Validators {

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