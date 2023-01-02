using System.ComponentModel;

namespace Nameless.WebApplication.Domain.Entities {

    public enum Roles {

        [Description("NONE")]
        None,

        [Description("USER")]
        User,

        [Description("ADMINISTRATOR")]
        Administrator,

        [Description("SYSTEM_ADMINISTRATOR")]
        System
    }
}
