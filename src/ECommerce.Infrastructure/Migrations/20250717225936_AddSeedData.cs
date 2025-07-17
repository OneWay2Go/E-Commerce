using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { "FmIw5nInWhDN6lmTExm1c071sg6PDoK1MHwCo2L/PeI=", "73e844e3-d3c7-4442-89b0-9996f52eae9c" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { "SzvRILEzLKYDubeN7aT2/M3c2CxQEZekHzrc462OSic=", "261a0d9d-8cd2-45bf-8ebb-032f9ed3ea36" });
        }
    }
}
