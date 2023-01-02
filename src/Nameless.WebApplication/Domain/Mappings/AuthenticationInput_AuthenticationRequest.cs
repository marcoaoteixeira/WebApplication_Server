using AutoMapper;
using Nameless.WebApplication.Api.v1.Models;
using Nameless.WebApplication.Services;

namespace Nameless.WebApplication.Domain.Mappings {

    public sealed class AuthenticationInput_AuthenticationRequest : Profile {

        #region Public Constructors

        public AuthenticationInput_AuthenticationRequest() {
            CreateMap<AuthenticationInput, AuthenticationRequest>();
        }

        #endregion
    }
}
