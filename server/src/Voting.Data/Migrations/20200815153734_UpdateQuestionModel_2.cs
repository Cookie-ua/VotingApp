using Microsoft.EntityFrameworkCore.Migrations;

namespace Voting.Data.Migrations
{
    public partial class UpdateQuestionModel_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Questions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Questions");
        }
    }
}
