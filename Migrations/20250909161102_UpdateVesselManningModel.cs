using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendAESCO.Migrations
{
    /// <inheritdoc />
    public partial class UpdateVesselManningModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "VesselMannings",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CurrentCount",
                table: "VesselMannings",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "VesselMannings",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RequiredCount",
                table: "VesselMannings",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "VesselMannings",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "VesselMannings");

            migrationBuilder.DropColumn(
                name: "CurrentCount",
                table: "VesselMannings");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "VesselMannings");

            migrationBuilder.DropColumn(
                name: "RequiredCount",
                table: "VesselMannings");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "VesselMannings");
        }
    }
}
