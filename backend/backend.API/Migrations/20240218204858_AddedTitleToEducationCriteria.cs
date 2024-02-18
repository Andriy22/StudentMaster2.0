using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.API.Migrations
{
    /// <inheritdoc />
    public partial class AddedTitleToEducationCriteria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "EducationMaterial",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "EducationMaterial");
        }
    }
}
