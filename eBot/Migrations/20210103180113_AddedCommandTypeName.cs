using Microsoft.EntityFrameworkCore.Migrations;

namespace eBot.Migrations
{
    public partial class AddedCommandTypeName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LastCommandTypeName",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastCommandTypeName",
                table: "Users");
        }
    }
}
