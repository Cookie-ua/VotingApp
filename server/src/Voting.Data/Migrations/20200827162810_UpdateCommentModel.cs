using Microsoft.EntityFrameworkCore.Migrations;

namespace Voting.Data.Migrations
{
    public partial class UpdateCommentModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Comments");

            migrationBuilder.AddColumn<bool>(
                name: "IsBlocked",
                table: "Comments",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBlocked",
                table: "Comments");

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "Comments",
                type: "int",
                nullable: true);
        }
    }
}
