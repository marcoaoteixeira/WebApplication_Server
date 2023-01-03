#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace Nameless.WebApplication.Entities.Migrations {
    /// <inheritdoc />
    public partial class InitialMigration : Migration {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(256)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(2048)", nullable: false),
                    Locked = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_Users", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Claims",
                columns: table => new {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(2048)", nullable: false),
                    OwnerID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_Claims", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Claims_Users_OwnerID",
                        column: x => x.OwnerID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(2048)", nullable: false),
                    ExpiresIn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedIn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByIp = table.Column<string>(type: "nvarchar(32)", nullable: false),
                    RevokedIn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RevokedByIp = table.Column<string>(type: "nvarchar(32)", nullable: true),
                    ReplacedByToken = table.Column<string>(type: "nvarchar(2048)", nullable: true),
                    RevokeReason = table.Column<string>(type: "nvarchar(512)", nullable: true),
                    OwnerID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_RefreshTokens", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_OwnerID",
                        column: x => x.OwnerID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Claims_OwnerID",
                table: "Claims",
                column: "OwnerID");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_OwnerID",
                table: "RefreshTokens",
                column: "OwnerID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropTable(
                name: "Claims");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
