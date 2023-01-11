using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Nameless.WebApplication.Domain.v1.Users.Models.Input;
using Nameless.WebApplication.Entities;

namespace Nameless.WebApplication.Domain.v1.Users.Validators {

    public sealed class CreateUserInputValidator : AbstractValidator<CreateUserInput> {

        #region Public Constructors

        public CreateUserInputValidator(ApplicationDbContext dbContext) {
            Prevent.Null(dbContext, nameof(dbContext));

            RuleFor(_ => _.UserName)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(256);

            RuleFor(_ => _.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(256)
                .MustAsync(async (email, cancellationToken) => {
                    var result = await dbContext.Users.AnyAsync(_ => _.Email == email, cancellationToken);
                    return !result;
                })
                .WithMessage("User already exists.");

            RuleFor(_ => _.Password)
                .NotEmpty()
                .Matches(Constants.PASSWORD_REGEX_PATTERN);

            RuleFor(_ => _.ConfirmPassword)
                .Equal(input => input.Password);
        }

        #endregion
    }
}
