using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerWater2.Migrations
{
    /// <inheritdoc />
    public partial class databasev109 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Certificate_Order_SqlOrderID",
                table: "Certificate");

            migrationBuilder.DropIndex(
                name: "IX_Certificate_SqlOrderID",
                table: "Certificate");

            migrationBuilder.DropColumn(
                name: "SqlOrderID",
                table: "Certificate");

            migrationBuilder.AddColumn<List<string>>(
                name: "certificates",
                table: "Order",
                type: "text[]",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "certificates",
                table: "Order");

            migrationBuilder.AddColumn<long>(
                name: "SqlOrderID",
                table: "Certificate",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Certificate_SqlOrderID",
                table: "Certificate",
                column: "SqlOrderID");

            migrationBuilder.AddForeignKey(
                name: "FK_Certificate_Order_SqlOrderID",
                table: "Certificate",
                column: "SqlOrderID",
                principalTable: "Order",
                principalColumn: "ID");
        }
    }
}
