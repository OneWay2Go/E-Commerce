using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDeletedToCoupon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Coupons",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Code", "PasswordHash", "PasswordSalt" },
                values: new object[] { "462be9e4-02d1-4f4d-a448-51aeecd2f49f", "/oCUaTkcpawdvocGm4Tt9tQor5EPciM8m1UyhhCW6O8=", "ded9cd86-f2aa-40fa-b0ed-81e546ddd6f5" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Coupons");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Code", "PasswordHash", "PasswordSalt" },
                values: new object[] { "45f386d4-8b56-4b4b-b717-01bcc044de0b", "sw18e0ajGQHyLtExB9eLX6xCsdxeGjxTxpgk2PJvfec=", "5d9acc28-36a4-42fe-b604-9c8b6ad89621" });
        }
    }
}
