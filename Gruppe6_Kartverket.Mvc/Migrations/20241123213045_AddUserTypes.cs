using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gruppe6_Kartverket.Mvc.Migrations
{
    /// <inheritdoc />
    public partial class AddUserTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "UserTypes",
                columns: new[] { "UserType", "UserTypeDescription" },
                values: new object[,]
                {
                    { "Us", "User" },
                    { "Ad", "Admin" }
                });

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserTypes",
                keyColumn: "UserType",
                keyValues: new object[] { "Us", "Ad" });

        }
    }
}
