using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BackendAESCO.Migrations
{
    /// <inheritdoc />
    public partial class ExpensesTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CrewExpenseReport",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CrewMemberId = table.Column<int>(type: "integer", nullable: false),
                    ShipId = table.Column<int>(type: "integer", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    ReportDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrewExpenseReport", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CrewExpenseReport_Ships_ShipId",
                        column: x => x.ShipId,
                        principalTable: "Ships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CrewExpenseReport_Users_CrewMemberId",
                        column: x => x.CrewMemberId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CrewPayrolls",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CrewMemberId = table.Column<int>(type: "integer", nullable: false),
                    PeriodStart = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PeriodEnd = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BaseWage = table.Column<decimal>(type: "numeric", nullable: false),
                    Overtime = table.Column<decimal>(type: "numeric", nullable: false),
                    Bonuses = table.Column<decimal>(type: "numeric", nullable: false),
                    Deductions = table.Column<decimal>(type: "numeric", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PaymentMethod = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrewPayrolls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CrewPayrolls_Users_CrewMemberId",
                        column: x => x.CrewMemberId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StatementOfCash",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VesselId = table.Column<int>(type: "integer", nullable: false),
                    CreatedById = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Inflow = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    Outflow = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    Balance = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatementOfCash", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StatementOfCash_Ships_VesselId",
                        column: x => x.VesselId,
                        principalTable: "Ships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StatementOfCash_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VesselMannings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VesselId = table.Column<int>(type: "integer", nullable: false),
                    Rank = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VesselMannings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VesselMannings_Ships_VesselId",
                        column: x => x.VesselId,
                        principalTable: "Ships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CrewExpenses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExpenseReportId = table.Column<long>(type: "bigint", nullable: false),
                    CrewMemberId = table.Column<int>(type: "integer", nullable: false),
                    Category = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Currency = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    ExpenseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrewExpenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CrewExpenses_CrewExpenseReport_ExpenseReportId",
                        column: x => x.ExpenseReportId,
                        principalTable: "CrewExpenseReport",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CrewExpenses_Users_CrewMemberId",
                        column: x => x.CrewMemberId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CrewExpenseReport_CrewMemberId",
                table: "CrewExpenseReport",
                column: "CrewMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_CrewExpenseReport_ShipId",
                table: "CrewExpenseReport",
                column: "ShipId");

            migrationBuilder.CreateIndex(
                name: "IX_CrewExpenses_CrewMemberId",
                table: "CrewExpenses",
                column: "CrewMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_CrewExpenses_ExpenseReportId",
                table: "CrewExpenses",
                column: "ExpenseReportId");

            migrationBuilder.CreateIndex(
                name: "IX_CrewPayrolls_CrewMemberId",
                table: "CrewPayrolls",
                column: "CrewMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_StatementOfCash_CreatedById",
                table: "StatementOfCash",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_StatementOfCash_VesselId",
                table: "StatementOfCash",
                column: "VesselId");

            migrationBuilder.CreateIndex(
                name: "IX_VesselMannings_VesselId",
                table: "VesselMannings",
                column: "VesselId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CrewExpenses");

            migrationBuilder.DropTable(
                name: "CrewPayrolls");

            migrationBuilder.DropTable(
                name: "StatementOfCash");

            migrationBuilder.DropTable(
                name: "VesselMannings");

            migrationBuilder.DropTable(
                name: "CrewExpenseReport");
        }
    }
}
