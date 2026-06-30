using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotelo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCommercialModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "MaintenanceRequests");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "MaintenanceRequests");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "MaintenanceRequests");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "MaintenanceRequests");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "HousekeepingTasks");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "HousekeepingTasks");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "HousekeepingTasks");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "HousekeepingTasks");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "MaintenanceRequests",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "HousekeepingTasks",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactPerson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiscountRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contracts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RoomRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RoomsQuota = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contracts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contracts_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_ReservationId",
                table: "Invoices",
                column: "ReservationId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_CompanyId",
                table: "Contracts",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Reservations_ReservationId",
                table: "Invoices",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Reservations_ReservationId",
                table: "Invoices");

            migrationBuilder.DropTable(
                name: "Contracts");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_ReservationId",
                table: "Invoices");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "MaintenanceRequests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "MaintenanceRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "MaintenanceRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "MaintenanceRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "MaintenanceRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "HousekeepingTasks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "HousekeepingTasks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "HousekeepingTasks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "HousekeepingTasks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "HousekeepingTasks",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
