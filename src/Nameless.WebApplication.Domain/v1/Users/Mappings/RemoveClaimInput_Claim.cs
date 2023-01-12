using System.Security.Claims;
using AutoMapper;
using Nameless.WebApplication.Domain.v1.Users.Models.Input;

namespace Nameless.WebApplication.Domain.v1.Users.Mappings {

    public sealed class RemoveClaimInput_Claim : Profile {

        #region Public Constructors

        public RemoveClaimInput_Claim() {
            CreateMap<RemoveClaimInput, Claim>();
        }

        #endregion
    }
}
