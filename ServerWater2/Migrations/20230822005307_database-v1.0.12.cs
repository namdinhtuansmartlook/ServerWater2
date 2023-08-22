using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerWater2.Migrations
{
    /// <inheritdoc />
    public partial class databasev1012 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SqlAreaSqlGroup");

            migrationBuilder.DropColumn(
                name: "isClear",
                table: "User");

            migrationBuilder.DropColumn(
                name: "notifications",
                table: "User");

            migrationBuilder.AddColumn<List<string>>(
                name: "idToken",
                table: "User",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "groupID",
                table: "Area",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Area_groupID",
                table: "Area",
                column: "groupID");

            migrationBuilder.AddForeignKey(
                name: "FK_Area_Group_groupID",
                table: "Area",
                column: "groupID",
                principalTable: "Group",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Area_Group_groupID",
                table: "Area");

            migrationBuilder.DropIndex(
                name: "IX_Area_groupID",
                table: "Area");

            migrationBuilder.DropColumn(
                name: "idToken",
                table: "User");

            migrationBuilder.DropColumn(
                name: "groupID",
                table: "Area");

            migrationBuilder.AddColumn<bool>(
                name: "isClear",
                table: "User",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "notifications",
                table: "User",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "SqlAreaSqlGroup",
                columns: table => new
                {
                    areasID = table.Column<long>(type: "bigint", nullable: false),
                    groupsID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SqlAreaSqlGroup", x => new { x.areasID, x.groupsID });
                    table.ForeignKey(
                        name: "FK_SqlAreaSqlGroup_Area_areasID",
                        column: x => x.areasID,
                        principalTable: "Area",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SqlAreaSqlGroup_Group_groupsID",
                        column: x => x.groupsID,
                        principalTable: "Group",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SqlAreaSqlGroup_groupsID",
                table: "SqlAreaSqlGroup",
                column: "groupsID");
        }
    }
}
