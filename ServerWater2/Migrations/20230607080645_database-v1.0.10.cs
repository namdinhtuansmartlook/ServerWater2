using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerWater2.Migrations
{
    /// <inheritdoc />
    public partial class databasev1010 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_Area_areaID",
                table: "Customer");

            migrationBuilder.RenameColumn(
                name: "areaID",
                table: "Customer",
                newName: "SqlAreaID");

            migrationBuilder.RenameIndex(
                name: "IX_Customer_areaID",
                table: "Customer",
                newName: "IX_Customer_SqlAreaID");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_Area_SqlAreaID",
                table: "Customer",
                column: "SqlAreaID",
                principalTable: "Area",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_Area_SqlAreaID",
                table: "Customer");

            migrationBuilder.RenameColumn(
                name: "SqlAreaID",
                table: "Customer",
                newName: "areaID");

            migrationBuilder.RenameIndex(
                name: "IX_Customer_SqlAreaID",
                table: "Customer",
                newName: "IX_Customer_areaID");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_Area_areaID",
                table: "Customer",
                column: "areaID",
                principalTable: "Area",
                principalColumn: "ID");
        }
    }
}
