using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotelo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminModule2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppModules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppModules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobFunctions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobFunctions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobFunctions_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAccesses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobFunctionId = table.Column<int>(type: "int", nullable: false),
                    AppModuleId = table.Column<int>(type: "int", nullable: false),
                    CanView = table.Column<bool>(type: "bit", nullable: false),
                    CanCreate = table.Column<bool>(type: "bit", nullable: false),
                    CanEdit = table.Column<bool>(type: "bit", nullable: false),
                    CanDelete = table.Column<bool>(type: "bit", nullable: false),
                    CanExport = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccesses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAccesses_AppModules_AppModuleId",
                        column: x => x.AppModuleId,
                        principalTable: "AppModules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAccesses_JobFunctions_JobFunctionId",
                        column: x => x.JobFunctionId,
                        principalTable: "JobFunctions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserJobFunctions",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    JobFunctionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserJobFunctions", x => new { x.UserId, x.JobFunctionId });
                    table.ForeignKey(
                        name: "FK_UserJobFunctions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserJobFunctions_JobFunctions_JobFunctionId",
                        column: x => x.JobFunctionId,
                        principalTable: "JobFunctions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobFunctions_DepartmentId",
                table: "JobFunctions",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccesses_AppModuleId",
                table: "UserAccesses",
                column: "AppModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccesses_JobFunctionId",
                table: "UserAccesses",
                column: "JobFunctionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserJobFunctions_JobFunctionId",
                table: "UserJobFunctions",
                column: "JobFunctionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAccesses");

            migrationBuilder.DropTable(
                name: "UserJobFunctions");

            migrationBuilder.DropTable(
                name: "AppModules");

            migrationBuilder.DropTable(
                name: "JobFunctions");
        }
    }
}
