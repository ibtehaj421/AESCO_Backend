using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendAESCO.Migrations
{
    /// <inheritdoc />
    public partial class DocumentApprovalsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Drop the old column
            migrationBuilder.DropColumn(
                name: "ChangedBy",
                table: "DocumentVersions");

            // 2. Add the new int FK column
            migrationBuilder.AddColumn<int>(
                name: "ChangedBy",
                table: "DocumentVersions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            // 3. Add index
            migrationBuilder.CreateIndex(
                name: "IX_DocumentVersions_ChangedBy",
                table: "DocumentVersions",
                column: "ChangedBy");

            // 4. Add FK constraint
            migrationBuilder.AddForeignKey(
                name: "FK_DocumentVersions_Users_ChangedBy",
                table: "DocumentVersions",
                column: "ChangedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            // 5. Create DocumentApprovals (leave this part as-is)
            migrationBuilder.CreateTable(
                name: "DocumentApprovals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DocumentId = table.Column<Guid>(type: "uuid", nullable: false),
                    ApproverId = table.Column<int>(type: "integer", nullable: false),
                    RequestedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RespondedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentApprovals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentApprovals_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocumentApprovals_Users_ApproverId",
                        column: x => x.ApproverId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentVersions_Users_ChangedBy",
                table: "DocumentVersions");

            migrationBuilder.DropTable(
                name: "DocumentApprovals");

            migrationBuilder.DropIndex(
                name: "IX_DocumentVersions_ChangedBy",
                table: "DocumentVersions");

            migrationBuilder.AlterColumn<string>(
                name: "ChangedBy",
                table: "DocumentVersions",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}
