using System.ComponentModel;

namespace Nameless.WebApplication.Entities {

    public enum Roles {

        [Description("USER")]
        User,

        [Description("SUPER_USER")]
        SuperUser,

        [Description("ADMINISTRATOR")]
        Administrator,

        [Description("SYSTEM_ADMINISTRATOR")]
        SystemAdministrator
    }
}
