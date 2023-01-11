using AutoMapper;
using Nameless.WebApplication.Api.Users.v1.Models.Input;
using Nameless.WebApplication.Entities;

namespace Nameless.WebApplication.Api.Users.v1.Mappings {

    public sealed class AddClaimInput_Claim : Profile {

        #region Public Constructors

        public AddClaimInput_Claim() {
            CreateMap<AddClaimInput, UserClaim>();
        }

        #endregion
    }
}
