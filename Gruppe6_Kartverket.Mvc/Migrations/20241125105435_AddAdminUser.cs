using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.AspNetCore.Identity;
using System;

#nullable disable

namespace Gruppe6_Kartverket.Mvc.Migrations
{
    public partial class AddAdminUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var userId = Guid.NewGuid().ToString();
            var registrationDate = DateTime.UtcNow;
            var passwordHasher = new PasswordHasher<IdentityUser>();
            var dummyUser = new IdentityUser();
            var passwordHash = passwordHasher.HashPassword(dummyUser, "Admin123");

            

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "UserName", "NormalizedUserName", "Email", "NormalizedEmail", "EmailConfirmed", "PasswordHash", "SecurityStamp", "ConcurrencyStamp", "PhoneNumber", "PhoneNumberConfirmed", "TwoFactorEnabled", "LockoutEnd", "LockoutEnabled", "AccessFailedCount" },
                values: new object[] { userId, "Admin", "ADMIN", "Admin@email.com", "ADMIN@EMAIL.COM", true, passwordHash, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "00", true, false, null, true, 0}
            );
            
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "UserName", "UserType", "UserPassword" },
                values: new object[] { userId, "Admin", "Ad", "Admin123" }
            );

            migrationBuilder.InsertData(
                table: "UserInfos",
                columns: new[] { "UserId", "FirstName", "LastName", "Email", "PhoneNumber", "UserStatus", "RegistrationDate", "Gender" },
                values: new object[] { userId, "Admin", "Admin", "Admin@email.com", "00", "Active", registrationDate, "Other" }
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var userId = "Admin"; // Use the same userId as in the Up method

            migrationBuilder.DeleteData(
                table: "UserInfos",
                keyColumn: "UserId",
                keyValue: userId
            );

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: userId
            );

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: userId
            );
        }
    }
}