using AutoMapper;
using Nameless.WebApplication.Domain.Dtos.Common;
using Nameless.WebApplication.Domain.Dtos.v1.Auth;

namespace Nameless.WebApplication.Domain.Mappings.v1.Auth {

    public sealed class AuthenticationInput_AuthenticationRequest : Profile {

        #region Public Constructors

        public AuthenticationInput_AuthenticationRequest() {
            CreateMap<AuthenticationInput, AuthenticationRequest>();
        }

        #endregion
    }
}
