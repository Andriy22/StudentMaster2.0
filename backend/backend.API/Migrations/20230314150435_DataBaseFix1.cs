using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.API.Migrations
{
    /// <inheritdoc />
    public partial class DataBaseFix1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubjectId",
                table: "Works",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Works_SubjectId",
                table: "Works",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Works_Subjects_SubjectId",
                table: "Works",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Works_Subjects_SubjectId",
                table: "Works");

            migrationBuilder.DropIndex(
                name: "IX_Works_SubjectId",
                table: "Works");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "Works");
        }
    }
}
