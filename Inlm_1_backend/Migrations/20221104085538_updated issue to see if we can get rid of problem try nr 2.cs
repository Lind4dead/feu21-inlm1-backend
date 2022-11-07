using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inlm_1_backend.Migrations
{
    public partial class updatedissuetoseeifwecangetridofproblemtrynr2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issues_Users_UserId",
                table: "Issues");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Issues",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_Users_UserId",
                table: "Issues",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issues_Users_UserId",
                table: "Issues");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Issues",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_Users_UserId",
                table: "Issues",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
