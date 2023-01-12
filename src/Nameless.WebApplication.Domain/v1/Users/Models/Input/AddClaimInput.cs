﻿namespace Nameless.WebApplication.Domain.v1.Users.Models.Input {

    public sealed class AddClaimInput {

        #region Public Properties

        public string Type { get; set; } = default!;
        public string Value { get; set; } = default!;

        #endregion
    }
}
