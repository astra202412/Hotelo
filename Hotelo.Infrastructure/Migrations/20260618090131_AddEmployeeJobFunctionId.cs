using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotelo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployeeJobFunctionId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "JobFunctionId",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_JobFunctionId",
                table: "Employees",
                column: "JobFunctionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_JobFunctions_JobFunctionId",
                table: "Employees",
                column: "JobFunctionId",
                principalTable: "JobFunctions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_JobFunctions_JobFunctionId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_JobFunctionId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "JobFunctionId",
                table: "Employees");
        }
    }
}
