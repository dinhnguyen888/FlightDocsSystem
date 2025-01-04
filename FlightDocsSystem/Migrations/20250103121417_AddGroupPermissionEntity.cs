using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightDocsSystem.Migrations
{
    public partial class AddGroupPermissionEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "accounts",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "PermissionName",
                table: "accounts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "groupPermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupPermissionName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Creator = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_groupPermissions", x => x.Id);
                    table.UniqueConstraint("AK_groupPermissions_GroupPermissionName", x => x.GroupPermissionName);
                });

            migrationBuilder.CreateIndex(
                name: "IX_accounts_Email",
                table: "accounts",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_accounts_PermissionName",
                table: "accounts",
                column: "PermissionName");

            migrationBuilder.CreateIndex(
                name: "IX_groupPermissions_GroupPermissionName",
                table: "groupPermissions",
                column: "GroupPermissionName",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_accounts_groupPermissions_PermissionName",
                table: "accounts",
                column: "PermissionName",
                principalTable: "groupPermissions",
                principalColumn: "GroupPermissionName",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_accounts_groupPermissions_PermissionName",
                table: "accounts");

            migrationBuilder.DropTable(
                name: "groupPermissions");

            migrationBuilder.DropIndex(
                name: "IX_accounts_Email",
                table: "accounts");

            migrationBuilder.DropIndex(
                name: "IX_accounts_PermissionName",
                table: "accounts");

            migrationBuilder.DropColumn(
                name: "PermissionName",
                table: "accounts");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "accounts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
