using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Nameless.WebApplication.Commands;
using Nameless.WebApplication.Commands.Impl;
using Nameless.WebApplication.Entities;

namespace Nameless.WebApplication.UnitTest.Commands.Fixtures {
    
    public class Animal {
        public string? Name { get; set; }
    }

    public class AnimalCommand : Command {
        public string? Name { get; set; }
    }

    public class AnimalCommandHandler : CommandHandlerBase<AnimalCommand> {

        public AnimalCommandHandler(ApplicationDbContext dbContext, IMapper mapper, IValidator<AnimalCommand>? validator = default)
            : base (dbContext, mapper, validator) { }

        public override Task<Response> InnerHandleAsync(AnimalCommand command, CancellationToken cancellationToken = default) {
            var animal = Mapper.Map<Animal>(command);

            return Response.Successful(animal.Name).AsTask();
        }
    }

    public class AnimalCommand_Animal : Profile {
        public AnimalCommand_Animal() {
            CreateMap<AnimalCommand, Animal>();
        }
    }

    public class AnimalCommandValidator : AbstractValidator<AnimalCommand> {

        public AnimalCommandValidator() {
            RuleFor(_ => _.Name).Equal("Dog");
        }
    }
}
