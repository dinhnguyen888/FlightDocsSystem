using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightDocsSystem.Migrations
{
    public partial class UpdateAndAddEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_accounts_groupPermissions_PermissionName",
                table: "accounts");

            migrationBuilder.DropTable(
                name: "groupPermissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_accounts",
                table: "accounts");

            migrationBuilder.RenameTable(
                name: "accounts",
                newName: "Accounts");

            migrationBuilder.RenameColumn(
                name: "PermissionName",
                table: "Accounts",
                newName: "RoleName");

            migrationBuilder.RenameIndex(
                name: "IX_accounts_Email",
                table: "Accounts",
                newName: "IX_Accounts_Email");

            migrationBuilder.RenameIndex(
                name: "IX_accounts_PermissionName",
                table: "Accounts",
                newName: "IX_Accounts_RoleName");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "DocsTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocsTypeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Creator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocsTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Flights",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Route = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartureDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ArrivalDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PointOfLoading = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PointOfUnloading = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FlightStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flights", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Creator = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                    table.UniqueConstraint("AK_Roles_RoleName", x => x.RoleName);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    DocumentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DocsTypeId = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Creator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastestVersion = table.Column<float>(type: "real", nullable: false),
                    FlightId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.DocumentId);
                    table.ForeignKey(
                        name: "FK_Documents_DocsTypes_DocsTypeId",
                        column: x => x.DocsTypeId,
                        principalTable: "DocsTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Documents_Flights_FlightId",
                        column: x => x.FlightId,
                        principalTable: "Flights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    DocsTypeId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    PermissionType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => new { x.DocsTypeId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_Permissions_DocsTypes_DocsTypeId",
                        column: x => x.DocsTypeId,
                        principalTable: "DocsTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Permissions_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Documents_DocsTypeId",
                table: "Documents",
                column: "DocsTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_FlightId",
                table: "Documents",
                column: "FlightId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_RoleId",
                table: "Permissions",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_RoleName",
                table: "Roles",
                column: "RoleName",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Roles_RoleName",
                table: "Accounts",
                column: "RoleName",
                principalTable: "Roles",
                principalColumn: "RoleName",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Roles_RoleName",
                table: "Accounts");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "Flights");

            migrationBuilder.DropTable(
                name: "DocsTypes");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts");

            migrationBuilder.RenameTable(
                name: "Accounts",
                newName: "accounts");

            migrationBuilder.RenameColumn(
                name: "RoleName",
                table: "accounts",
                newName: "PermissionName");

            migrationBuilder.RenameIndex(
                name: "IX_Accounts_Email",
                table: "accounts",
                newName: "IX_accounts_Email");

            migrationBuilder.RenameIndex(
                name: "IX_Accounts_RoleName",
                table: "accounts",
                newName: "IX_accounts_PermissionName");

            migrationBuilder.AddPrimaryKey(
                name: "PK_accounts",
                table: "accounts",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "groupPermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Creator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GroupPermissionName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_groupPermissions", x => x.Id);
                    table.UniqueConstraint("AK_groupPermissions_GroupPermissionName", x => x.GroupPermissionName);
                });

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
    }
}
