using System;
using System.Collections.Generic;
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
                name: "SqlAreaID",
                table: "Customer",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "route",
                table: "Customer",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Area",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "text", nullable: false),
                    nameArea = table.Column<string>(type: "text", nullable: false),
                    des = table.Column<string>(type: "text", nullable: false),
                    createdTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    lastestTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Area", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Device",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "text", nullable: false),
                    nameDevice = table.Column<string>(type: "text", nullable: false),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: false),
                    createdTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    lastestTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    startTimeSChedule = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    typeID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Device", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Device_Type_typeID",
                        column: x => x.typeID,
                        principalTable: "Type",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Layer",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "text", nullable: false),
                    nameLayer = table.Column<string>(type: "text", nullable: false),
                    des = table.Column<string>(type: "text", nullable: false),
                    createdTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    lastestTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Layer", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Point",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "text", nullable: false),
                    namePoint = table.Column<string>(type: "text", nullable: false),
                    des = table.Column<string>(type: "text", nullable: false),
                    longitude = table.Column<string>(type: "text", nullable: false),
                    latitude = table.Column<string>(type: "text", nullable: false),
                    note = table.Column<string>(type: "text", nullable: false),
                    imageShow = table.Column<string>(type: "text", nullable: false),
                    images = table.Column<List<string>>(type: "text[]", nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: false),
                    createdTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    lastestTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Point", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Schedule",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "text", nullable: false),
                    des = table.Column<string>(type: "text", nullable: false),
                    period = table.Column<string>(type: "text", nullable: false),
                    note = table.Column<string>(type: "text", nullable: false),
                    createdTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    lastestTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: false),
                    SqlTypeID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedule", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Schedule_Type_SqlTypeID",
                        column: x => x.SqlTypeID,
                        principalTable: "Type",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "text", nullable: false),
                    nameStatus = table.Column<string>(type: "text", nullable: false),
                    isOnline = table.Column<bool>(type: "boolean", nullable: false),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: false),
                    lastestTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    createdTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Value",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nameValue = table.Column<string>(type: "text", nullable: false),
                    unit = table.Column<string>(type: "text", nullable: false),
                    des = table.Column<string>(type: "text", nullable: false),
                    createdTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: false),
                    SqlTypeID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Value", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Value_Type_SqlTypeID",
                        column: x => x.SqlTypeID,
                        principalTable: "Type",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "SqlAreaSqlUser",
                columns: table => new
                {
                    areasID = table.Column<long>(type: "bigint", nullable: false),
                    usersID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SqlAreaSqlUser", x => new { x.areasID, x.usersID });
                    table.ForeignKey(
                        name: "FK_SqlAreaSqlUser_Area_areasID",
                        column: x => x.areasID,
                        principalTable: "Area",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SqlAreaSqlUser_User_usersID",
                        column: x => x.usersID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SqlDeviceSqlLayer",
                columns: table => new
                {
                    devicesID = table.Column<long>(type: "bigint", nullable: false),
                    layersID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SqlDeviceSqlLayer", x => new { x.devicesID, x.layersID });
                    table.ForeignKey(
                        name: "FK_SqlDeviceSqlLayer_Device_devicesID",
                        column: x => x.devicesID,
                        principalTable: "Device",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SqlDeviceSqlLayer_Layer_layersID",
                        column: x => x.layersID,
                        principalTable: "Layer",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SqlAreaSqlPoint",
                columns: table => new
                {
                    areasID = table.Column<long>(type: "bigint", nullable: false),
                    pointsID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SqlAreaSqlPoint", x => new { x.areasID, x.pointsID });
                    table.ForeignKey(
                        name: "FK_SqlAreaSqlPoint_Area_areasID",
                        column: x => x.areasID,
                        principalTable: "Area",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SqlAreaSqlPoint_Point_pointsID",
                        column: x => x.pointsID,
                        principalTable: "Point",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SqlDeviceSqlPoint",
                columns: table => new
                {
                    devicesID = table.Column<long>(type: "bigint", nullable: false),
                    pointsID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SqlDeviceSqlPoint", x => new { x.devicesID, x.pointsID });
                    table.ForeignKey(
                        name: "FK_SqlDeviceSqlPoint_Device_devicesID",
                        column: x => x.devicesID,
                        principalTable: "Device",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SqlDeviceSqlPoint_Point_pointsID",
                        column: x => x.pointsID,
                        principalTable: "Point",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LogDevice",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    pointID = table.Column<long>(type: "bigint", nullable: true),
                    deviceID = table.Column<long>(type: "bigint", nullable: true),
                    scheduleID = table.Column<long>(type: "bigint", nullable: true),
                    userID = table.Column<long>(type: "bigint", nullable: true),
                    images = table.Column<List<string>>(type: "text[]", nullable: true),
                    note = table.Column<string>(type: "text", nullable: false),
                    timeDo = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    timeRef = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogDevice", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LogDevice_Device_deviceID",
                        column: x => x.deviceID,
                        principalTable: "Device",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_LogDevice_Point_pointID",
                        column: x => x.pointID,
                        principalTable: "Point",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_LogDevice_Schedule_scheduleID",
                        column: x => x.scheduleID,
                        principalTable: "Schedule",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_LogDevice_User_userID",
                        column: x => x.userID,
                        principalTable: "User",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "SqlStatusSqlType",
                columns: table => new
                {
                    statussID = table.Column<long>(type: "bigint", nullable: false),
                    typesID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SqlStatusSqlType", x => new { x.statussID, x.typesID });
                    table.ForeignKey(
                        name: "FK_SqlStatusSqlType_Status_statussID",
                        column: x => x.statussID,
                        principalTable: "Status",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SqlStatusSqlType_Type_typesID",
                        column: x => x.typesID,
                        principalTable: "Type",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LogValue",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    deviceID = table.Column<long>(type: "bigint", nullable: true),
                    valueConfigID = table.Column<long>(type: "bigint", nullable: true),
                    value = table.Column<string>(type: "text", nullable: false),
                    time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogValue", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LogValue_Device_deviceID",
                        column: x => x.deviceID,
                        principalTable: "Device",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_LogValue_Value_valueConfigID",
                        column: x => x.valueConfigID,
                        principalTable: "Value",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customer_SqlAreaID",
                table: "Customer",
                column: "SqlAreaID");

            migrationBuilder.CreateIndex(
                name: "IX_Device_typeID",
                table: "Device",
                column: "typeID");

            migrationBuilder.CreateIndex(
                name: "IX_LogDevice_deviceID",
                table: "LogDevice",
                column: "deviceID");

            migrationBuilder.CreateIndex(
                name: "IX_LogDevice_pointID",
                table: "LogDevice",
                column: "pointID");

            migrationBuilder.CreateIndex(
                name: "IX_LogDevice_scheduleID",
                table: "LogDevice",
                column: "scheduleID");

            migrationBuilder.CreateIndex(
                name: "IX_LogDevice_userID",
                table: "LogDevice",
                column: "userID");

            migrationBuilder.CreateIndex(
                name: "IX_LogValue_deviceID",
                table: "LogValue",
                column: "deviceID");

            migrationBuilder.CreateIndex(
                name: "IX_LogValue_valueConfigID",
                table: "LogValue",
                column: "valueConfigID");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_SqlTypeID",
                table: "Schedule",
                column: "SqlTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_SqlAreaSqlPoint_pointsID",
                table: "SqlAreaSqlPoint",
                column: "pointsID");

            migrationBuilder.CreateIndex(
                name: "IX_SqlAreaSqlUser_usersID",
                table: "SqlAreaSqlUser",
                column: "usersID");

            migrationBuilder.CreateIndex(
                name: "IX_SqlDeviceSqlLayer_layersID",
                table: "SqlDeviceSqlLayer",
                column: "layersID");

            migrationBuilder.CreateIndex(
                name: "IX_SqlDeviceSqlPoint_pointsID",
                table: "SqlDeviceSqlPoint",
                column: "pointsID");

            migrationBuilder.CreateIndex(
                name: "IX_SqlStatusSqlType_typesID",
                table: "SqlStatusSqlType",
                column: "typesID");

            migrationBuilder.CreateIndex(
                name: "IX_Value_SqlTypeID",
                table: "Value",
                column: "SqlTypeID");

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

            migrationBuilder.DropTable(
                name: "LogDevice");

            migrationBuilder.DropTable(
                name: "LogValue");

            migrationBuilder.DropTable(
                name: "SqlAreaSqlPoint");

            migrationBuilder.DropTable(
                name: "SqlAreaSqlUser");

            migrationBuilder.DropTable(
                name: "SqlDeviceSqlLayer");

            migrationBuilder.DropTable(
                name: "SqlDeviceSqlPoint");

            migrationBuilder.DropTable(
                name: "SqlStatusSqlType");

            migrationBuilder.DropTable(
                name: "Schedule");

            migrationBuilder.DropTable(
                name: "Value");

            migrationBuilder.DropTable(
                name: "Area");

            migrationBuilder.DropTable(
                name: "Layer");

            migrationBuilder.DropTable(
                name: "Device");

            migrationBuilder.DropTable(
                name: "Point");

            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.DropIndex(
                name: "IX_Customer_SqlAreaID",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "SqlAreaID",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "route",
                table: "Customer");
        }
    }
}
