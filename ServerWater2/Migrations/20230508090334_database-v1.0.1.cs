using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ServerWater2.Migrations
{
    /// <inheritdoc />
    public partial class databasev101 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Request_requestID",
                table: "Order");

            migrationBuilder.DropTable(
                name: "Request");

            migrationBuilder.RenameColumn(
                name: "requestID",
                table: "Order",
                newName: "typeID");

            migrationBuilder.RenameIndex(
                name: "IX_Order_requestID",
                table: "Order",
                newName: "IX_Order_typeID");

            migrationBuilder.AddColumn<List<string>>(
                name: "images",
                table: "Order",
                type: "text[]",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Type",
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
                    table.PrimaryKey("PK_Type", x => x.ID);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Type_typeID",
                table: "Order",
                column: "typeID",
                principalTable: "Type",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Type_typeID",
                table: "Order");

            migrationBuilder.DropTable(
                name: "Type");

            migrationBuilder.DropColumn(
                name: "images",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "typeID",
                table: "Order",
                newName: "requestID");

            migrationBuilder.RenameIndex(
                name: "IX_Order_typeID",
                table: "Order",
                newName: "IX_Order_requestID");

            migrationBuilder.CreateTable(
                name: "Request",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "text", nullable: false),
                    des = table.Column<string>(type: "text", nullable: false),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Request", x => x.ID);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Request_requestID",
                table: "Order",
                column: "requestID",
                principalTable: "Request",
                principalColumn: "ID");
        }
    }
}
