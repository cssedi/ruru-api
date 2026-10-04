using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ruru_api.Migrations
{
    /// <inheritdoc />
    public partial class SeedTestUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Age", "Height", "Name", "Surname", "Weight" },
                values: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), 30, 180f, "Test", "User", 75f });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));
        }
    }
}
