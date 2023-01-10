using FluentValidation.TestHelper;
using Nameless.WebApplication.Api.Users.v1.Models.Input;
using Nameless.WebApplication.Api.Users.v1.Validators;
using Nameless.WebApplication.Entities;

namespace Nameless.WebApplication.UnitTest.Domain.Validations.v1.Users {

    public sealed class CreateUserInputValidatorTests {

        [Test]
        public async Task ValidateAsync_Must_Return_Success_With_Valid_Data() {
            // arrange
            var dbContext = DbContextFactory.CreateInMemory();
            var validator = new CreateUserInputValidator(dbContext);
            var createUserInput = new CreateUserInput {
                Username = "username",
                Email = "email@email.com",
                Password = "123456abc@#",
                ConfirmPassword = "123456abc@#",
                Locked = false
            };

            // act
            var result = await validator.TestValidateAsync(createUserInput);

            // assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public async Task ValidateAsync_Must_Return_Failure_If_User_Already_Exists() {
            // arrange
            var dbContext = DbContextFactory.CreateInMemory();
            dbContext.Users.Add(new User {
                ID = Guid.NewGuid(),
                Username = "username",
                Email = "email@email.com",
                Password = "123456abc@#",
            });
            await dbContext.SaveChangesAsync();

            var validator = new CreateUserInputValidator(dbContext);
            var createUserInput = new CreateUserInput {
                Username = "username",
                Email = "email@email.com",
                Password = "123456abc@#",
                ConfirmPassword = "123456abc@#",
                Locked = false
            };

            // act
            var result = await validator.TestValidateAsync(createUserInput);

            // assert
            result.ShouldHaveValidationErrorFor(_ => _.Email);
        }
    }
}
