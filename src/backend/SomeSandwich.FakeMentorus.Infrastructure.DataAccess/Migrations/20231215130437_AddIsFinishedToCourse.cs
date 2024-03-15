using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SomeSandwich.FakeMentorus.Infrastructure.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddIsFinishedToCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFinished",
                table: "Courses",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFinished",
                table: "Courses");
        }
    }
}
