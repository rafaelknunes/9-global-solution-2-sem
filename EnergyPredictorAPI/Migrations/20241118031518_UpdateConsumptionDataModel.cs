using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnergyPredictorAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateConsumptionDataModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Timestamp",
                table: "ConsumptionData",
                type: "TIMESTAMP",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP(7)",
                oldDefaultValueSql: "SYSDATE");

            migrationBuilder.AlterColumn<string>(
                name: "DeviceType",
                table: "ConsumptionData",
                type: "NVARCHAR2(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(2000)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Timestamp",
                table: "ConsumptionData",
                type: "TIMESTAMP(7)",
                nullable: false,
                defaultValueSql: "SYSDATE",
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP");

            migrationBuilder.AlterColumn<string>(
                name: "DeviceType",
                table: "ConsumptionData",
                type: "NVARCHAR2(2000)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(50)",
                oldMaxLength: 50);
        }
    }
}
