using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerWater2.Migrations
{
    /// <inheritdoc />
    public partial class databasev1014 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "idToken",
                table: "User");

            migrationBuilder.AddColumn<bool>(
                name: "isClear",
                table: "User",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "notification",
                table: "User",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isClear",
                table: "User");

            migrationBuilder.DropColumn(
                name: "notification",
                table: "User");

            migrationBuilder.AddColumn<List<string>>(
                name: "idToken",
                table: "User",
                type: "text[]",
                nullable: true);
        }
    }
}
