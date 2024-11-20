using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Konteh.BackOffice.Api.Migrations
{
    /// <inheritdoc />
    public partial class AnswerLogicalDeleteMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Answers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Answers");
        }
    }
}
