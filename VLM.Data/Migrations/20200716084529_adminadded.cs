using Microsoft.EntityFrameworkCore.Migrations;

namespace VLM.Data.Migrations
{
    public partial class adminadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Admin");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Admin",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Admin",
                keyColumn: "AdminId",
                keyValue: 15031999,
                columns: new[] { "Password", "UserName" },
                values: new object[] { "adminadmin", "admin1999" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Admin");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Admin",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Admin",
                keyColumn: "AdminId",
                keyValue: 15031999,
                columns: new[] { "Email", "Password" },
                values: new object[] { "surisagar@gmail.com", "Sagarsuri" });
        }
    }
}
