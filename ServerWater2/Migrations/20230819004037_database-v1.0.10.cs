using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ServerWater2.Migrations
{
    /// <inheritdoc />
    public partial class databasev1010 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "surveyID",
                table: "Order",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ViewForm",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    data = table.Column<string>(type: "text", nullable: false),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ViewForm", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Order_surveyID",
                table: "Order",
                column: "surveyID");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_User_surveyID",
                table: "Order",
                column: "surveyID",
                principalTable: "User",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_User_surveyID",
                table: "Order");

            migrationBuilder.DropTable(
                name: "ViewForm");

            migrationBuilder.DropIndex(
                name: "IX_Order_surveyID",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "surveyID",
                table: "Order");
        }
    }
}
