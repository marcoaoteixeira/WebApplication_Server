using AutoMapper;
using Nameless.WebApplication.Domain.v1.Auth.Models.Input;

namespace Nameless.WebApplication.Domain.v1.Auth.Mappings {

    public sealed class AuthenticationInput_AuthenticationRequest : Profile {

        #region Public Constructors

        public AuthenticationInput_AuthenticationRequest() {
            CreateMap<AuthenticationInput, AuthenticationRequest>();
        }

        #endregion
    }
}
