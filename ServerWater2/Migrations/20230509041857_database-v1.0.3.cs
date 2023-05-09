using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ServerWater2.Migrations
{
    /// <inheritdoc />
    public partial class databasev103 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogAction");

            migrationBuilder.DropTable(
                name: "LogRequest");

            migrationBuilder.DropTable(
                name: "SqlCustomerSqlUser");

            migrationBuilder.DropColumn(
                name: "images",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "diachiLD",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "diachiLH",
                table: "Customer");

            migrationBuilder.RenameColumn(
                name: "longitude",
                table: "Order",
                newName: "phone");

            migrationBuilder.RenameColumn(
                name: "latitude",
                table: "Order",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "tenKH",
                table: "Customer",
                newName: "phone");

            migrationBuilder.RenameColumn(
                name: "sdt",
                table: "Customer",
                newName: "note");

            migrationBuilder.RenameColumn(
                name: "maDB",
                table: "Customer",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "idKH",
                table: "Customer",
                newName: "code");

            migrationBuilder.RenameColumn(
                name: "diachiTT",
                table: "Customer",
                newName: "address");

            migrationBuilder.AddColumn<string>(
                name: "addressContract",
                table: "Order",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "addressCustomer",
                table: "Order",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "addressWater",
                table: "Order",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<List<string>>(
                name: "images",
                table: "Customer",
                type: "text[]",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Action",
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
                    table.PrimaryKey("PK_Action", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LogOrder",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    orderID = table.Column<long>(type: "bigint", nullable: true),
                    userID = table.Column<long>(type: "bigint", nullable: true),
                    actionID = table.Column<long>(type: "bigint", nullable: true),
                    time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    note = table.Column<string>(type: "text", nullable: false),
                    latitude = table.Column<string>(type: "text", nullable: false),
                    longitude = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogOrder", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LogOrder_Action_actionID",
                        column: x => x.actionID,
                        principalTable: "Action",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_LogOrder_Order_orderID",
                        column: x => x.orderID,
                        principalTable: "Order",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_LogOrder_User_userID",
                        column: x => x.userID,
                        principalTable: "User",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_LogOrder_actionID",
                table: "LogOrder",
                column: "actionID");

            migrationBuilder.CreateIndex(
                name: "IX_LogOrder_orderID",
                table: "LogOrder",
                column: "orderID");

            migrationBuilder.CreateIndex(
                name: "IX_LogOrder_userID",
                table: "LogOrder",
                column: "userID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogOrder");

            migrationBuilder.DropTable(
                name: "Action");

            migrationBuilder.DropColumn(
                name: "addressContract",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "addressCustomer",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "addressWater",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "images",
                table: "Customer");

            migrationBuilder.RenameColumn(
                name: "phone",
                table: "Order",
                newName: "longitude");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Order",
                newName: "latitude");

            migrationBuilder.RenameColumn(
                name: "phone",
                table: "Customer",
                newName: "tenKH");

            migrationBuilder.RenameColumn(
                name: "note",
                table: "Customer",
                newName: "sdt");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Customer",
                newName: "maDB");

            migrationBuilder.RenameColumn(
                name: "code",
                table: "Customer",
                newName: "idKH");

            migrationBuilder.RenameColumn(
                name: "address",
                table: "Customer",
                newName: "diachiTT");

            migrationBuilder.AddColumn<List<string>>(
                name: "images",
                table: "Order",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "diachiLD",
                table: "Customer",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "diachiLH",
                table: "Customer",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "LogAction",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    orderID = table.Column<long>(type: "bigint", nullable: true),
                    stateID = table.Column<long>(type: "bigint", nullable: true),
                    userID = table.Column<long>(type: "bigint", nullable: true),
                    note = table.Column<string>(type: "text", nullable: false),
                    time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogAction", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LogAction_Order_orderID",
                        column: x => x.orderID,
                        principalTable: "Order",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_LogAction_State_stateID",
                        column: x => x.stateID,
                        principalTable: "State",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_LogAction_User_userID",
                        column: x => x.userID,
                        principalTable: "User",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "LogRequest",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    customerID = table.Column<long>(type: "bigint", nullable: true),
                    orderID = table.Column<long>(type: "bigint", nullable: true),
                    userID = table.Column<long>(type: "bigint", nullable: true),
                    note = table.Column<string>(type: "text", nullable: false),
                    time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogRequest", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LogRequest_Customer_customerID",
                        column: x => x.customerID,
                        principalTable: "Customer",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_LogRequest_Order_orderID",
                        column: x => x.orderID,
                        principalTable: "Order",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_LogRequest_User_userID",
                        column: x => x.userID,
                        principalTable: "User",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "SqlCustomerSqlUser",
                columns: table => new
                {
                    customersID = table.Column<long>(type: "bigint", nullable: false),
                    usersID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SqlCustomerSqlUser", x => new { x.customersID, x.usersID });
                    table.ForeignKey(
                        name: "FK_SqlCustomerSqlUser_Customer_customersID",
                        column: x => x.customersID,
                        principalTable: "Customer",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SqlCustomerSqlUser_User_usersID",
                        column: x => x.usersID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LogAction_orderID",
                table: "LogAction",
                column: "orderID");

            migrationBuilder.CreateIndex(
                name: "IX_LogAction_stateID",
                table: "LogAction",
                column: "stateID");

            migrationBuilder.CreateIndex(
                name: "IX_LogAction_userID",
                table: "LogAction",
                column: "userID");

            migrationBuilder.CreateIndex(
                name: "IX_LogRequest_customerID",
                table: "LogRequest",
                column: "customerID");

            migrationBuilder.CreateIndex(
                name: "IX_LogRequest_orderID",
                table: "LogRequest",
                column: "orderID");

            migrationBuilder.CreateIndex(
                name: "IX_LogRequest_userID",
                table: "LogRequest",
                column: "userID");

            migrationBuilder.CreateIndex(
                name: "IX_SqlCustomerSqlUser_usersID",
                table: "SqlCustomerSqlUser",
                column: "usersID");
        }
    }
}
