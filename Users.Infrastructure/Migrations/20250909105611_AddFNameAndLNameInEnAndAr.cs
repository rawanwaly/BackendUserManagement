using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFNameAndLNameInEnAndAr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Users",
                newName: "LastNameEN");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Users",
                newName: "LastNameAR");

            migrationBuilder.AddColumn<string>(
                name: "FirstNameAR",
                table: "Users",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstNameEN",
                table: "Users",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstNameAR",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FirstNameEN",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "LastNameEN",
                table: "Users",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "LastNameAR",
                table: "Users",
                newName: "FirstName");
        }
    }
}
