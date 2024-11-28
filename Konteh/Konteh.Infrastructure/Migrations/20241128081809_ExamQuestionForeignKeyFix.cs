using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Konteh.BackOffice.Api.Migrations
{
    /// <inheritdoc />
    public partial class ExamQuestionForeignKeyFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamQuestions_Exams_QuestionId",
                table: "ExamQuestions");

            migrationBuilder.CreateIndex(
                name: "IX_ExamQuestions_ExamId",
                table: "ExamQuestions",
                column: "ExamId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamQuestions_Exams_ExamId",
                table: "ExamQuestions",
                column: "ExamId",
                principalTable: "Exams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamQuestions_Exams_ExamId",
                table: "ExamQuestions");

            migrationBuilder.DropIndex(
                name: "IX_ExamQuestions_ExamId",
                table: "ExamQuestions");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamQuestions_Exams_QuestionId",
                table: "ExamQuestions",
                column: "QuestionId",
                principalTable: "Exams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
