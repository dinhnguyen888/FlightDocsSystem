using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightDocsSystem.Migrations
{
    public partial class ModifySomeEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileStores");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FileStores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityId = table.Column<int>(type: "int", nullable: false),
                    FileInEntity = table.Column<int>(type: "int", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
    }
}
