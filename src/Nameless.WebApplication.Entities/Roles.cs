using System.ComponentModel;

namespace Nameless.WebApplication.Entities {

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
