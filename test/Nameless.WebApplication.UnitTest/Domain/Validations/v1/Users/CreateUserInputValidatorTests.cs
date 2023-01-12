﻿using FluentValidation.TestHelper;
using Nameless.WebApplication.Domain.v1.Users.Models.Input;
using Nameless.WebApplication.Domain.v1.Users.Validators;
using Nameless.WebApplication.Entities;

namespace Nameless.WebApplication.UnitTest.Domain.Validations.v1.Users {

    public sealed class CreateUserInputValidatorTests {

        [Test]
        public async Task ValidateAsync_Must_Return_Success_With_Valid_Data() {
            // arrange
            var dbContext = DbContextFactory.CreateInMemory();
            var validator = new CreateUserInputValidator(dbContext);
            var createUserInput = new CreateUserInput {
                UserName = "username",
                Email = "email@email.com",
                Password = "123456abc@#",
                ConfirmPassword = "123456abc@#"
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
                Id = Guid.NewGuid(),
                UserName = "username",
                Email = "email@email.com",
                PasswordHash = "123456abc@#",
            });
            await dbContext.SaveChangesAsync();

            var validator = new CreateUserInputValidator(dbContext);
            var createUserInput = new CreateUserInput {
                UserName = "username",
                Email = "email@email.com",
                Password = "123456abc@#",
                ConfirmPassword = "123456abc@#"
            };

            // act
            var result = await validator.TestValidateAsync(createUserInput);

            // assert
            result.ShouldHaveValidationErrorFor(_ => _.Email);
        }
    }
}
