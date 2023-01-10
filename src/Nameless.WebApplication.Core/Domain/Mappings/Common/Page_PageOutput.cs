using AutoMapper;
using Nameless.WebApplication.Collections.Generic;
using Nameless.WebApplication.Domain.Output;

namespace Nameless.WebApplication.Domain.Mappings.Common {

    public sealed class Page_PageOutput : Profile {

        #region Public Constructors

        public Page_PageOutput() {
            CreateMap(typeof(Page<>), typeof(PageOutput<>))
                .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
                .ForMember(
                    name: nameof(PageOutput<object>.Items),
                    memberOptions: opts => opts.MapFrom(src => src)
                );
        }

        #endregion
    }
}
