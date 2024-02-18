using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.API.Migrations
{
    /// <inheritdoc />
    public partial class AddedEducationMaterials : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EducationMaterial",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttachmentId = table.Column<int>(type: "int", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationMaterial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EducationMaterial_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EducationMaterial_Attachments_AttachmentId",
                        column: x => x.AttachmentId,
                        principalTable: "Attachments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EducationMaterial_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EducationMaterialGroup",
                columns: table => new
                {
                    EducationMaterialId = table.Column<int>(type: "int", nullable: false),
                    GroupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationMaterialGroup", x => new { x.EducationMaterialId, x.GroupId });
                    table.ForeignKey(
                        name: "FK_EducationMaterialGroup_EducationMaterial_EducationMaterialId",
                        column: x => x.EducationMaterialId,
                        principalTable: "EducationMaterial",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EducationMaterialGroup_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EducationMaterial_AttachmentId",
                table: "EducationMaterial",
                column: "AttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_EducationMaterial_CreatedById",
                table: "EducationMaterial",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_EducationMaterial_SubjectId",
                table: "EducationMaterial",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_EducationMaterialGroup_GroupId",
                table: "EducationMaterialGroup",
                column: "GroupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EducationMaterialGroup");

            migrationBuilder.DropTable(
                name: "EducationMaterial");
        }
    }
}
