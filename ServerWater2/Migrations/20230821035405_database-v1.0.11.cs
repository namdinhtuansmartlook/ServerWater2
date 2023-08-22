using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerWater2.Migrations
{
    /// <inheritdoc />
    public partial class databasev1011 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "persons",
                table: "Order",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "persons",
                table: "Customer",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "persons",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "persons",
                table: "Customer");
        }
    }
}
