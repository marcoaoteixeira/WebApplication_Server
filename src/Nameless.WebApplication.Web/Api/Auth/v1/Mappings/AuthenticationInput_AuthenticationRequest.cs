using AutoMapper;
using Nameless.WebApplication.Api.Auth.v1.Models.Input;
using Nameless.WebApplication.Domain;

namespace Nameless.WebApplication.Api.Auth.v1.Mappings {

    public sealed class AuthenticationInput_AuthenticationRequest : Profile {

        #region Public Constructors

        public AuthenticationInput_AuthenticationRequest() {
            CreateMap<AuthenticationInput, AuthenticationRequest>();
        }

        #endregion
    }
}
