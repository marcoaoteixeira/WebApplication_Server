using AutoMapper;
using FluentValidation.Results;

namespace Nameless.WebApplication.Commands.Mappings
{
    public sealed class ValidationResult_Response : Profile
    {

        #region Public Constructors

        public ValidationResult_Response()
        {
            CreateMap<ValidationFailure, Error>()
                .IgnoreAllPropertiesWithAnInaccessibleSetter()
                .ConstructUsing(failure => new Error(failure.PropertyName, failure.ErrorMessage));

            CreateMap<ValidationResult, Response>()
                .IgnoreAllPropertiesWithAnInaccessibleSetter()
                .ConstructUsing((result, ctx) => new Response(errors: ctx.Mapper.Map<Error[]>(result.Errors), message: "Validation error"));
        }

        #endregion
    }
}
