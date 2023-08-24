using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ServerWater2.Migrations
{
    /// <inheritdoc />
    public partial class databasev1013 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Group_groupID",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_groupID",
                table: "User");

            migrationBuilder.DropColumn(
                name: "groupID",
                table: "User");

            migrationBuilder.CreateTable(
                name: "CalcItems",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    des = table.Column<string>(type: "text", nullable: false),
                    unit = table.Column<string>(type: "text", nullable: false),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalcItems", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CalcItems");

            migrationBuilder.AddColumn<long>(
                name: "groupID",
                table: "User",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_groupID",
                table: "User",
                column: "groupID");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Group_groupID",
                table: "User",
                column: "groupID",
                principalTable: "Group",
                principalColumn: "ID");
        }
    }
}
