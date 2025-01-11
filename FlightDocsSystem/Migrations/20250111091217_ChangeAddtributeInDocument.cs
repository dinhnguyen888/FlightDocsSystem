using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightDocsSystem.Migrations
{
    public partial class ChangeAddtributeInDocument : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "LastestVersion",
                table: "Documents",
                type: "float",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "LastestVersion",
                table: "Documents",
                type: "real",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
