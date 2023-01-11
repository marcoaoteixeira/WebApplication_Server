﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nameless.WebApplication.Entities.Migrations
{
    /// <inheritdoc />
    public partial class UserAvatarUrlField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AvatarUrl",
                schema: "dbo",
                table: "Users",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarUrl",
                schema: "dbo",
                table: "Users");
        }
    }
}
