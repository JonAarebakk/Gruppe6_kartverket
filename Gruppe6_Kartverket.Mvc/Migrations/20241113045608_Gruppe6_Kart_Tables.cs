using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gruppe6_Kartverket.Mvc.Migrations
{
    /// <inheritdoc />
    public partial class Gruppe6_Kart_Tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CaseRecord_CaseLocations_LocationId",
                table: "CaseRecord");

            migrationBuilder.DropForeignKey(
                name: "FK_CaseRecord_Users_UserId",
                table: "CaseRecord");

            migrationBuilder.DropTable(
                name: "Cases");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CaseRecord",
                table: "CaseRecord");

            migrationBuilder.RenameTable(
                name: "CaseRecord",
                newName: "CaseRecords");

            migrationBuilder.RenameColumn(
                name: "CaseId",
                table: "CaseRecords",
                newName: "CaseRecordId");

            migrationBuilder.RenameIndex(
                name: "IX_CaseRecord_UserId",
                table: "CaseRecords",
                newName: "IX_CaseRecords_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_CaseRecord_LocationId",
                table: "CaseRecords",
                newName: "IX_CaseRecords_LocationId");

            migrationBuilder.AddColumn<int>(
                name: "CaseLocationLocationId",
                table: "CaseRecords",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CaseRecords",
                table: "CaseRecords",
                column: "CaseRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseRecords_CaseLocationLocationId",
                table: "CaseRecords",
                column: "CaseLocationLocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_CaseRecords_CaseLocations_CaseLocationLocationId",
                table: "CaseRecords",
                column: "CaseLocationLocationId",
                principalTable: "CaseLocations",
                principalColumn: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_CaseRecords_CaseLocations_LocationId",
                table: "CaseRecords",
                column: "LocationId",
                principalTable: "CaseLocations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseRecords_Users_UserId",
                table: "CaseRecords",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CaseRecords_CaseLocations_CaseLocationLocationId",
                table: "CaseRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_CaseRecords_CaseLocations_LocationId",
                table: "CaseRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_CaseRecords_Users_UserId",
                table: "CaseRecords");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CaseRecords",
                table: "CaseRecords");

            migrationBuilder.DropIndex(
                name: "IX_CaseRecords_CaseLocationLocationId",
                table: "CaseRecords");

            migrationBuilder.DropColumn(
                name: "CaseLocationLocationId",
                table: "CaseRecords");

            migrationBuilder.RenameTable(
                name: "CaseRecords",
                newName: "CaseRecord");

            migrationBuilder.RenameColumn(
                name: "CaseRecordId",
                table: "CaseRecord",
                newName: "CaseId");

            migrationBuilder.RenameIndex(
                name: "IX_CaseRecords_UserId",
                table: "CaseRecord",
                newName: "IX_CaseRecord_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_CaseRecords_LocationId",
                table: "CaseRecord",
                newName: "IX_CaseRecord_LocationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CaseRecord",
                table: "CaseRecord",
                column: "CaseId");

            migrationBuilder.CreateTable(
                name: "Cases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CaseLocationLocationId = table.Column<int>(type: "int", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Title = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cases_CaseLocations_CaseLocationLocationId",
                        column: x => x.CaseLocationLocationId,
                        principalTable: "CaseLocations",
                        principalColumn: "LocationId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Cases_CaseLocationLocationId",
                table: "Cases",
                column: "CaseLocationLocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_CaseRecord_CaseLocations_LocationId",
                table: "CaseRecord",
                column: "LocationId",
                principalTable: "CaseLocations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseRecord_Users_UserId",
                table: "CaseRecord",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
