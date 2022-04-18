using Microsoft.EntityFrameworkCore.Migrations;

namespace USERWebApi.Migrations
{
    public partial class _2nd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_AspNetUsers_RegistrationModelId",
                table: "Patients");

            migrationBuilder.AlterColumn<string>(
                name: "RegistrationModelId",
                table: "Patients",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PatientName",
                table: "Patients",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_AspNetUsers_RegistrationModelId",
                table: "Patients",
                column: "RegistrationModelId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_AspNetUsers_RegistrationModelId",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "PatientName",
                table: "Patients");

            migrationBuilder.AlterColumn<string>(
                name: "RegistrationModelId",
                table: "Patients",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_AspNetUsers_RegistrationModelId",
                table: "Patients",
                column: "RegistrationModelId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
