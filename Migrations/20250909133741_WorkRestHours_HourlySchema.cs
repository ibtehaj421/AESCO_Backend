using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendAESCO.Migrations
{
    /// <inheritdoc />
    public partial class WorkRestHours_HourlySchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "CrewWorkRestHours");

            migrationBuilder.DropColumn(
                name: "RestDescription",
                table: "CrewWorkRestHours");

            migrationBuilder.DropColumn(
                name: "RestHours",
                table: "CrewWorkRestHours");

            migrationBuilder.DropColumn(
                name: "TotalHours",
                table: "CrewWorkRestHours");

            migrationBuilder.DropColumn(
                name: "WorkHours",
                table: "CrewWorkRestHours");

            migrationBuilder.RenameColumn(
                name: "WorkDescription",
                table: "CrewWorkRestHours",
                newName: "Description");

            migrationBuilder.AddColumn<int>(
                name: "Day",
                table: "CrewWorkRestHours",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Hour",
                table: "CrewWorkRestHours",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsWorking",
                table: "CrewWorkRestHours",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Month",
                table: "CrewWorkRestHours",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "CrewWorkRestHours",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Day",
                table: "CrewWorkRestHours");

            migrationBuilder.DropColumn(
                name: "Hour",
                table: "CrewWorkRestHours");

            migrationBuilder.DropColumn(
                name: "IsWorking",
                table: "CrewWorkRestHours");

            migrationBuilder.DropColumn(
                name: "Month",
                table: "CrewWorkRestHours");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "CrewWorkRestHours");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "CrewWorkRestHours",
                newName: "WorkDescription");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "CrewWorkRestHours",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "RestDescription",
                table: "CrewWorkRestHours",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "RestHours",
                table: "CrewWorkRestHours",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalHours",
                table: "CrewWorkRestHours",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "WorkHours",
                table: "CrewWorkRestHours",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
