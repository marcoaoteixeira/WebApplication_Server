using AutoMapper;
using Nameless.WebApplication.Api.Users.v1.Models.Input;
using Nameless.WebApplication.Entities;

namespace Nameless.WebApplication.Api.Users.v1.Mappings {

    public sealed class CreateUserInput_User : Profile {

        #region Public Constructors

        public CreateUserInput_User() {
            CreateMap<CreateUserInput, User>();
        }

        #endregion
    }
}
