using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ServerWater2.Migrations
{
    /// <inheritdoc />
    public partial class databasev108 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "groupID",
                table: "User",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "areaID",
                table: "Order",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "document",
                table: "Order",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "groupID",
                table: "Order",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Area",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    des = table.Column<string>(type: "text", nullable: false),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Area", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Certificate",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    des = table.Column<string>(type: "text", nullable: false),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: false),
                    SqlOrderID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Certificate", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Certificate_Order_SqlOrderID",
                        column: x => x.SqlOrderID,
                        principalTable: "Order",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Group",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    des = table.Column<string>(type: "text", nullable: false),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Group", x => x.ID);
                });

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
                name: "IX_User_groupID",
                table: "User",
                column: "groupID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_areaID",
                table: "Order",
                column: "areaID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_groupID",
                table: "Order",
                column: "groupID");

            migrationBuilder.CreateIndex(
                name: "IX_Certificate_SqlOrderID",
                table: "Certificate",
                column: "SqlOrderID");

            migrationBuilder.CreateIndex(
                name: "IX_SqlAreaSqlGroup_groupsID",
                table: "SqlAreaSqlGroup",
                column: "groupsID");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Area_areaID",
                table: "Order",
                column: "areaID",
                principalTable: "Area",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Group_groupID",
                table: "Order",
                column: "groupID",
                principalTable: "Group",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Group_groupID",
                table: "User",
                column: "groupID",
                principalTable: "Group",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Area_areaID",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Group_groupID",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Group_groupID",
                table: "User");

            migrationBuilder.DropTable(
                name: "Certificate");

            migrationBuilder.DropTable(
                name: "SqlAreaSqlGroup");

            migrationBuilder.DropTable(
                name: "Area");

            migrationBuilder.DropTable(
                name: "Group");

            migrationBuilder.DropIndex(
                name: "IX_User_groupID",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_Order_areaID",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_groupID",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "groupID",
                table: "User");

            migrationBuilder.DropColumn(
                name: "areaID",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "document",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "groupID",
                table: "Order");
        }
    }
}
