using Microsoft.EntityFrameworkCore.Migrations;

namespace Voting.Data.Migrations
{
    public partial class UpdateVoteNodel_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AnswerId",
                table: "Votes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Votes_AnswerId",
                table: "Votes",
                column: "AnswerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_Answers_UserId",
                table: "Votes",
                column: "AnswerId",
                principalTable: "Answers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
