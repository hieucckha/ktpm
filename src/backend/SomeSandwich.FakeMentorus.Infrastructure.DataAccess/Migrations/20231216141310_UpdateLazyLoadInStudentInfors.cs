using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SomeSandwich.FakeMentorus.Infrastructure.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLazyLoadInStudentInfors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StudentInfos_StudentId",
                table: "StudentInfos");

            migrationBuilder.CreateIndex(
                name: "IX_StudentInfos_StudentId",
                table: "StudentInfos",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StudentInfos_StudentId",
                table: "StudentInfos");

            migrationBuilder.CreateIndex(
                name: "IX_StudentInfos_StudentId",
                table: "StudentInfos",
                column: "StudentId",
                unique: true);
        }
    }
}
