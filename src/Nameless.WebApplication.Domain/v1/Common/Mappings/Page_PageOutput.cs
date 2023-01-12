using AutoMapper;
using Nameless.WebApplication.Collections.Generic;
using Nameless.WebApplication.Domain.v1.Common.Models.Output;

namespace Nameless.WebApplication.Domain.v1.Common.Mappings
{

    public sealed class Page_PageOutput : Profile
    {

        #region Public Constructors

        public Page_PageOutput()
        {
            CreateMap(typeof(Page<>), typeof(PageOutput<>))
                .IgnoreAll()!
                .ForMember(nameof(PageOutput<object>.Items), opts => opts.MapFrom(src => src))
                .ForMember(nameof(PageOutput<object>.Index), opts => opts.MapFrom(nameof(Page<object>.Index)))
                .ForMember(nameof(PageOutput<object>.Size), opts => opts.MapFrom(nameof(Page<object>.Size)))
                .ForMember(nameof(PageOutput<object>.Total), opts => opts.MapFrom(nameof(Page<object>.Total)));
        }

        #endregion
    }
}
