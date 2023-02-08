using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.API.Migrations
{
    /// <inheritdoc />
    public partial class addedConfirmCodes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0b580274-d464-4143-bed4-f5c122a9dac6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4bc40ccb-f916-4d29-930a-e976800362a5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "78b946d5-b8fb-4ae2-8fff-0e8dd805907e");

            migrationBuilder.CreateTable(
                name: "ConfirmCodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Code = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfirmCodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConfirmCodes_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "38ba12c9-c9fc-46b7-9314-90afa2cf6128", "0ab5fec8-f1ba-4d1d-aee1-9567994f914a", "Admin", "ADMIN" },
                    { "3a8b6db2-6d18-4e05-a1d0-65e7637710b1", "9c9f5714-846a-4d48-8995-48990842e250", "Student", "STUDENT" },
                    { "8983ecc8-25d8-4e98-a120-d60896fe290e", "b59f0ae9-760f-4ff8-a9ab-b4cb78ccfd65", "Teacher", "TEACHER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConfirmCodes_UserID",
                table: "ConfirmCodes",
                column: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfirmCodes");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "38ba12c9-c9fc-46b7-9314-90afa2cf6128");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3a8b6db2-6d18-4e05-a1d0-65e7637710b1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8983ecc8-25d8-4e98-a120-d60896fe290e");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0b580274-d464-4143-bed4-f5c122a9dac6", "859d3e18-23dc-4b03-9ace-64795acfb967", "Student", "STUDENT" },
                    { "4bc40ccb-f916-4d29-930a-e976800362a5", "5292eb8b-2452-489c-a69f-5bbc89e704c6", "Admin", "ADMIN" },
                    { "78b946d5-b8fb-4ae2-8fff-0e8dd805907e", "67e7849d-dc59-4a8d-a522-58c092b435f5", "Teacher", "TEACHER" }
                });
        }
    }
}
