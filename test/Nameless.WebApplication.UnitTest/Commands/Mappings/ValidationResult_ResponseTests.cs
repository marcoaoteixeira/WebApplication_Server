using FluentAssertions;
using FluentValidation.Results;
using Nameless.WebApplication.Commands;
using Nameless.WebApplication.Commands.Mappings;

namespace Nameless.WebApplication.UnitTest.Commands.Mappings {

    public class ValidationResult_ResponseTests : TestCaseBase {

        [OneTimeSetUp] public void OneTimeSetUp() {
            ConfigureMapper(new[] { typeof(ValidationResult_Response) });
        }

        [Test]
        public void Map_ValidationResult_To_Response_Object() {
            // arrange
            var validationFailures = new[] {
                new ValidationFailure("Name", "Empty name")
            };
            var validationResult = new ValidationResult(validationFailures);
            
            // act
            var response = Mapper.Map<Response>(validationResult);

            // assert
            response.Should().NotBeNull();
            response.Errors.Should().NotBeEmpty();
            response.Success.Should().BeFalse();
            response.Errors.First().Code.Should().Be("Name");
            response.Errors.First().Message.Should().Be("Empty name");
        }
    }
}
