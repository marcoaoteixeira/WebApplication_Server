using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nameless.WebApplication.Entities.Migrations {
    /// <inheritdoc />
    public partial class AddFieldRoleToUser : Migration {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Users",
                type: "nvarchar(64)",
                nullable: false,
                defaultValue: Roles.User.ToString());
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "Users");
        }
    }
}
