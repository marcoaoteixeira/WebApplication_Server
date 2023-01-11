using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Nameless.WebApplication.Api.Users.v1.Models.Input;
using Nameless.WebApplication.Entities;

namespace Nameless.WebApplication.Api.Users.v1.Validators {

    public sealed class CreateUserInputValidator : AbstractValidator<CreateUserInput> {

        #region Public Constructors

        public CreateUserInputValidator(ApplicationDbContext dbContext) {
            Prevent.Null(dbContext, nameof(dbContext));

            RuleFor(_ => _.Username)
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
