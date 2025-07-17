using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SuperAdminSeedDataChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { "j3kMC9Ao/DRPde66UPlw8fzS69RSkWqSAk3eiYqAuLI=", "994b6515-08c3-4b9f-a225-286e19c28496" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { "rkEn0+eUY4G4WJco0H0rNtxthXZhxx+n+6P97k8tKc4=", "8c8cf1e7-c946-44f3-a3df-b4f6b1f1b33c" });
        }
    }
}
