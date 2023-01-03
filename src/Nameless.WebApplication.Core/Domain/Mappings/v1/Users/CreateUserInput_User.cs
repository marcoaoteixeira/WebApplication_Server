using AutoMapper;
using Nameless.WebApplication.Domain.Dtos.v1.Users;
using Nameless.WebApplication.Domain.Entities;

namespace Nameless.WebApplication.Domain.Mappings.v1.Users {

    public sealed class CreateUserInput_User : Profile {

        #region Public Constructors

        public CreateUserInput_User() {
            CreateMap<CreateUserInput, User>();
        }

        #endregion
    }
}
