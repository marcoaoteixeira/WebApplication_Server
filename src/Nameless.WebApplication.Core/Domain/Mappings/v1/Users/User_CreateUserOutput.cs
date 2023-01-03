using AutoMapper;
using Nameless.WebApplication.Domain.Dtos.v1.Users;
using Nameless.WebApplication.Entities;

namespace Nameless.WebApplication.Domain.Mappings.v1.Users {

    public sealed class User_CreateUserOutput : Profile {

        #region Public Constructors

        public User_CreateUserOutput() {
            CreateMap<User, CreateUserOutput>();
        }

        #endregion
    }
}
