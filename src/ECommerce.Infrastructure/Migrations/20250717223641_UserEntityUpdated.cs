using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UserEntityUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsEmailConfirmed",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { "bA6fwuSyVEjeambH2YK+IBlXPgE1QfQC8mi71reo3CY=", "87b8fd1a-0f0e-4f29-8293-cd3ce95efed8" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsEmailConfirmed",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Code", "IsEmailConfirmed", "PasswordHash", "PasswordSalt" },
                values: new object[] { "462be9e4-02d1-4f4d-a448-51aeecd2f49f", true, "/oCUaTkcpawdvocGm4Tt9tQor5EPciM8m1UyhhCW6O8=", "ded9cd86-f2aa-40fa-b0ed-81e546ddd6f5" });
        }
    }
}
