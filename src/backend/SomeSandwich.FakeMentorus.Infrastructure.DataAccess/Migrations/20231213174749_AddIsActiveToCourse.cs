using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SomeSandwich.FakeMentorus.Infrastructure.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddIsActiveToCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Requests",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "GradeCompositions",
                type: "text",
                unicode: false,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldUnicode: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "GradeCompositions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActivated",
                table: "Courses",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "GradeCompositions");

            migrationBuilder.DropColumn(
                name: "IsActivated",
                table: "Courses");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "GradeCompositions",
                type: "text",
                unicode: false,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldUnicode: false,
                oldNullable: true);
        }
    }
}
