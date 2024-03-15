using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SomeSandwich.FakeMentorus.Infrastructure.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddIsFinalToGradeCompositionAndRemoveIsFinishFromCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFinished",
                table: "Courses");

            migrationBuilder.AddColumn<bool>(
                name: "IsFinal",
                table: "GradeCompositions",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFinal",
                table: "GradeCompositions");

            migrationBuilder.AddColumn<bool>(
                name: "IsFinished",
                table: "Courses",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
