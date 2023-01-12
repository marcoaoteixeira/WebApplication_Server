﻿using System.Security.Claims;
using AutoMapper;
using Nameless.WebApplication.Domain.v1.Users.Models.Input;

namespace Nameless.WebApplication.Domain.v1.Users.Mappings {

    public sealed class AddClaimInput_Claim : Profile {

        #region Public Constructors

        public AddClaimInput_Claim() {
            CreateMap<AddClaimInput, Claim>();
        }

        #endregion
    }
}
