using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Konteh.BackOffice.Api.Migrations
{
    /// <inheritdoc />
    public partial class Migration1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnswerExamQuestion_Answers_SubmittedAnswersId",
                table: "AnswerExamQuestion");

            migrationBuilder.DropForeignKey(
                name: "FK_AnswerExamQuestion_ExamQuestions_ExamQuestionId",
                table: "AnswerExamQuestion");

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerExamQuestion_Answers_SubmittedAnswersId",
                table: "AnswerExamQuestion",
                column: "SubmittedAnswersId",
                principalTable: "Answers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerExamQuestion_ExamQuestions_ExamQuestionId",
                table: "AnswerExamQuestion",
                column: "ExamQuestionId",
                principalTable: "ExamQuestions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnswerExamQuestion_Answers_SubmittedAnswersId",
                table: "AnswerExamQuestion");

            migrationBuilder.DropForeignKey(
                name: "FK_AnswerExamQuestion_ExamQuestions_ExamQuestionId",
                table: "AnswerExamQuestion");

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerExamQuestion_Answers_SubmittedAnswersId",
                table: "AnswerExamQuestion",
                column: "SubmittedAnswersId",
                principalTable: "Answers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerExamQuestion_ExamQuestions_ExamQuestionId",
                table: "AnswerExamQuestion",
                column: "ExamQuestionId",
                principalTable: "ExamQuestions",
                principalColumn: "Id");
        }
    }
}
