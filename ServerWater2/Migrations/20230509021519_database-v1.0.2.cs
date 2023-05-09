using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerWater2.Migrations
{
    /// <inheritdoc />
    public partial class databasev102 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_User_userID",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "userID",
                table: "Order",
                newName: "workerID");

            migrationBuilder.RenameIndex(
                name: "IX_Order_userID",
                table: "Order",
                newName: "IX_Order_workerID");

            migrationBuilder.AddColumn<string>(
                name: "latitude",
                table: "Order",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "longitude",
                table: "Order",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "managerID",
                table: "Order",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "receiverID",
                table: "Order",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_managerID",
                table: "Order",
                column: "managerID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_receiverID",
                table: "Order",
                column: "receiverID");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_User_managerID",
                table: "Order",
                column: "managerID",
                principalTable: "User",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_User_receiverID",
                table: "Order",
                column: "receiverID",
                principalTable: "User",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_User_workerID",
                table: "Order",
                column: "workerID",
                principalTable: "User",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_User_managerID",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_User_receiverID",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_User_workerID",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_managerID",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_receiverID",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "latitude",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "longitude",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "managerID",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "receiverID",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "workerID",
                table: "Order",
                newName: "userID");

            migrationBuilder.RenameIndex(
                name: "IX_Order_workerID",
                table: "Order",
                newName: "IX_Order_userID");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_User_userID",
                table: "Order",
                column: "userID",
                principalTable: "User",
                principalColumn: "ID");
        }
    }
}
