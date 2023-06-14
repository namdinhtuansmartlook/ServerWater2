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
            migrationBuilder.AddColumn<long>(
                name: "SqlCustomerID",
                table: "Device",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Device_SqlCustomerID",
                table: "Device",
                column: "SqlCustomerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Device_Customer_SqlCustomerID",
                table: "Device",
                column: "SqlCustomerID",
                principalTable: "Customer",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Device_Customer_SqlCustomerID",
                table: "Device");

            migrationBuilder.DropIndex(
                name: "IX_Device_SqlCustomerID",
                table: "Device");

            migrationBuilder.DropColumn(
                name: "SqlCustomerID",
                table: "Device");
        }
    }
}
