using AutoMapper;
using Nameless.WebApplication.Api.Users.v1.Models.Input;
using Nameless.WebApplication.Entities;

namespace Nameless.WebApplication.Api.Users.v1.Mappings {

    public sealed class UpdateUserInput_User : Profile {

        #region Public Constructors

        public UpdateUserInput_User() {
            CreateMap<UpdateUserInput, User>();
        }

        #endregion
    }
}
