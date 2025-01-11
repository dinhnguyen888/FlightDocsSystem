using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightDocsSystem.Migrations
{
    public partial class MotifyAttributeSomeEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SignaturePath",
                table: "Flights",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DocumentPath",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "FileStores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileInEntity = table.Column<int>(type: "int", nullable: false),
                    EntityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileStores", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileStores_FileName",
                table: "FileStores",
                column: "FileName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileStores");

            migrationBuilder.DropColumn(
                name: "SignaturePath",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "DocumentPath",
                table: "Documents");
        }
    }
}
