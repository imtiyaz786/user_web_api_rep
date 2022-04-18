using Microsoft.EntityFrameworkCore.Migrations;

namespace USERWebApi.Migrations
{
    public partial class init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PatientName",
                table: "Patients");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Patients",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Patients");

            migrationBuilder.AddColumn<string>(
                name: "PatientName",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
