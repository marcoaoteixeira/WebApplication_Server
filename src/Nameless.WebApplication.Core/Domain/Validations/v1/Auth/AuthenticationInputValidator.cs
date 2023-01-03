using FluentValidation;
using Nameless.WebApplication.Domain.Dtos.v1.Auth;

namespace Nameless.WebApplication.Domain.Validations.v1.Auth {

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