using AutoMapper;
using Nameless.WebApplication.Domain.Dtos.v1.Users;
using Nameless.WebApplication.Domain.Entities;

namespace Nameless.WebApplication.Domain.Mappings.v1.Users {

    public sealed class User_UserOutput : Profile {

        #region Public Constructors

        public User_UserOutput() {
            CreateMap<User, UserOutput>();
        }

        #endregion
    }
}
