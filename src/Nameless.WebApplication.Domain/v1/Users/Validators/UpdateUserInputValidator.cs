using FluentValidation;
using Nameless.WebApplication.Domain.v1.Users.Models.Input;
using Nameless.WebApplication.Entities;

namespace Nameless.WebApplication.Domain.v1.Users.Validators {

    public sealed class UpdateUserInputValidator : AbstractValidator<UpdateUserInput> {

        #region Public Constructors

        public UpdateUserInputValidator() {
            RuleFor(_ => _.UserName)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(256);

            RuleFor(_ => _.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(256);

            RuleFor(_ => _.Role)
                .NotEmpty()
                .IsEnumName(typeof(Roles));
        }

        #endregion
    }
}
