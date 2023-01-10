using AutoMapper;
using Nameless.WebApplication.Api.Users.v1.Models.Output;
using Nameless.WebApplication.Entities;

namespace Nameless.WebApplication.Api.Users.v1.Mappings {

    public sealed class User_CreateUserOutput : Profile {

        #region Public Constructors

        public User_CreateUserOutput() {
            CreateMap<User, CreateUserOutput>();
        }

        #endregion
    }
}
