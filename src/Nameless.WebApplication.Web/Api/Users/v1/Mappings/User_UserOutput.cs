using AutoMapper;
using Nameless.WebApplication.Api.Users.v1.Models.Output;
using Nameless.WebApplication.Entities;

namespace Nameless.WebApplication.Api.Users.v1.Mappings {

    public sealed class User_UserOutput : Profile {

        #region Public Constructors

        public User_UserOutput() {
            CreateMap<User, UserOutput>();
        }

        #endregion
    }
}
