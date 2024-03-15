using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SomeSandwich.FakeMentorus.Infrastructure.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddInviteCodeToCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentRequest_AspNetUsers_UserId",
                table: "CommentRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_CommentRequest_Request_RequestId",
                table: "CommentRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseStudent_AspNetUsers_StudentId",
                table: "CourseStudent");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseStudent_Course_CourseId",
                table: "CourseStudent");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseTeacher_AspNetUsers_TeacherId",
                table: "CourseTeacher");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseTeacher_Course_CourseId",
                table: "CourseTeacher");

            migrationBuilder.DropForeignKey(
                name: "FK_Grade_AspNetUsers_StudentId",
                table: "Grade");

            migrationBuilder.DropForeignKey(
                name: "FK_Grade_GradeComposition_GradeCompositionId",
                table: "Grade");

            migrationBuilder.DropForeignKey(
                name: "FK_GradeComposition_Course_CourseId",
                table: "GradeComposition");

            migrationBuilder.DropForeignKey(
                name: "FK_Request_AspNetUsers_StudentId",
                table: "Request");

            migrationBuilder.DropForeignKey(
                name: "FK_Request_Course_CourseId",
                table: "Request");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Request",
                table: "Request");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GradeComposition",
                table: "GradeComposition");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Grade",
                table: "Grade");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseTeacher",
                table: "CourseTeacher");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseStudent",
                table: "CourseStudent");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Course",
                table: "Course");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommentRequest",
                table: "CommentRequest");

            migrationBuilder.RenameTable(
                name: "Request",
                newName: "Requests");

            migrationBuilder.RenameTable(
                name: "GradeComposition",
                newName: "GradeCompositions");

            migrationBuilder.RenameTable(
                name: "Grade",
                newName: "Grades");

            migrationBuilder.RenameTable(
                name: "CourseTeacher",
                newName: "CourseTeachers");

            migrationBuilder.RenameTable(
                name: "CourseStudent",
                newName: "CourseStudents");

            migrationBuilder.RenameTable(
                name: "Course",
                newName: "Courses");

            migrationBuilder.RenameTable(
                name: "CommentRequest",
                newName: "CommentRequests");

            migrationBuilder.RenameIndex(
                name: "IX_Request_StudentId",
                table: "Requests",
                newName: "IX_Requests_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_Request_CourseId",
                table: "Requests",
                newName: "IX_Requests_CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_GradeComposition_CourseId",
                table: "GradeCompositions",
                newName: "IX_GradeCompositions_CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_Grade_StudentId",
                table: "Grades",
                newName: "IX_Grades_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_Grade_GradeCompositionId",
                table: "Grades",
                newName: "IX_Grades_GradeCompositionId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseTeacher_TeacherId",
                table: "CourseTeachers",
                newName: "IX_CourseTeachers_TeacherId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseTeacher_CourseId",
                table: "CourseTeachers",
                newName: "IX_CourseTeachers_CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseStudent_StudentId",
                table: "CourseStudents",
                newName: "IX_CourseStudents_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseStudent_CourseId",
                table: "CourseStudents",
                newName: "IX_CourseStudents_CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_CommentRequest_UserId",
                table: "CommentRequests",
                newName: "IX_CommentRequests_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_CommentRequest_RequestId",
                table: "CommentRequests",
                newName: "IX_CommentRequests_RequestId");

            migrationBuilder.AddColumn<string>(
                name: "InviteCode",
                table: "Courses",
                type: "text",
                unicode: false,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Requests",
                table: "Requests",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GradeCompositions",
                table: "GradeCompositions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Grades",
                table: "Grades",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseTeachers",
                table: "CourseTeachers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseStudents",
                table: "CourseStudents",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Courses",
                table: "Courses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommentRequests",
                table: "CommentRequests",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentRequests_AspNetUsers_UserId",
                table: "CommentRequests",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CommentRequests_Requests_RequestId",
                table: "CommentRequests",
                column: "RequestId",
                principalTable: "Requests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseStudents_AspNetUsers_StudentId",
                table: "CourseStudents",
                column: "StudentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseStudents_Courses_CourseId",
                table: "CourseStudents",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseTeachers_AspNetUsers_TeacherId",
                table: "CourseTeachers",
                column: "TeacherId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseTeachers_Courses_CourseId",
                table: "CourseTeachers",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GradeCompositions_Courses_CourseId",
                table: "GradeCompositions",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_AspNetUsers_StudentId",
                table: "Grades",
                column: "StudentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_GradeCompositions_GradeCompositionId",
                table: "Grades",
                column: "GradeCompositionId",
                principalTable: "GradeCompositions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_AspNetUsers_StudentId",
                table: "Requests",
                column: "StudentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Courses_CourseId",
                table: "Requests",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentRequests_AspNetUsers_UserId",
                table: "CommentRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_CommentRequests_Requests_RequestId",
                table: "CommentRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseStudents_AspNetUsers_StudentId",
                table: "CourseStudents");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseStudents_Courses_CourseId",
                table: "CourseStudents");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseTeachers_AspNetUsers_TeacherId",
                table: "CourseTeachers");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseTeachers_Courses_CourseId",
                table: "CourseTeachers");

            migrationBuilder.DropForeignKey(
                name: "FK_GradeCompositions_Courses_CourseId",
                table: "GradeCompositions");

            migrationBuilder.DropForeignKey(
                name: "FK_Grades_AspNetUsers_StudentId",
                table: "Grades");

            migrationBuilder.DropForeignKey(
                name: "FK_Grades_GradeCompositions_GradeCompositionId",
                table: "Grades");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_AspNetUsers_StudentId",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Courses_CourseId",
                table: "Requests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Requests",
                table: "Requests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Grades",
                table: "Grades");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GradeCompositions",
                table: "GradeCompositions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseTeachers",
                table: "CourseTeachers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseStudents",
                table: "CourseStudents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Courses",
                table: "Courses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommentRequests",
                table: "CommentRequests");

            migrationBuilder.DropColumn(
                name: "InviteCode",
                table: "Courses");

            migrationBuilder.RenameTable(
                name: "Requests",
                newName: "Request");

            migrationBuilder.RenameTable(
                name: "Grades",
                newName: "Grade");

            migrationBuilder.RenameTable(
                name: "GradeCompositions",
                newName: "GradeComposition");

            migrationBuilder.RenameTable(
                name: "CourseTeachers",
                newName: "CourseTeacher");

            migrationBuilder.RenameTable(
                name: "CourseStudents",
                newName: "CourseStudent");

            migrationBuilder.RenameTable(
                name: "Courses",
                newName: "Course");

            migrationBuilder.RenameTable(
                name: "CommentRequests",
                newName: "CommentRequest");

            migrationBuilder.RenameIndex(
                name: "IX_Requests_StudentId",
                table: "Request",
                newName: "IX_Request_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_Requests_CourseId",
                table: "Request",
                newName: "IX_Request_CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_Grades_StudentId",
                table: "Grade",
                newName: "IX_Grade_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_Grades_GradeCompositionId",
                table: "Grade",
                newName: "IX_Grade_GradeCompositionId");

            migrationBuilder.RenameIndex(
                name: "IX_GradeCompositions_CourseId",
                table: "GradeComposition",
                newName: "IX_GradeComposition_CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseTeachers_TeacherId",
                table: "CourseTeacher",
                newName: "IX_CourseTeacher_TeacherId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseTeachers_CourseId",
                table: "CourseTeacher",
                newName: "IX_CourseTeacher_CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseStudents_StudentId",
                table: "CourseStudent",
                newName: "IX_CourseStudent_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseStudents_CourseId",
                table: "CourseStudent",
                newName: "IX_CourseStudent_CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_CommentRequests_UserId",
                table: "CommentRequest",
                newName: "IX_CommentRequest_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_CommentRequests_RequestId",
                table: "CommentRequest",
                newName: "IX_CommentRequest_RequestId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Request",
                table: "Request",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Grade",
                table: "Grade",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GradeComposition",
                table: "GradeComposition",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseTeacher",
                table: "CourseTeacher",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseStudent",
                table: "CourseStudent",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Course",
                table: "Course",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommentRequest",
                table: "CommentRequest",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentRequest_AspNetUsers_UserId",
                table: "CommentRequest",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CommentRequest_Request_RequestId",
                table: "CommentRequest",
                column: "RequestId",
                principalTable: "Request",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseStudent_AspNetUsers_StudentId",
                table: "CourseStudent",
                column: "StudentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseStudent_Course_CourseId",
                table: "CourseStudent",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseTeacher_AspNetUsers_TeacherId",
                table: "CourseTeacher",
                column: "TeacherId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseTeacher_Course_CourseId",
                table: "CourseTeacher",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Grade_AspNetUsers_StudentId",
                table: "Grade",
                column: "StudentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Grade_GradeComposition_GradeCompositionId",
                table: "Grade",
                column: "GradeCompositionId",
                principalTable: "GradeComposition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GradeComposition_Course_CourseId",
                table: "GradeComposition",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Request_AspNetUsers_StudentId",
                table: "Request",
                column: "StudentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Request_Course_CourseId",
                table: "Request",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
