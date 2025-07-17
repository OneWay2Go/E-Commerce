using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UserUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Code", "PasswordHash", "PasswordSalt" },
                values: new object[] { "45f386d4-8b56-4b4b-b717-01bcc044de0b", "sw18e0ajGQHyLtExB9eLX6xCsdxeGjxTxpgk2PJvfec=", "5d9acc28-36a4-42fe-b604-9c8b6ad89621" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { "j3kMC9Ao/DRPde66UPlw8fzS69RSkWqSAk3eiYqAuLI=", "994b6515-08c3-4b9f-a225-286e19c28496" });
        }
    }
}
