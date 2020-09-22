using Microsoft.EntityFrameworkCore.Migrations;

namespace Voting.Data.Migrations
{
    public partial class UpdateQuestionModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_AspNetUsers_UserId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_UserId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Questions");

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Questions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_OwnerId",
                table: "Questions",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_AspNetUsers_OwnerId",
                table: "Questions",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_AspNetUsers_OwnerId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_OwnerId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Questions");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Questions",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_UserId",
                table: "Questions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_AspNetUsers_UserId",
                table: "Questions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
