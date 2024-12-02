using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Konteh.BackOffice.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddExamStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Exams",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Exams");
        }
    }
}
