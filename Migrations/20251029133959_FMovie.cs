using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieWebApp.Migrations
{
    /// <inheritdoc />
    public partial class FMovie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cast",
                table: "movies");

            migrationBuilder.DropColumn(
                name: "Director",
                table: "movies");

            migrationBuilder.DropColumn(
                name: "Episodes",
                table: "movies");

            migrationBuilder.DropColumn(
                name: "TrailerUrl",
                table: "movies");

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Cast",
                table: "movies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Director",
                table: "movies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Episodes",
                table: "movies",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrailerUrl",
                table: "movies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
