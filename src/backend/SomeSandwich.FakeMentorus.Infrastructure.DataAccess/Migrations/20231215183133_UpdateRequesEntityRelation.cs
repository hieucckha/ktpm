using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SomeSandwich.FakeMentorus.Infrastructure.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRequesEntityRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Courses_CourseId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_CourseId",
                table: "Requests");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "Requests",
                newName: "GradeId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_GradeId",
                table: "Requests",
                column: "GradeId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Grades_GradeId",
                table: "Requests",
                column: "GradeId",
                principalTable: "Grades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Grades_GradeId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_GradeId",
                table: "Requests");

            migrationBuilder.RenameColumn(
                name: "GradeId",
                table: "Requests",
                newName: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_CourseId",
                table: "Requests",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Courses_CourseId",
                table: "Requests",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
