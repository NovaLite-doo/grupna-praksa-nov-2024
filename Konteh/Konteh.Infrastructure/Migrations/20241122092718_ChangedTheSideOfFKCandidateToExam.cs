using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Konteh.BackOffice.Api.Migrations
{
    /// <inheritdoc />
    public partial class ChangedTheSideOfFKCandidateToExam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candidates_Exams_ExamId",
                table: "Candidates");

            migrationBuilder.DropForeignKey(
                name: "FK_ExamQuestions_Exams_QuestionId",
                table: "ExamQuestions");

            migrationBuilder.DropIndex(
                name: "IX_Candidates_ExamId",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "ExamId",
                table: "Candidates");

            migrationBuilder.AddColumn<int>(
                name: "CandidateId",
                table: "Exams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Exams_CandidateId",
                table: "Exams",
                column: "CandidateId",
                unique: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_Candidates_CandidateId",
                table: "Exams",
                column: "CandidateId",
                principalTable: "Candidates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamQuestions_Exams_ExamId",
                table: "ExamQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_Exams_Candidates_CandidateId",
                table: "Exams");

            migrationBuilder.DropIndex(
                name: "IX_Exams_CandidateId",
                table: "Exams");

            migrationBuilder.DropIndex(
                name: "IX_ExamQuestions_ExamId",
                table: "ExamQuestions");

            migrationBuilder.DropColumn(
                name: "CandidateId",
                table: "Exams");

            migrationBuilder.AddColumn<int>(
                name: "ExamId",
                table: "Candidates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_ExamId",
                table: "Candidates",
                column: "ExamId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Candidates_Exams_ExamId",
                table: "Candidates",
                column: "ExamId",
                principalTable: "Exams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
