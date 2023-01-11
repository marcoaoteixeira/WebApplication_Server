using AutoMapper;
using Nameless.WebApplication.Domain.v1.Users.Models.Input;
using Nameless.WebApplication.Entities;

namespace Nameless.WebApplication.Domain.v1.Users.Mappings {

    public sealed class UpdateUserInput_User : Profile {

        #region Public Constructors

        public UpdateUserInput_User() {
            CreateMap<UpdateUserInput, User>()
                .IgnoreAll()!
                .ForMember(dest => dest.UserName, opts => opts.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opts => opts.MapFrom(src => src.Email));
        }

        #endregion
    }
}
