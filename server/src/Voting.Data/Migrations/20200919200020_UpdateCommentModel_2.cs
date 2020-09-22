using Microsoft.EntityFrameworkCore.Migrations;

namespace Voting.Data.Migrations
{
    public partial class UpdateCommentModel_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RecipientId",
                table: "Comments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_RecipientId",
                table: "Comments",
                column: "RecipientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_RecipientId",
                table: "Comments",
                column: "RecipientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_RecipientId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_RecipientId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "RecipientId",
                table: "Comments");
        }
    }
}
