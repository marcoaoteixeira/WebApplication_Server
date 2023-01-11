using AutoMapper;
using Nameless.WebApplication.Domain.v1.Users.Models.Input;
using Nameless.WebApplication.Entities;

namespace Nameless.WebApplication.Domain.v1.Users.Mappings {

    public sealed class AddClaimInput_Claim : Profile {

        #region Public Constructors

        public AddClaimInput_Claim() {
            CreateMap<AddClaimInput, UserClaim>()
                .ForMember(dest => dest.ClaimType, opts => opts.MapFrom(src => src.Name))
                .ForMember(dest => dest.ClaimValue, opts => opts.MapFrom(src => src.Value));
        }

        #endregion
    }
}
