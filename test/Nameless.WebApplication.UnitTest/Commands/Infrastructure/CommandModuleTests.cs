using Autofac;
using FluentAssertions;
using FluentValidation;
using Nameless.WebApplication.Commands;
using Nameless.WebApplication.Commands.Infrastructure;
using Nameless.WebApplication.Commands.Mappings;
using Nameless.WebApplication.UnitTest.Commands.Fixtures;

namespace Nameless.WebApplication.UnitTest.Commands.Infrastructure {

    public class CommandModuleTests : TestCaseBase {

        [OneTimeSetUp]
        public void OneTimeSetUp() {
            ConfigureMapper(new[] {
                typeof(ValidationResult_Response),
                typeof(AnimalCommand_Animal)
            });
        }

        [Test]
        public async Task CommandModule_Resolve_Dependencies() {

            // arrange
            var builder = new ContainerBuilder();
            builder.RegisterModule(new CommandModule {
                CommandHandlerImplementations = new[] {
                    typeof(SumCommandHandler)
                }
            });

            var container = builder.Build();

            // act
            var dispatcher = container.Resolve<ICommandDispatcher>();
            var response = await dispatcher.DispatchAsync(new SumCommand { X = 1, Y = 2 });

            // assert
            response.Should().NotBeNull();
            response.State.Should().Be(3);
        }

        [Test]
        public async Task CommandModule_Resolve_Commands_For_CommandHandlerBase_Without_Validator() {
            // arrange
            var builder = new ContainerBuilder();
            builder.RegisterInstance(DbContextFactory.CreateInMemory());
            builder.RegisterInstance(Mapper);
            builder.RegisterModule(new CommandModule {
                CommandHandlerImplementations = new[] {
                    typeof(AnimalCommandHandler)
                }
            });

            var container = builder.Build();

            // act
            var dispatcher = container.Resolve<ICommandDispatcher>();
            var response = await dispatcher.DispatchAsync(new AnimalCommand { Name = "Cat" });

            // assert
            response.Should().NotBeNull();
            response.State.Should().Be("Cat");
        }

        [Test]
        public async Task CommandModule_Resolve_Commands_For_CommandHandlerBase_With_Validator_Returns_Validation_Error() {
            // arrange
            var builder = new ContainerBuilder();
            builder.RegisterInstance(DbContextFactory.CreateInMemory());
            builder.RegisterInstance(Mapper);
            builder.RegisterInstance(new AnimalCommandValidator())
                .As<IValidator<AnimalCommand>>()
                .SingleInstance();
            builder.RegisterModule(new CommandModule {
                CommandHandlerImplementations = new[] {
                    typeof(AnimalCommandHandler)
                }
            });

            var container = builder.Build();

            // act
            var dispatcher = container.Resolve<ICommandDispatcher>();
            var response = await dispatcher.DispatchAsync(new AnimalCommand { Name = "Cat" });

            // assert
            response.Should().NotBeNull();
            response.Success.Should().BeFalse();
            response.Errors.First().Code.Should().Be("Name");
            response.Message.Should().Be("Validation error");
        }
    }
}
