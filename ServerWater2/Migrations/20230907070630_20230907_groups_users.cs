using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerWater2.Migrations
{
    /// <inheritdoc />
    public partial class _20230907_groups_users : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SqlGroupSqlUser",
                columns: table => new
                {
                    groupsID = table.Column<long>(type: "bigint", nullable: false),
                    usersID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SqlGroupSqlUser", x => new { x.groupsID, x.usersID });
                    table.ForeignKey(
                        name: "FK_SqlGroupSqlUser_Group_groupsID",
                        column: x => x.groupsID,
                        principalTable: "Group",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SqlGroupSqlUser_User_usersID",
                        column: x => x.usersID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SqlGroupSqlUser_usersID",
                table: "SqlGroupSqlUser",
                column: "usersID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SqlGroupSqlUser");
        }
    }
}
