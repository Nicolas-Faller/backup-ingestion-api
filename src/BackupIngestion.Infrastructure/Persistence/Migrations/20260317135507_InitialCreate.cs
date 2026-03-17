using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackupIngestion.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BackupExecutions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ClientName = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    JobName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    SourceType = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    StartedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FinishedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DurationSeconds = table.Column<int>(type: "INTEGER", nullable: false),
                    DataSizeBytes = table.Column<long>(type: "INTEGER", nullable: false),
                    BackupSizeBytes = table.Column<long>(type: "INTEGER", nullable: false),
                    TransferredSizeBytes = table.Column<long>(type: "INTEGER", nullable: false),
                    Message = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    ImportedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BackupExecutions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BackupExecutions_ClientName",
                table: "BackupExecutions",
                column: "ClientName");

            migrationBuilder.CreateIndex(
                name: "IX_BackupExecutions_SourceType",
                table: "BackupExecutions",
                column: "SourceType");

            migrationBuilder.CreateIndex(
                name: "IX_BackupExecutions_StartedAtUtc",
                table: "BackupExecutions",
                column: "StartedAtUtc");

            migrationBuilder.CreateIndex(
                name: "IX_BackupExecutions_Status",
                table: "BackupExecutions",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BackupExecutions");
        }
    }
}
