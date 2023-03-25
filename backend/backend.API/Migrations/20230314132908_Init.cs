using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.API.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1548c5b9-11a8-40ac-a955-1b065c52da42", "ea6a03d5-c051-49d3-953f-9ea6babb5245", "Student", "STUDENT" },
                    { "fbc4bb8c-980d-4f38-ba1f-3d4c62e65e80", "46ca54ad-93be-4aa9-8e9b-004d576d268a", "Admin", "ADMIN" },
                    { "fcd7a980-c8d9-4217-8c21-31ede9b72cc9", "1083da9f-caa5-4fdd-9e11-9dbc4e6b7ba3", "Teacher", "TEACHER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1548c5b9-11a8-40ac-a955-1b065c52da42");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fbc4bb8c-980d-4f38-ba1f-3d4c62e65e80");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fcd7a980-c8d9-4217-8c21-31ede9b72cc9");
        }
    }
}
